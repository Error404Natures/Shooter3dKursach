using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToSampleScene : MonoBehaviour
{
    
    
    public KeyCode exitKeyCode;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene("SampleScene");
        }
        else if (Input.GetKeyDown(exitKeyCode))
        {
            Application.Quit();
        }
    }
}
