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
    //�������� ������
    public float spawnDelay = 0.5f;

    //������� �����
    public int currentWave = 0;
    //������
    public float waveCoolDown = 10.0f;

    public bool inCooldown;
    //����� ��� ����� UI ��  �������
    public float cooldownCounter = 0;

    public List<Enemy> currentZombiesAlive;

    public GameObject zombiePrefab;

    public GameObject waveOverUI;
    //�� ��������......
    //������ ������� �� ������ ��� ���� ������� :_)
    public TextMeshProUGUI cooldownCounterUI;
    public TextMeshProUGUI �urrentWaveUI;

    private void Start()
    {
        currentZombiesPerWave = initialZombiesPerWave;

        GlobalReferences.Instance.waveNumber = currentWave;

        StartNextWave();
    }
    private void Update()
    {
        //�������� � ������ ������� �����/�����
        List<Enemy> zombiesToRemove = new List<Enemy>();
        foreach (Enemy zombie in currentZombiesAlive)
        {
            if (zombie.isDead)
            {
                zombiesToRemove.Add(zombie);
            }
        }
        //������� ����
        foreach (Enemy zombie in zombiesToRemove)
        {
            currentZombiesAlive.Remove(zombie);
        }
        zombiesToRemove.Clear();

        //�������
        if (currentZombiesAlive.Count == 0 && inCooldown == false)
        {
            StartCoroutine(WaveCoolDown());
        }
        //����������� ����� ��������� ������
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
        //�� �������� �������
        //cooldownCounterUI.gameObject.SetActive(false);
        //���������� ����� � ����� ������������� � ��� ����
        currentZombiesPerWave *= mobMultip;
        StartNextWave();
    }

    private void StartNextWave()
    {
        currentZombiesAlive.Clear();

        currentWave++;

        GlobalReferences.Instance.waveNumber = currentWave;

        �urrentWaveUI.text = "�����: " + currentWave.ToString();

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
