using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public void GoToMainMenu()
    {
        // �������� ����� MainMenu
        SceneManager.LoadScene("MainMenu");
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene("MainMenu");
        }
        else if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}

