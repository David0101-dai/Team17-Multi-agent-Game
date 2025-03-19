using System.Collections;
using UnityEngine;

public class EnergyBall_Controller_2 : MonoBehaviour
{
    [SerializeField] private string targetLayerName = "Player";
    [SerializeField] private float xSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool worked;
    [SerializeField] private bool flipped;
    [SerializeField] private int direction;
    [SerializeField] private float existTimer;
    public GameObject Attacker;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        worked = false;
    }

    public void Setup(GameObject Attacker, float speed)
    {
        this.Attacker = Attacker;
        xSpeed = speed;
        // 根据玩家的位置来确定能量球的朝向
        AdjustDirection();
    }

       // 这个方法会根据敌人（Attacker）和玩家的位置来决定能量球的朝向
    private void AdjustDirection()
    {
       if (Attacker != null)
        {
            if (xSpeed > 0)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);  // 旋转180度
            }
            else
            {
                transform.localRotation = Quaternion.identity;  // 不旋转
            }
        }
    }

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

            Damageable damageableComponent = collision.GetComponent<Damageable>();
            Damageable attackerDamageable = Attacker.GetComponent<Damageable>();
            if (damageableComponent != null)
            {
                if (attackerDamageable != null)
                {
                    anim.SetBool("boom", true);
                    damageableComponent.TakeDamage(Attacker, true, false, false,true,false,false);
                    StartCoroutine(DestroyAfterAnimation());
                }
                else
                {
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
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}
