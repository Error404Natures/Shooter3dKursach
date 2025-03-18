using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //��� ���� ����� ����� ���� �� � ��� ������ � ����
    public bool isActiveWapon;
    public int weaponDamage;

    [Header("Shoot")]
    //��������
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    [Header("Burst")]
    //����� �������� �� 3 ������
    public int bulletsPerBurst = 3;
    public int burstBulletLeft;

    [Header("Spread")]
    //����� �� �����
    public float spreadIntensity;
    //����� � �������, �� ������ ���� ������
    public float hipSpreadIntensity;
    public float adsSpreadIntensity;

    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30f;
    public float bulletPrefabLideTime = 3f;

    //�������� ������� ������ ���
    public GameObject muzzleEffect;
    //������ �� ��������
    internal Animator animator;

    [Header("Reloading")]
    //��� �����������
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    //������ �������
    [Header("Array Body kits")]
    public GameObject[] attachments;
    public Transform[] attachmentPoints;


    //������ ��������� ������ ����� ��� � ����� ������
    //�� ������ �� �������� ����� ��
    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    //UI
    public TextMeshProUGUI ammoDisplay;

    //��� ����� ����� �������� ����������� �� ����������������
    //����� ���������� �������� 
    bool isADS;

    [Header("For Aiming")]
    private CameraShake cameraShake; // ������ �� ������ ������ ������
    private AimingSystem aimingSystem; // ������ �� ������ ������������

    //������������ ��� ������ ����� ������
    public enum WeaponModel
    {
        Pistol1911,
        M4,
        AK74
    }

    public WeaponModel thisWeaponModel;

    //������������ ������� ��������
    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    private void Start()
    {
        cameraShake = GameObject.FindObjectOfType<CameraShake>();
        aimingSystem = GameObject.FindObjectOfType<AimingSystem>();
    }

    private void Awake()
    {
        readyToShoot = true;
        burstBulletLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();

        bulletsLeft = magazineSize;

        spreadIntensity = hipSpreadIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActiveWapon == true)
        {
            if (Input.GetMouseButtonDown(1))
            {
                EnterADS();
                aimingSystem.EnterAim();
            }
            if (Input.GetMouseButtonUp(1))
            {
                ExitADS();
                aimingSystem.ExitAim();
            }

            if (bulletsLeft == 0 && isShooting)
            {
                SoundManager.Instance.emptyManagizeSoundM1911.Play();
            }

            if (currentShootingMode == ShootingMode.Auto)
            {
                //��������� ��� ��� ����������� ��������
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Single ||
                currentShootingMode == ShootingMode.Burst)
            {
                //������ ��� ���� ��� ��� ���������� � �������� ������
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);

            }

            //�����������
            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && isReloading == false && WeaponManger.Instance.CheckAmmoLeftFor(thisWeaponModel) > 0)
            {
                Reload();
            }
            //�������������� ����������� ����� ������� ����
            if (readyToShoot && isShooting == false && isReloading == false && bulletsLeft <= 0)
            {
                //�������� ����� �� �����
                // Reload();
            }

            //��������� ����� �� �� ��������
            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletLeft = bulletsPerBurst;
                FireWeapon();
            }

            //��������� ������
            if (Input.GetKeyDown(KeyCode.E))
            {
                //�� ���� � ������ ����� ��� �� ������ ��������� ������� � 
                //��� ������� ����������� ����, ������ ��� � ���� ��������� 
                //���-�� � ������ ������������� ������ �� ������������(((
                //�� ������� ���-�� ������ ����� � �������� � ��� � ���� �����
                for (int i = 0; i < attachments.Length; i++)
                {
                    if (attachments[i] != null && i < attachmentPoints.Length)
                    {
                        attachments[i].transform.SetParent(attachmentPoints[i]); // ��������� ��������
                        attachments[i].transform.localPosition = new Vector3((float)0.1556, (float)1.7423, (float)-0.579); // ����� �������
                        attachments[i].transform.localRotation = Quaternion.Euler(0, 180, 0);
                        attachments[i].SetActive(true);

                        MeshRenderer meshRenderer = attachments[i].GetComponent<MeshRenderer>();
                        if (meshRenderer != null)
                        {
                            meshRenderer.enabled = true;
                        }
                    }
                }
                //�� ������ ������ ������ ������
                //������ ���� ��� ������ ��� ��������, �� ���������� ���������
                //����� ������� ����� ���������
                weaponDamage = 50;
            }
        }
    }

    private void EnterADS()
    {
        //animator.SetTrigger("enterADS");
        isADS = true;
        HUDmanager.Instance.middleDot.SetActive(false);
        spreadIntensity = adsSpreadIntensity;
    }
    private void ExitADS()
    {
        //animator.SetTrigger("exitADS");
        isADS = false;
        HUDmanager.Instance.middleDot.SetActive(true);
        spreadIntensity = hipSpreadIntensity;
    }


    private void FireWeapon()
    {

        bulletsLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();

        if (isADS)
        {
            //� �������� ������������ �� ������� ����
            //������ �� ���������� ��������
            //animator.SetTrigger("Recoil_ADS");
        }
        else
        {
            animator.SetTrigger("Recoil");
        }

        //��� ������� � ������� ������ � � ������� ��������
        //��� ����� ����� ������ ����������� ��� ��� ��������
        // SoundManager.Instance.shotingSoundM1911.Play();
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        //������� ��������� ����
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        Bullet bul = bullet.GetComponent<Bullet>();
        bul.bulletDamage = weaponDamage;

        //���������� ���� � ������������ ��������
        bullet.transform.forward = shootingDirection;

        //������������ ����
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        //���������� ���� ������ �����
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLideTime));

        //�������� ����� �� �� ��������
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        //�������� ����� ���� � �������� �� � ��� ������� ��� ��������
        if (currentShootingMode == ShootingMode.Burst && burstBulletLeft > 1)
        {
            burstBulletLeft--;
            Invoke("FireWeapon", shootingDelay);

        }

        if (cameraShake != null)
        {
            cameraShake.Shake();
        }
    }


    private void Reload()
    {
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);

        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        if (WeaponManger.Instance.CheckAmmoLeftFor(thisWeaponModel) > magazineSize)
        {
            bulletsLeft = magazineSize;
            WeaponManger.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        }
        else
        {
            bulletsLeft = WeaponManger.Instance.CheckAmmoLeftFor(thisWeaponModel);
            WeaponManger.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        }

        isReloading = false;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    //����������� ����������� � �������
    private Vector3 CalculateDirectionAndSpread()
    {
        //�������� �� ������ ������ ��� �������� ���� � ���������� ��� ���
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            //���� �� ���� ��
            targetPoint = hit.point;
        }
        else
        {
            //�������� � ������
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        //�������
        float z = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        //���������� ����������� �������� � �����
        return direction + new Vector3(0, y, z);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}

