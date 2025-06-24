using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueClick : MonoBehaviour
{ 
    [SerializeField]
    GameObject RewardCanvas;

    /// <summary>
    /// This method should open the map aka the navigation scene to facilitate navigation between encounters
    /// Load scene, set progress
    /// </summary>
    public void ContinueClicked()
    {
        MapController.Instance.IncrementStage();
        //Match3Skin.Instance.StartNewGame();
        if(RewardCanvas != null)
        {
            Destroy(RewardCanvas);
        }
        MapController.Instance.LoadMapScene();
    }
}
