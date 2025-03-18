using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    //настройки гранат
    [SerializeField] float delay = 3.7f;
    [SerializeField] float damageRadius = 23f;
    [SerializeField] float explosionForce = 1200f;

    //для счета времени
    float countdown;

    //была прощена и взорвалась
    bool hasExploded = false;
    public bool hasBeenThrown = false;

    //перечесление для вида гранат
    //как с оружием
    public enum ThrowableType
    {
        Grenade
    }
    public ThrowableType throwabletype;

    private void Start()
    {
        //устанавливаем время задержки
        countdown = delay;
    }
    private void Update()
    {
        if (hasBeenThrown)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0f && !hasExploded)
            {
                Explode();
                hasExploded = true;
            }
        }
    }

    private void Explode()
    {
        GetThowableEffect();
        Destroy(gameObject);
    }

    //отвечает за эффекты
    private void GetThowableEffect()
    {
        switch (throwabletype)
        {
            case ThrowableType.Grenade:
                GrenadeEffect();
                break;
        }
    }

    private void GrenadeEffect()
    {
        //визуальный эффект
        GameObject explosionEffect = GlobalReferences.Instance.grenadeExplosionEffect;
        Instantiate(explosionEffect, transform.position, transform.rotation);

        //воспроизведение звука
        SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.grenadeSound);


        //физический эффект
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius);
            }
            //так же наносим урон врагу здесь
            if (objectInRange.gameObject.GetComponent<Enemy>())
            {
                //УРОН ГРАНАТЫ НАПИСАЛ ХАРДКОДОМ!!!!
                objectInRange.gameObject.GetComponent<Enemy>().TakeDamage(50);
            }
        }
    }
    
}
