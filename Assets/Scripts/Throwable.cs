using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    //��������� ������
    [SerializeField] float delay = 3.7f;
    [SerializeField] float damageRadius = 23f;
    [SerializeField] float explosionForce = 1200f;

    //��� ����� �������
    float countdown;

    //���� ������� � ����������
    bool hasExploded = false;
    public bool hasBeenThrown = false;

    //������������ ��� ���� ������
    //��� � �������
    public enum ThrowableType
    {
        Grenade
    }
    public ThrowableType throwabletype;

    private void Start()
    {
        //������������� ����� ��������
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

    //�������� �� �������
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
        //���������� ������
        GameObject explosionEffect = GlobalReferences.Instance.grenadeExplosionEffect;
        Instantiate(explosionEffect, transform.position, transform.rotation);

        //��������������� �����
        SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.grenadeSound);


        //���������� ������
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius);
            }
            //��� �� ������� ���� ����� �����
            if (objectInRange.gameObject.GetComponent<Enemy>())
            {
                //���� ������� ������� ���������!!!!
                objectInRange.gameObject.GetComponent<Enemy>().TakeDamage(50);
            }
        }
    }
    
}
