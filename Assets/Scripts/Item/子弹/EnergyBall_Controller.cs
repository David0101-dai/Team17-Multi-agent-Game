using System.Collections;
using System.Collections.Generic;
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
    rb = GetComponent<Rigidbody2D>();  // 自动获取 Rigidbody2D 组件
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
        rb.velocity = new Vector2(xSpeed,rb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName) && !worked){
            Damageable damageableComponent = collision.GetComponent<Damageable>();
            if (damageableComponent != null)
            {
                anim.SetBool("boom", true);  // 激活动画
                damageableComponent.TakeDamage(Attacker,true,false,false,false,true,false);
            }
        }
    }
    public void FlipArrow(){
        if(flipped){
            return;
        }
        xSpeed = xSpeed*-1;
        flipped = true;
        transform.Rotate(0,180,0);
        targetLayerName = "Enemy";
    }
}
