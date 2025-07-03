using UnityEngine;

public class StatusDisplay : MonoBehaviour
{
    public BaseStatus status;
    
    public void Init(BaseStatus status)
    {
        Game game = GameObject.Find("Game").GetComponent<Game>();
        int index = 0;
        if(status.GetType() == typeof(Burn))
        {
            index = 0;
        }
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = game.statusSprites[index];
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

