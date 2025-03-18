using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionForKey : MonoBehaviour
{
    public string sceneName; // ��� �����, �� ������� ����� �������
    public KeyCode keyCode; // ��� �������, ������� ��������� �������
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

