using UnityEngine;

public class Game : MonoBehaviour
{
    Vector3 dragStart;
    bool isDragging;
    [SerializeField]
    Match3Skin match3;
    private void Awake() => match3.StartNewGame();

    private void Update()
    {
        if (match3.IsPlaying)
        {
            if(!match3.IsBusy)
            {
                HandleInput();
            }
            match3.DoWork();
        }
    }

    private void HandleInput()
    {
        if(!isDragging && Input.GetMouseButtonDown(0))
        {
            dragStart = Input.mousePosition;
            isDragging = true;
        }
        else if(isDragging && Input.GetMouseButton(0))
        {
            isDragging = match3.EvaluateDrag(dragStart, Input.mousePosition);
        }
        else
        {
            isDragging = false;
        }
    }
}
