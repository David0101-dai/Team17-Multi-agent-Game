using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(FlipSprite))]
[RequireComponent(typeof(CollisionDetection))]
public abstract class Character : MonoBehaviour
{
    [Header("Attack Colider")]
    public Transform attackCheck;
    public float attackCheckRadius;

    [Header("Knockback Value")]
    public float knockbackXSpeed = 3f;
    public float knockbackYSpeed = 3f;
    public GameObject damageFrom;

    public Animator Anim { get; private set; }
    public Rigidbody2D Rb { get; private set; }

    public FlipSprite Flip { get; private set; }
    public CollisionDetection ColDetect { get; private set; }
    public SpriteRenderer Sr { get; private set; }

    public FSM Fsm { get; private set; }

    protected virtual void Start()
    {
        Anim = GetComponentInChildren<Animator>(); //获取动画器
        Rb = GetComponent<Rigidbody2D>();  //获取刚体
        Flip = GetComponent<FlipSprite>();  //获取反转脚本
        ColDetect = GetComponent<CollisionDetection>();  //获取碰撞脚本
        Sr = GetComponentInChildren<SpriteRenderer>();  //获取渲染者
        Fsm = new FSM();
    }

    protected virtual void Update()
    {
        Fsm.CurrentState?.Update();
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    public void MakeTransprent(bool isTransprent)
    {
        Sr.color = isTransprent ? Color.clear : Color.white;
    }

    public abstract void SlowBy(float slowPercentage, float slowDuration);

    public abstract void Die();
}
