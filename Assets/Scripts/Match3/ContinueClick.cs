using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        
        foreach (BaseCharacter character in Party.Instance.members)
        {
            //gold digger item logic.
            foreach (DraggableItem item in character.items)
            {
                if (item is GoldDigger gd)
                {
                    gd.GainGold();
                    gd.TryUpgradeGoldDigger();
                }
            }
            //gotta reset potential since it breaks healing rooms, I found out (29/10/25)
            character.potential = 0;
        }
        
        if (MapController.Instance != null)
        {
            //MapController.Instance.MarkRoomCleared(MapController.Instance.SelectedRow, MapController.Instance.SelectedColumn);
        }
        MapController.Instance.IncrementStage();
        if (RewardCanvas != null)
        {
            Destroy(RewardCanvas);
        }
        if (SceneManager.GetActiveScene().name.Equals("BossScene"))
        {
            MapController.Instance.DefeatBoss();
        }
        SceneManager.LoadScene("NavigationScene");
    }
}
