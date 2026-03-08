using NUnit.Compatibility;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StatusDisplay : MonoBehaviour
{
    public BaseStatus status;
    public TextMeshProUGUI durationText, stackText;
    public void Init(BaseStatus status)
    {
        this.status = status;
        Image renderer = GetComponent<Image>();
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
            else if (status.GetType() == typeof(Poison))
            {
                index = 5;
            }
            else if (status.GetType() == typeof(Wound))
            {
                index = 6;
            }
            ToggleText(status);
            this.status.Changed += Status_Changed;
            renderer.sprite = game.statusSprites[index];
        }
        else
        {
            renderer.sprite = status.Icon;
        }
        
    }

    private void Status_Changed()
    {
        UpdateText();
    }

    private void ToggleText(BaseStatus status)
    {
        if (status is IDurationStatus durationStatus)
        {
            durationText.text = durationStatus.Duration.ToString();
        }
        else
        {
            durationText.gameObject.SetActive(false);
        }
        if (status is IStackableStatus stackableStatus)
        {
            stackText.text = stackableStatus.StackCount.ToString();
        }
        else
        {
            stackText.gameObject.SetActive(false);
        }
    }
    private void UpdateText()
    {
        if (durationText != null)
        {
            if(status is IDurationStatus durationStatus)
            durationText.text = durationStatus.Duration.ToString();
        }
        if (stackText != null)
        {
            if (status is IStackableStatus stackStatus)
                stackText.text = stackStatus.StackCount.ToString();
        }
    }
    //private void OnMouseOver()
    //{
    //    HoverManager.Instance.DisplayHover(status.ToString());
    //}
    //private void OnMouseExit()
    //{
    //    HoverManager.Instance.RemoveHover();
    //}
}

