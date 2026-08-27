using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ProfileManager : MonoBehaviour
{
    public static int Identifier = 0;

    [SerializeField]
    TextMeshProUGUI ProfileText;
    [SerializeField]
    TMP_InputField editName, addName;
    [SerializeField]
    Button changeButton, editButton, addButton, saveButton, cancelButton;
    [SerializeField]
    GameObject MenuObject, AddMenuObject;
    [SerializeField]
    GameObject optionObj;
    [SerializeField]
    Transform SRContent;

    private List<ProfileOption> options = new List<ProfileOption>();
    public List<PlayerProfile> Profiles = new List<PlayerProfile>();
    public PlayerProfile CurrentProfile;
    public string FilePath;

    private void Start()
    {
        cancelButton.onClick.AddListener(CancelClick);
        saveButton.onClick.AddListener(SaveClick);
        addButton.onClick.AddListener(ShowAddMenu);
        editButton.onClick.AddListener(EditProfileName);
        changeButton.onClick.AddListener(ShowProfileMenu);
        FilePath = Path.Combine(Application.persistentDataPath, "profiles.json");
        LoadProfiles();
        if (Profiles.Count > 0)
        {
            ShowDisplays();
        }
        else
        {
            CreateProfile($"Default Profile");
            ShowDisplays();
        }

        ShowAddMenu();
        CloseProfileMenu();
        
    }
    private void ShowAddMenu()
    {
        if (AddMenuObject.activeSelf)
        {
            AddMenuObject.SetActive(false);
        }
        else
        {
            AddMenuObject.SetActive(true);
        }
    }
    private void CancelClick()
    {
        addName.text = "";
        ShowAddMenu();
    }
    private void SaveClick()
    {
        string s = addName.text;
        CreateProfile(s);
        addName.text = "";
        ShowAddMenu();
    }
    private void ShowDisplays()
    {
        foreach (Transform child in SRContent)
        {
            Destroy(child.gameObject);
        }

        options.Clear();
        foreach (PlayerProfile profile in Profiles)
        {
            GameObject gO = Instantiate(optionObj);
            ProfileOption p = gO.GetComponent<ProfileOption>();
            p.SetData(profile, this);
            options.Add(p);
            gO.transform.SetParent(SRContent);
            RectTransform content = SRContent.GetComponent<RectTransform>();
            content.sizeDelta = new Vector2(content.sizeDelta.x,content.sizeDelta.y + 15f);
        }
        if(CurrentProfile == null)
        {
            CurrentProfile = Profiles[0];
        }
        ProfileText.text = $"Profile: {CurrentProfile.Name}";
    }

    public void SetCurrentProfile(int i)
    {
        foreach (PlayerProfile p in Profiles)
        {
            if (p.ID == i)
            {
                CurrentProfile = Profiles[Profiles.IndexOf(p)];
            }
        }
        //visual feed back?
        ProfileText.text = $"Profile: {CurrentProfile.Name}";
    }
    public void ShowProfileMenu()
    {
        if (MenuObject.activeSelf)
        {
            CloseProfileMenu();

        }
        else
        {
            MenuObject.SetActive(true);
        }
    }
    public void CloseProfileMenu()
    {
        MenuObject.SetActive(false);
    }
    public void CreateProfile(string name)
    {
        PlayerProfile profile = new PlayerProfile();

        profile.Name = name;
        profile.ID = GetNewID();

        Profiles.Add(profile);

        SaveProfiles();
    }
    public void SaveProfiles()
    {
        SaveFile save = new SaveFile();

        foreach (PlayerProfile profile in Profiles)
        {
            save.Profiles.Add(profile.GetSaveData());
        }
        string json = JsonUtility.ToJson(save, true);
        File.WriteAllText(FilePath, json);
        ShowDisplays();
    }
    public void LoadProfiles()
    {
        if (!File.Exists(FilePath))
            return;

        string json = File.ReadAllText(FilePath);
        if (json.Equals(""))
        {
            return;
        }
        SaveFile save = JsonUtility.FromJson<SaveFile>(json);

        Profiles.Clear();

        foreach (var profileSave in save.Profiles)
        {
            Profiles.Add(PlayerProfile.FromSave(profileSave));
            Identifier = Mathf.Max(Identifier, profileSave.ID + 1);
        }
    }
    public void EditProfileName()
    {
        CurrentProfile.Name = editName.text;
        foreach(ProfileOption option in options)
        {
            if(option.Profile.ID == CurrentProfile.ID)
            {
                option.UpdateNameText();
            }
        }
        SaveProfiles();
    } 
    
    public static int GetNewID()
    {
        int id = Identifier;
        Identifier++;
        return id;
    }
}
public class PlayerProfile
{
    public string Name;
    public int ID;

    public HashSet<int> UnlockedEvents = new();
    public HashSet<int> UnlockedItems = new();

    public PlayerProfileSaveData GetSaveData()
    {
        return new PlayerProfileSaveData
        {
            name = Name,
            ID = ID,
            unlockedEventsIndex = new List<int>(UnlockedEvents),
            unlockedItemsIndex = new List<int>(UnlockedItems)
        };
    }

    public static PlayerProfile FromSave(PlayerProfileSaveData save)
    {
        PlayerProfile profile = new PlayerProfile();

        profile.Name = save.name;
        profile.ID = save.ID;

        profile.UnlockedEvents = new HashSet<int>(save.unlockedEventsIndex);
        profile.UnlockedItems = new HashSet<int>(save.unlockedItemsIndex);

        return profile;
    }
}
[Serializable]
public class SaveFile
{
    public List<PlayerProfileSaveData> Profiles = new();
}
[Serializable]
public class PlayerProfileSaveData
{
    public string name;
    public int ID;
    public List<int> unlockedEventsIndex;
    public List<int> unlockedItemsIndex;
    public PlayerProfileSaveData(string name, int iD, List<int> unlockedEventsIndex, List<int> unlockedItemsIndex)
    {
        this.name = name;
        ID = iD;
        this.unlockedEventsIndex = unlockedEventsIndex;
        this.unlockedItemsIndex = unlockedItemsIndex;
    }
    public PlayerProfileSaveData() 
    {
        unlockedEventsIndex = new List<int>();
        unlockedItemsIndex = new List<int>();
        ID = ProfileManager.GetNewID();
    }
}
