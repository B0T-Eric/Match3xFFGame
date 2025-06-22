using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private static MenuController instance;
    public static MenuController Instance { get { return instance; } }

    public Button play, options, log, exit;
    public GameObject optionsWindow, logWindow;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    void Start()
    {
        play.onClick.AddListener(PlayClick);
        options.onClick.AddListener(OptionsClick);
        log.onClick.AddListener(LogClick);
        exit.onClick.AddListener(ExitClick);
    }

    void PlayClick()
    {
        SceneManager.LoadScene("CharacterSelectScene");
    }
    void OptionsClick()
    {
        options.gameObject.GetComponentInParent<Animator>().SetTrigger("OptionClicked");
        optionsWindow.SetActive(true);
    }
    void LogClick()
    {
        log.gameObject.GetComponentInParent<Animator>().SetTrigger("LogBookClicked");
    }
    void ExitClick()
    {
        Application.Quit();
    }
}
