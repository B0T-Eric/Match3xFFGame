using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusDisplayManager : MonoBehaviour
{
    [SerializeField]
    GameObject statusDisplay;
    Transform statusContainer;
    public void DisplayStatus(BaseStatus status)
    {
        statusContainer = this.transform;
        GameObject display = Instantiate(statusDisplay,statusContainer);
        StatusDisplay s = display.GetComponent<StatusDisplay>();
        s.Init(status);
    }
    public void RemoveDisplayedStatus()
    {
        if (statusDisplay != null && statusContainer != null)
        {
            foreach (Transform child in statusContainer)
            {
                Destroy(child);
            }
        }
    }
}
