using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyActionIndicator : MonoBehaviour
{
    public List<RectTransform> pointerPositions = new();
    public RectTransform Pointer;
    int currentPosition = 0;
    public void SetPointer(RectTransform pointer)
    {
        Pointer = pointer;
        Pointer.SetParent(pointerPositions[0]);
        Pointer.anchoredPosition = Vector2.zero;
    }
    public void ShiftPointer()
    {
        currentPosition++;
        if(currentPosition > pointerPositions.Count -1)
        {
            currentPosition = 0;
        }
        
        Pointer.SetParent(pointerPositions[currentPosition]);
        Pointer.anchoredPosition = Vector2.zero;
    }
}
