using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    //тут мы делаем сингалтон
    public static SoundManager Instance { get; set; }

    public AudioSource ShootingChannel;
    

    public AudioClip P1911Shoot;
    public AudioClip M4Shoot;

    public AudioSource reloadSoundM4;
    public AudioSource reloadSoundM1911;

    public AudioSource emptyManagizeSoundM1911;

    public AudioSource throwablesChannel;
    public AudioClip grenadeSound;

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
    public void PlayShootingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol1911:
                ShootingChannel.PlayOneShot(P1911Shoot); 
                break;
            case WeaponModel.M4:
                ShootingChannel.PlayOneShot(M4Shoot);
                break;
        }
    }

    public void PlayReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol1911:
                reloadSoundM1911.Play();
                break;
            case WeaponModel.M4:
                //чем это отличается от play
                //он полностью один раз возпроизведет звук
                //и не будет накладываться друг на друга
                reloadSoundM4.Play();
                break;
        }
    }
}
