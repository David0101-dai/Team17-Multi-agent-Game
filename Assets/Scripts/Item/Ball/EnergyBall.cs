using System.Collections;
using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    [SerializeField] protected string targetLayerName = "Player";
    [SerializeField] protected float xSpeed;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected bool worked;
    [SerializeField] protected bool flipped;
    [SerializeField] protected int direction;
    [SerializeField] protected float existTimer;
    public GameObject Attacker;
    public Animator anim;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        worked = false;
    }

    protected virtual void Setup(GameObject attacker, float speed)
    {
        this.Attacker = attacker;
        xSpeed = speed;
        AdjustDirection();
    }

    protected void AdjustDirection()
    {
        if (Attacker != null)
        {
            if (xSpeed > 0)
            {
                transform.localRotation = Quaternion.identity; 
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    public void Update()
    {
        if (!worked){
            rb.velocity = new Vector2(xSpeed, rb.velocity.y);
        }else{
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
            
    }

    // 将 OnTriggerEnter2D 设置为 virtual，允许派生类覆写
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // 基类中的默认行为（可以不写或做通用处理）
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

    public IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}
