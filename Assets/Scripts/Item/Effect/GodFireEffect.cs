using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GodFireEffect", menuName = "Data/ItemEffect/GodFireEffect")]
public class GodFireEffect : ItemEffect
{
    [SerializeField] private GameObject GodFirePrefab;
    public override void ExecuteEffect(GameObject from, GameObject to)
    {
        var parent = FxManager.Instance.fx.transform;
        Instantiate(GodFirePrefab, to.transform.position + new Vector3(0, 2.3f, 0), Quaternion.identity, parent);

    }
}
