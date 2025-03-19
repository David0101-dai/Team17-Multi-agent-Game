using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "IceNecklaceEffect", menuName = "Data/ItemEffect/IceNecklaceEffect")]
public class IceNecklaceEffect : ItemEffect
{
    [SerializeField] private GameObject IceNecklacePrefab;
    
    public override void ExecuteEffect(GameObject from, GameObject to)
    {
        var newIceNecklace = Instantiate(IceNecklacePrefab);
        newIceNecklace.transform.SetParent(to.transform, false); // false 保证相对位置不变
        newIceNecklace.transform.localPosition = new Vector3(0, 1, 0);
    }
}
