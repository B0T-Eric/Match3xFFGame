using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoverManager : MonoBehaviour
{
    TextMeshPro hoverText;
    private static HoverManager instance;
    public static HoverManager Instance { get { return instance; } }
    private void Awake()
    {
        instance = this;
        hoverText = GetComponent<TextMeshPro>();
    }
    public void DisplayHover(string text)
    {
        hoverText.text = text;
        Debug.Log("Hovering item");
    }
    public void RemoveHover()
    {
        hoverText.text = null;
        Debug.Log("Stopped Hovering");
    }
}
