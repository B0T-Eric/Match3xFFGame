using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuActions : MonoBehaviour
{
    [SerializeField]
    PauseMenuScript PauseMenuScript;
    public void ResumeGame()
    {
        PauseMenuScript.Resume();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
