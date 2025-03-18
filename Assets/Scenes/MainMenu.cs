using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //условно сделать меню настроек все делается через этот класс!!!
    public TMP_Text highScoreUI;

    string newGameScene = "SampleScene";

    // Start is called before the first frame update
    void Start()
    {
        //текст с максимальными балами
        int highScore = SaveLoadManager.Instance.LoadHighScore();
        highScoreUI.text = $"Количество прожитых волн: {highScore}";

    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene(newGameScene);
        }
        else if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }
    public void ExitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
     Application.Quit();
#endif
    }
}
