using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField]
    GameObject canvas;
    bool isPaused;
    private void Start()
    {
        isPaused = false;
        ToggleCanvas();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            ToggleCanvas();
        }
    }
    void ToggleCanvas()
    {
        if (isPaused)
        {
            canvas.SetActive(true);
        }
        else canvas.SetActive(false);
    }
    public void Resume()
    {
        isPaused=false;
        ToggleCanvas();
    }
}
