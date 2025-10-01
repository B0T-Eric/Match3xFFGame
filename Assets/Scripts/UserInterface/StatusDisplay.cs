using NUnit.Compatibility;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatusDisplay : MonoBehaviour
{
    public BaseStatus status;
    
    public void Init(BaseStatus status)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "GameScene")
        {
            Game game = GameObject.Find("Game").GetComponent<Game>();
            int index = 0;
            if (status.GetType() == typeof(Burn))
            {
                index = 0;
            }
            else if (status.GetType() == typeof(Weakness))
            {
                index = 1;
            }
            else if (status.GetType() == typeof(Rage))
            {
                index = 2;
            }
            else if (status.GetType() == typeof(Passion))
            {
                index = 3;
            }
            else if (status.GetType() == typeof(Brittle))
            {
                index = 4;
            }
            
            spriteRenderer.sprite = game.statusSprites[index];
        }
        else
        {
            spriteRenderer.sprite = status.Icon;
        }
        
    }
    private void OnMouseOver()
    {
        HoverManager.Instance.DisplayHover(status.ToString());
    }
    private void OnMouseExit()
    {
        HoverManager.Instance.RemoveHover();
    }
}

