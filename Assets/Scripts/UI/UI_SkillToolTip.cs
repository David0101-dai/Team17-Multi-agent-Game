using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_SkillToolTip : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI skillText;
    [SerializeField] public TextMeshProUGUI skillName;

    public void ShowToolTip(string _skillDescription, string _skillName)
    {
        skillName.text = _skillName;
        skillText.text = _skillDescription;
        gameObject.SetActive(true);
    }

    public void HideToolTip() => gameObject.SetActive(false);
}
