using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MobeSpawner : MonoBehaviour
{
    public int initialZombiesPerWave = 5;
    public int currentZombiesPerWave;
    public int mobMultip = 2;
    //задержка спавна
    public float spawnDelay = 0.5f;

    //счетчик волны
    public int currentWave = 0;
    //таймер
    public float waveCoolDown = 10.0f;

    public bool inCooldown;
    //чисто для теста UI мб  оставлю
    public float cooldownCounter = 0;

    public List<Enemy> currentZombiesAlive;

    public GameObject zombiePrefab;

    public GameObject waveOverUI;
    //не работает......
    //просто отключу но должен был быть счетчик :_)
    public TextMeshProUGUI cooldownCounterUI;
    public TextMeshProUGUI сurrentWaveUI;

    private void Start()
    {
        currentZombiesPerWave = initialZombiesPerWave;

        GlobalReferences.Instance.waveNumber = currentWave;

        StartNextWave();
    }
    private void Update()
    {
        //собираем в список мертвых зомби/мобов
        List<Enemy> zombiesToRemove = new List<Enemy>();
        foreach (Enemy zombie in currentZombiesAlive)
        {
            if (zombie.isDead)
            {
                zombiesToRemove.Add(zombie);
            }
        }
        //удалить всех
        foreach (Enemy zombie in zombiesToRemove)
        {
            currentZombiesAlive.Remove(zombie);
        }
        zombiesToRemove.Clear();

        //кулдаун
        if (currentZombiesAlive.Count == 0 && inCooldown == false)
        {
            StartCoroutine(WaveCoolDown());
        }
        //перезарядка перед следуйщей волной
        if (inCooldown)
        {
            cooldownCounter -= Time.deltaTime;
        }
        else
        {

            cooldownCounter = waveCoolDown;
        }
        cooldownCounterUI.text = cooldownCounter.ToString("F0");
    }

    private IEnumerator WaveCoolDown()
    {
        inCooldown = true;
        waveOverUI.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(waveCoolDown);

        inCooldown = false;
        waveOverUI.gameObject.SetActive(false);
        //не работает счетчик
        //cooldownCounterUI.gameObject.SetActive(false);
        //количество зомби в волне увеличивается в два раза
        currentZombiesPerWave *= mobMultip;
        StartNextWave();
    }

    private void StartNextWave()
    {
        currentZombiesAlive.Clear();

        currentWave++;

        GlobalReferences.Instance.waveNumber = currentWave;

        сurrentWaveUI.text = "Волна: " + currentWave.ToString();

        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < currentZombiesPerWave; i++)
        {
            Vector3 spawnOffet = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffet;

            var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

            Enemy enemyScript = zombie.GetComponent<Enemy>();

            currentZombiesAlive.Add(enemyScript);

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
