using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusDisplayManager : MonoBehaviour
{
    [SerializeField]
    GameObject statusDisplay, statusDisplayUI;
    private Dictionary<BaseStatus, GameObject> statusDisplays = new();
    public void DisplayStatus(BaseStatus status)
    {
        if (statusDisplays.ContainsKey(status))
            return;
        GameObject display = Instantiate(statusDisplayUI, transform);
        StatusDisplay s = display.GetComponent<StatusDisplay>();
        s.Init(status);
        statusDisplays[status] = display;
    }
    public void RemoveDisplayedStatus(BaseStatus status)
    {
        if (statusDisplays.TryGetValue(status, out var display))
        {
            Destroy(display);
            statusDisplays.Remove(status);
        }
    }
    public void RemoveAllDisplays()
    {
        foreach (var kvp in statusDisplays)
        {
            Destroy(kvp.Value);
        }
        statusDisplays.Clear();
    }
}
