using TMPro;
//using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CraftSlot : ItemSlot
{

    private void OnEnable()
    {
        UpdateSlot(item);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        var craftData = item.data as ItemDataEquipment;
        Inventory.Instance.CanCraft(craftData, craftData.craftingmaterials);
    }
    public override void UpdateSlot(InventoryItem newItem)
    {
        itemImage.color = Color.clear;
    }
}