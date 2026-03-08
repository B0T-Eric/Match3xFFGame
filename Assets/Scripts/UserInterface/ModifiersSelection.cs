using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifiersSelection : MonoBehaviour
{
    public Toggle shortToggle, mediumToggle, longToggle, easyToggle, normalToggle, hardToggle;
    public int[] RunSizeOptions;
    [SerializeField]
    List<float> HealthModifiers = new List<float>();
    [SerializeField]
    List<float> DamageModifiers = new List<float>();
    void Start()
    {
        shortToggle.onValueChanged.AddListener(OnShortChange);
        mediumToggle.onValueChanged.AddListener(OnMediumChange);
        longToggle.onValueChanged.AddListener(OnLongChange);
        OnMediumChange(true);
        easyToggle.onValueChanged.AddListener(OnEasyToggle);
        normalToggle.onValueChanged.AddListener(OnNormalToggle);
        hardToggle.onValueChanged.AddListener(OnHardToggle);
        OnNormalToggle(true);
    }
    void OnEasyToggle(bool isOn)
    {
        if (isOn)
        {
            float damage = DamageModifiers[0];
            float health = HealthModifiers[0];
            RunSettings.SetGrowth(damage, health);
            Debug.Log("Difficulty Set to Easy");
        }
    }
    void OnNormalToggle(bool isOn)
    {
        if (isOn)
        {
            float damage = DamageModifiers[1];
            float health = HealthModifiers[1];
            RunSettings.SetGrowth(damage, health);
            Debug.Log("Difficulty Set to Normal");
        }
    }
    void OnHardToggle(bool isOn)
    {
        if (isOn)
        {
            float damage = DamageModifiers[2];
            float health = HealthModifiers[2];
            RunSettings.SetGrowth(damage, health);
            Debug.Log("Difficulty Set to Hard");
        }
    }
    void OnShortChange(bool isOn)
    {
        if (isOn)
        {
            RunSettings.SetMapSize(RunSizeOptions[0]);
            Debug.Log("Short Run Length Selected!");
        }
    }

    void OnMediumChange(bool isOn)
    {
        if (isOn)
        {
            Debug.Log("Medium Run Length Selected!");
            RunSettings.SetMapSize(RunSizeOptions[1]);
        }

    }
    void OnLongChange(bool isOn)
    {
        if (isOn)
        {
            Debug.Log("Long Run Length Selected!");
            RunSettings.SetMapSize(RunSizeOptions[2]);
        }
    }
}
