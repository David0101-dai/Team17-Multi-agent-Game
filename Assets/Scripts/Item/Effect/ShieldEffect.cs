using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "ShieldEffect", menuName = "Data/ItemEffect/ShieldEffect")]
public class ShieldEffect : ItemEffect
{
    [SerializeField] private GameObject ShieldPrefab;
    public override void ExecuteEffect(GameObject from, GameObject to)
    {
        var newShield = Instantiate(ShieldPrefab);
        newShield.transform.SetParent(to.transform, false); // false 保证相对位置不变
        newShield.transform.localPosition = new Vector3(0, 1, 0);  // 微调位置

    }
}
