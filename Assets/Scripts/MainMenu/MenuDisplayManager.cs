using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDisplayManager : MonoBehaviour
{
    public Animator animator;
    public void DisplayOptions()
    {
        MenuController.Instance.optionsWindow.SetActive(true);
    }
    public void DisplayLogbook()
    {
        MenuController.Instance.logWindow.SetActive(true);
    }
}
