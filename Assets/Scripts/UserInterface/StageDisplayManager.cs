using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageDisplayManager : MonoBehaviour
{
    public TextMeshProUGUI StageText;
    public void SetStageText(int stageNum)
    {
        StageText.text = $"Stage: {stageNum}"; 
    }
}
