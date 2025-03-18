using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReferences : MonoBehaviour
{
    //тут мы делаем сингалтон
    public static GlobalReferences Instance { get; set; }

    public GameObject bullImpactEffectPrefab;
    
    //это ссылка на эффект взрыва
    public GameObject grenadeExplosionEffect;
    public int waveNumber;

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
}
