using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    private void OnValidate()
    {
        if (!itemData) return;
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = itemData.name;
    }

    //aaass
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    

public void Setup(ItemData item, Vector2 velocity)
{
    if (item == null)
    {
        Debug.LogError("Item is null. Cannot set up item.");
        return;
    }
    itemData = item;
    GetComponent<Rigidbody2D>().velocity = velocity;
    GetComponent<SpriteRenderer>().sprite = item.icon;
    gameObject.name = item.name;
}


public void PickupItem()
{
    if (itemData == null)
    {
        Debug.Log("Item data is null. Cannot add item.");
        return;
    }

    if (!Inventory.Instance.CanAddItem(itemData))
    {
        rb.velocity = new Vector2(0, 7);
        return;
    }

    Inventory.Instance.AddItem(itemData);
    Destroy(gameObject);
}

}