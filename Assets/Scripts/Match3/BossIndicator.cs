using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossIndicator : MonoBehaviour
{
    public List<RectTransform> pointerPositions;
    public RectTransform Pointer;
    public List<GameObject> Icons;
    int currentPosition = 0;
    float speed = 5f;
    private Vector2 targetPosition;
    private void Start()
    {
        targetPosition = pointerPositions[currentPosition].anchoredPosition;
    }
    public void SetIconImages(List<Sprite> images)
    {
        for (int i = 0; i < Icons.Count; i++)
        {
            Image s = Icons[i].GetComponent<Image>();
            if (i < images.Count && images[i] != null)
            {
                s.sprite = images[i];
                Icons[i].SetActive(true);
            }
            else
            {
                s.sprite = null;
                Icons[i].SetActive(false);
            }
        }
    }
    public void ShiftPointer()
    {
        currentPosition++;
        if(currentPosition > pointerPositions.Count -1)
        {
            currentPosition = 0;
        }
        targetPosition = pointerPositions[currentPosition].anchoredPosition;
    }
    private void Update()
    {
        Pointer.anchoredPosition = Vector2.Lerp(Pointer.anchoredPosition, targetPosition, speed * Time.deltaTime);

    }
}
