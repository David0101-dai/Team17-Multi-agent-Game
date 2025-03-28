using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemIntroduceText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;

    public void ShowTooltip(ItemDataEquipment item)
    {
        if (item == null) return;
        itemNameText.text = item.itemName;
        itemTypeText.text = item.equipmentType.ToString();
        itemIntroduceText.text = item.itemIntroduce;
        itemDescriptionText.text = item.GetDescription();

        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}