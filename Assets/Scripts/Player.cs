using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int HP = 100;

    public TextMeshProUGUI playerHetthUI;
    public GameObject gameOverUI;
    public GameObject AimUI;
    public GameObject HudUI;

    public bool isDead;

    private void Start()
    {
        HudPlayerActiveStart();
    }

    void HudPlayerActiveStart()
    {
        playerHetthUI.text = $"Health: {HP}";
        AimUI.gameObject.SetActive(true);
        HudUI.gameObject.SetActive(true);
    }
    public void TakeDamage(int damageAmound)
    {
        HP -= damageAmound;

        if (HP <= 0)
        {
            print("player Dead");
            PlayerDead();
            isDead = true;
        }
        else
        {
            print("Player Hit");
            playerHetthUI.text = $"Health: {HP}";
        }
    }

    private void PlayerDead()
    {
        //отключаем скрипты потому что мы мертвы
        GetComponent<MouseMovement>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;

        //анимаци€ смерти
        GetComponentInChildren<Animator>().enabled = true;
        playerHetthUI.gameObject.SetActive(false);

        GetComponent<ScreenFader>().StartFade();
        AimUI.gameObject.SetActive(false);
        HudUI.SetActive(false);
        StartCoroutine(ShowGameOverUI());
    }

    private IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(1f);
        gameOverUI.gameObject.SetActive(true);

        int waveSurvived = GlobalReferences.Instance.waveNumber;

        if (waveSurvived - 1 > SaveLoadManager.Instance.LoadHighScore())
        {
            SaveLoadManager.Instance.SaveHighScore(waveSurvived - 1);
        }

        //передалал на просто кнопку в правом верхнем углу
        //¬ключаем скрипты так как мы должны в менюшку лазить
        //GetComponent<MouseMovement>().enabled = true;
        //GetComponent<PlayerMovement>().enabled = true;
    }

    private IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(2.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHand"))
        {
            if (isDead == false)
            {
                TakeDamage(other.gameObject.GetComponent<ZombieHand>().damage);
            }
        }
    }
}
