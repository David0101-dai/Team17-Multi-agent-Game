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
    public bool canAttack = false;

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
        if(Time.timeScale == 0){
            return;
        }
        Fsm.CurrentState?.Update();

        if (attackCheck == null)
        {
            Debug.LogError("attackCheck 没有正确赋值！");
            return;
        }

        var colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            //Debug.Log("检测到碰撞体: " + hit.gameObject.name);
            // 确保 transform.parent 不为 null，并且正确判断敌人
            if (hit.CompareTag("Player"))
            {
                canAttack = true;
                //Debug.Log("可以攻击");
                break;  // 找到一个符合条件的玩家后直接跳出循环
            }
            else
            {
                canAttack = false;
            }
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    public void MakeTransprent(bool isTransprent)
    {
        Sr.color = isTransprent ? Color.clear : Color.white;
    }

    public bool CanAttack()
    {
        return canAttack;
    }

    public abstract void SlowBy(float slowPercentage, float slowDuration);

    public abstract void Die();
}
