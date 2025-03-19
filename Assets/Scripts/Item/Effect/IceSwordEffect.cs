using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "IceSwordEffect", menuName = "Data/ItemEffect/IceSwordEffect")]
public class IceSwordEffect : ItemEffect
{
    [SerializeField] private GameObject IceSwordPrefab;
    public override void ExecuteEffect(GameObject from, GameObject to)
    {
        var parent = FxManager.Instance.fx.transform;
        Instantiate(IceSwordPrefab, to.transform.position+new Vector3(0,1,0), Quaternion.identity, parent);
        
    }
}
