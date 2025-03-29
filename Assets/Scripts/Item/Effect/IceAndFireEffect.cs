using UnityEngine;

[CreateAssetMenu(fileName = "IceAndFireEffect", menuName = "Data/ItemEffect/IceAndFire")]
public class IceAndFireEffect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private float cooldownTime = 0.3f; // 冷却时间
    private float lastEffectTime = -Mathf.Infinity; // 上次生成特效的时间

    public override void ExecuteEffect(GameObject from, GameObject to)
    {
        var currentTime = Time.time; // 获取当前时间（以秒为单位）

        // 检查冷却时间，只有当冷却时间已过时才生成新的特效
        if (currentTime - lastEffectTime < cooldownTime) return;

        var pos = from.transform.position + new Vector3(0, 1);
        var rot = from.transform.rotation;
        var parent = FxManager.Instance.fx.transform;

        if (!from.TryGetComponent(out Player player)) return;

        // 判断是否是第三次连击
        var thirdAttack = (player.AttackState as AttackState).comboCounter <= 2;
        if (!thirdAttack) return;

        // 生成特效
        var effect = Instantiate(iceAndFirePrefab, pos, rot, parent);
        if (!effect.TryGetComponent(out Rigidbody2D rb)) return;
        rb.velocity = velocity * player.Flip.facingDir;

        // 更新上次生成特效的时间
        lastEffectTime = currentTime;
    }
}
