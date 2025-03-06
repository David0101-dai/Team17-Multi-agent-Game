using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    private ItemObject itemObject;

    private void Awake()
    {
        itemObject = GetComponentInParent<ItemObject>();
    }

private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        if (itemObject != null)
        {
            itemObject.PickupItem();
            AudioManager.instance.PlaySFX(13, null); // 播放拾取音效
        }
        else
        {
            Debug.LogError("ItemObject is null. Cannot pick up item.");
        }
    }
}

}