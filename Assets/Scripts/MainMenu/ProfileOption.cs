using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileOption : MonoBehaviour
{
    public PlayerProfile Profile;
    public ProfileManager Manager;
    TextMeshProUGUI text;
    Button button;
    
    private void OnEnable()
    {
        
    }
    public void SetData(PlayerProfile profile, ProfileManager manager)
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        button.onClick.AddListener(() => Manager.SetCurrentProfile(Profile.ID));
        Profile = profile;
        Manager = manager;
        UpdateNameText();
    }
    public void UpdateNameText()
    {
        text.text = Profile.Name;
    }
}
