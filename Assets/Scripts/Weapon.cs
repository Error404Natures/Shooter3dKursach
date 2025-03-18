using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //для того чтобы знать есть ли у нас оружие в руке
    public bool isActiveWapon;
    public int weaponDamage;

    [Header("Shoot")]
    //стрельба
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    [Header("Burst")]
    //бюрст стрельба по 3 пульки
    public int bulletsPerBurst = 3;
    public int burstBulletLeft;

    [Header("Spread")]
    //спрей от бедра
    public float spreadIntensity;
    //спрей в прицеле, он должен быть меньше
    public float hipSpreadIntensity;
    public float adsSpreadIntensity;

    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30f;
    public float bulletPrefabLideTime = 3f;

    //получаем игровой объект дым
    public GameObject muzzleEffect;
    //ссылка на аниматор
    internal Animator animator;

    [Header("Reloading")]
    //для перезарядки
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    //массив обвесов
    [Header("Array Body kits")]
    public GameObject[] attachments;
    public Transform[] attachmentPoints;


    //хранит положение оружия когда оно в руках игрока
    //не ничего не работает вроде бы
    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    //UI
    public TextMeshProUGUI ammoDisplay;

    //это нужно чтобы анимация перезарядки не воспроизводилась
    //после прицельной стрельбы 
    bool isADS;

    [Header("For Aiming")]
    private CameraShake cameraShake; // Ссылка на скрипт тряски камеры
    private AimingSystem aimingSystem; // Ссылка на скрипт прицеливания

    //перечесление для разных видов оружия
    public enum WeaponModel
    {
        Pistol1911,
        M4,
        AK74
    }

    public WeaponModel thisWeaponModel;

    //перечесление режимов стрельбы
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
                //удержание лкм для непрерывной стрельбы
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Single ||
                currentShootingMode == ShootingMode.Burst)
            {
                //Щелчок лкм один раз для одиночного и бюрстого режима
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);

            }

            //перезарядка
            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && isReloading == false && WeaponManger.Instance.CheckAmmoLeftFor(thisWeaponModel) > 0)
            {
                Reload();
            }
            //автоматическая перезарядка когда магазин пуст
            if (readyToShoot && isShooting == false && isReloading == false && bulletsLeft <= 0)
            {
                //отключил чтобы на время
                // Reload();
            }

            //проверяем можем ли мы стрелять
            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletLeft = bulletsPerBurst;
                FireWeapon();
            }

            //установка обвеса
            if (Input.GetKeyDown(KeyCode.E))
            {
                //На пока я сделаю проще что бы просто подбираем коробку и 
                //это коробка увеличивает урон, потому что у меня сломалась 
                //что-то и теперь установленный прицел не отображается(((
                //НА ЗАМЕТКУ что-то скорее всего с позицией и все в этой части
                for (int i = 0; i < attachments.Length; i++)
                {
                    if (attachments[i] != null && i < attachmentPoints.Length)
                    {
                        attachments[i].transform.SetParent(attachmentPoints[i]); // Установка родителя
                        attachments[i].transform.localPosition = new Vector3((float)0.1556, (float)1.7423, (float)-0.579); // Сброс позиции
                        attachments[i].transform.localRotation = Quaternion.Euler(0, 180, 0);
                        attachments[i].SetActive(true);

                        MeshRenderer meshRenderer = attachments[i].GetComponent<MeshRenderer>();
                        if (meshRenderer != null)
                        {
                            meshRenderer.enabled = true;
                        }
                    }
                }
                //по логике оружие станет мощнее
                //просто хард код напишу для простоты, но управление значением
                //можно сделать через инспектор
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
            //я выключил прицеливание от первого лица
            //Отдачу от прицельной стрельбы
            //animator.SetTrigger("Recoil_ADS");
        }
        else
        {
            animator.SetTrigger("Recoil");
        }

        //тут хардкод и скрипте оружие и в будущем исправлю
        //это нужно чтобы просто разобраться как это работает
        // SoundManager.Instance.shotingSoundM1911.Play();
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        //создаем экземпляр пули
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        Bullet bul = bullet.GetComponent<Bullet>();
        bul.bulletDamage = weaponDamage;

        //направляем пулю в напраавление выстрела
        bullet.transform.forward = shootingDirection;

        //выстреливаем пулю
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        //уничтажаем пулю спустя время
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLideTime));

        //проверка можем ли мы стрелять
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        //проверим бюрст моде и остались ли у нас патроны для стрельбы
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

    //вычислиляет направление и разброс
    private Vector3 CalculateDirectionAndSpread()
    {
        //стрельба из центра экрана для проверки куда м направляем луч рей
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            //удар по чему то
            targetPoint = hit.point;
        }
        else
        {
            //стрельба в воздух
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        //разброс
        float z = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        //возвращает направление стрельбы и спрей
        return direction + new Vector3(0, y, z);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}

