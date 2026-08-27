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
        foreach(GameObject go in displays)
        {
            Destroy(go);
        }
        displays.Clear();
        foreach (Enhancement e in EnhancementHandler.FetchAllEnhancements())
        {
            GameObject obj = Instantiate(prefab);
            Image image = obj.GetComponent<Image>();
            image.sprite = e.GetSprite();
            obj.transform.SetParent(transform);
            displays.Add(obj);
        }
    }
}
