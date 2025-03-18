using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterectionManager : MonoBehaviour
{
    //синголтон. Он нужен что бы иметь доступ из либой точки проекта
    public static InterectionManager Instance { get; set; }
    //храним оружие на которое мы наводим оружие
    public Weapon hoveredWeapon = null;
    //храним ящик с патронами на которо
    public AmmoBox hoveredAmmoBox = null;
    
    public Throwable hoveredThrowable = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHitByRaycast = hit.transform.gameObject;

            if (objectHitByRaycast.GetComponent<Weapon>() && objectHitByRaycast.GetComponent<Weapon>().isActiveWapon == false)
            {
                hoveredWeapon = objectHitByRaycast.gameObject.GetComponent<Weapon>();
                hoveredWeapon.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManger.Instance.PickUpWeapon(objectHitByRaycast.gameObject);
                }
            }
            else
            {
                if (hoveredWeapon)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }
            }
            //ammoBox
            if (objectHitByRaycast.GetComponent<AmmoBox>())
            {
                hoveredAmmoBox = objectHitByRaycast.gameObject.GetComponent<AmmoBox>();
                hoveredAmmoBox.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManger.Instance.PickUpAmmo(hoveredAmmoBox);
                    Destroy(objectHitByRaycast.gameObject);
                }
                else
                {
                    if (hoveredAmmoBox)
                    {
                        hoveredAmmoBox.GetComponent<Outline>().enabled = false;
                    }
                }
            }

            //для гранаты
            if (objectHitByRaycast.GetComponent<Throwable>())
            {
                hoveredThrowable = objectHitByRaycast.gameObject.GetComponent<Throwable>();
                hoveredThrowable.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManger.Instance.PickUpThrowable(hoveredThrowable);
                    Destroy(objectHitByRaycast.gameObject);
                }
                else
                {
                    if (hoveredThrowable)
                    {
                        hoveredThrowable.GetComponent<Outline>().enabled = false;
                    }
                }
            }

        }
    }
}