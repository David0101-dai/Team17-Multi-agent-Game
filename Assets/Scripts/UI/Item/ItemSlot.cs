using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public InventoryItem item;

    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    private UI ui;

    private void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (item == null) return;
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.Instance.RemoveItem(item.data);
            return;
        }
        if (item.data.itemType != ItemType.Equipment) return;
        Inventory.Instance.EquipItem(item.data);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null) return;

        Vector2 mousePosition = Input.mousePosition;

        float xOffset = 0;
        float yOffset = 0;

        if (mousePosition.x > 600)
            xOffset = -100;
        else
            xOffset = 100;

        if (mousePosition.y > 320)
            yOffset = -100;
        else
            yOffset = 100;

        var equipment = item.data as ItemDataEquipment;
        if (equipment == null) return;
        ui.tooltip.ShowTooltip(equipment);
        ui.tooltip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null) return;
        ui.tooltip.HideTooltip();
    }



    public virtual void UpdateSlot(InventoryItem newItem)
    {
        item = newItem;
        if (item != null)
        {
            itemImage.color = Color.white;
            itemImage.sprite = item.data.icon;
            var text = item.stackSize > 1 ? item.stackSize.ToString() : string.Empty;
            itemText.text = text;
        }
        else
        {
            itemImage.color = Color.clear;
            itemImage.sprite = null;
            itemText.text = string.Empty;
        }
    }
}