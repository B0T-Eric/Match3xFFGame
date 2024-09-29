using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueClick : MonoBehaviour
{ 
    [SerializeField]
    GameObject RewardCanvas;

    public void ContinueClicked()
    {
        Match3Skin.Instance.StartNewGame();
        Destroy(RewardCanvas );
    }
}
