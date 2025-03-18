using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionForKey : MonoBehaviour
{
    public string sceneName; // Имя сцены, на которую нужно перейти
    public KeyCode keyCode; // Код клавиши, которая запускает переход
    public KeyCode exitKeyCode;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene("MainMenu");
        }
        else if (Input.GetKeyDown(exitKeyCode))
        {
            Application.Quit();
        }
    }
}

