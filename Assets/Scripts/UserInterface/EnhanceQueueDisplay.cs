using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class EnhanceQueueDisplay : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;
    List<GameObject> displays = new List<GameObject>();
    public void UpdateDisplay()
    {
        displays.Clear();
        foreach(Enhancement e in EnhancementHandler.FetchAllEnhancements())
        {
            GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);
            Image image = obj.GetComponent<Image>();
            image.sprite = e.GetSprite();
            displays.Add(obj);
        }
    }
}
