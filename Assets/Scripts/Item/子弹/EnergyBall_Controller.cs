using System.Collections;
using UnityEngine;

public class EnergyBall_Controller : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private string targetLayerName = "Player";
    [SerializeField] private Damageable damageable;
    [SerializeField] private float xSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool worked;
    [SerializeField] private bool flipped;
    [SerializeField] private float existTimer;
    public GameObject Attacker;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        worked = false;
    }

    public void Setup(Damageable damageable, GameObject Attacker)
    {
        this.Attacker = Attacker;
        this.damageable = damageable;
    }

    // Update is called once per frame
    void Update()
    {
        if(!worked)
        rb.velocity = new Vector2(xSpeed, rb.velocity.y);
    }

private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName) && !worked)
    {
        worked = true;
        rb.velocity = new Vector2(0, 0);

        // 检查 Attacker 和 Attacker 上的 Damageable 组件是否存在
        Damageable damageableComponent = collision.GetComponent<Damageable>();
        Damageable attackerDamageable = Attacker.GetComponent<Damageable>();
        if (damageableComponent != null)
        {
            if (attackerDamageable != null)
            {
                anim.SetBool("boom", true);  // 激活动画
                damageableComponent.TakeDamage(Attacker, true, false, false, false, true, false);
                // 开始自毁流程
                StartCoroutine(DestroyAfterAnimation());
            }
            else
            {
                // 如果攻击者的 Damageable 组件已销毁，则不执行伤害
                StartCoroutine(DestroyAfterAnimation());
                Debug.LogWarning("Attacker's Damageable component is missing.");
            }
        }
    }else if(collision.gameObject.layer == LayerMask.NameToLayer("Platform")){
            Damageable damageableComponent = collision.GetComponent<Damageable>();
            anim.SetBool("boom", true);  // 激活动画
            if(damageableComponent != null){
                damageableComponent.TakeDamage(Attacker, true, false, false, false, true, false);
            }// 开始自毁流程
            StartCoroutine(DestroyAfterAnimation());
    }
}


    public void FlipArrow()
    {
        if (flipped)
        {
            return;
        }
        xSpeed = xSpeed * -1;
        flipped = true;
        transform.Rotate(0, 180, 0);
        targetLayerName = "Enemy";
    }

    // 协程等待动画播放完后销毁物体
    private IEnumerator DestroyAfterAnimation()
    {
        // 等待动画播放完毕，假设动画播放时长为 1 秒
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        // 销毁物体
        Destroy(gameObject);
    }
}
