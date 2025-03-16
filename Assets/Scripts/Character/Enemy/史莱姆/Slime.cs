using System.Collections;
using UnityEngine;

public enum SlimeType {big,middle,small}
public class Slime : Enemy
{
    private bool isDead;

    [Header("Slime spesific")]
    [SerializeField] private SlimeType SlimeType;
    [SerializeField] private int amountOfSlimeToCreat;
    [SerializeField] private GameObject SlimePrefab;
    [SerializeField] private Vector2 minCreationVelocity;
    [SerializeField] private Vector2 maxCreationVelocity;

    #region State
    public IState IdleState { get; private set; }
    public IState PatrolState { get; private set; }
    public IState ChaseState { get; private set; }
    public IState AttackState { get; private set; }
    public IState HitState { get; private set; }
    public IState DeadState { get; private set; }
    public IState StunState { get; private set; }
    #endregion

    protected override void Start()
    {
        base.Start();

        IdleState = new SlimeIdleState(Fsm, this, "Idle");
        PatrolState = new SlimePatrolState(Fsm, this, "Move");
        ChaseState = new SlimeChaseState(Fsm, this, "Move");
        AttackState = new SlimeAttackState(Fsm, this, "Attack");
        HitState = new SlimeHitState(Fsm, this, "Hit");
        DeadState = new SlimeDeadState(Fsm, this, "Dead");
        StunState = new SlimeStunState(Fsm, this, "Stun");
        Fsm.SwitchState(IdleState);
        isDead = false;
    }

    protected override void Update()
    {
        base.Update();
       // Debug.Log("骷髅的Current State: " + Fsm.CurrentState.ToString());
    }

    protected override void SwitchHitState()
    {
        Fsm.SwitchState(HitState);
    }

    protected override void SwitchStunState()
    {
        Fsm.SwitchState(StunState);
    }

    protected override bool IsInStunState()
    {
        return Fsm.CurrentState == StunState;
    }

    public override void Die()
    {
        if(isDead){
            return;
        }

        isDead = true;

        PlayerManager.Instance.AddScore(2);

        Debug.Log(PlayerManager.finalscore);

        Fsm.SwitchState(DeadState);

        if(SlimeType ==SlimeType.small){
            return;
        }else{
            CreateSlimes(amountOfSlimeToCreat, SlimePrefab);
        }

    }

    private void CreateSlimes(int _amountOfSlime, GameObject _slimePrefab)
    {
        for (int i = 0; i < _amountOfSlime; i++)
        {
            // 在原本的位置上加上随机偏移量
            float xOffset = Random.Range(-1f, 1f);  // 控制水平偏移范围
            float yOffset = Random.Range(0.5f, 2f); // 控制垂直偏移范围

            Vector3 spawnPosition = transform.position + new Vector3(xOffset, yOffset, 0);  // 增加偏移量

            GameObject newSlime = Instantiate(_slimePrefab, spawnPosition, Quaternion.identity);

            newSlime.GetComponent<Slime>().SetUpSlime(Flip.facingDir);
        }
    }


    public void SetUpSlime(int _facingDir){

        // 获取 FlipSprite 组件
    FlipSprite flipSprite = GetComponent<FlipSprite>();

    if (flipSprite == null)
    {
        Debug.LogError("FlipSprite component is missing on the Slime object!");
        return;
    }

        if(_facingDir != flipSprite.facingDir){
            flipSprite.Flip();
        }

        float xVelocity = Random.Range(minCreationVelocity.x,maxCreationVelocity.x);
        float yVelocity = Random.Range(minCreationVelocity.y,maxCreationVelocity.y);
        
        GetComponent<Rigidbody2D>().velocity = new Vector2(-flipSprite.facingDir*xVelocity,yVelocity);
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }

    public override void SlowBy(float slowPercentage, float slowDuration)
    {
        StartCoroutine(Slow(slowPercentage, slowDuration));
        IEnumerator Slow(float slowPercentage, float slowDuration)
        {
            var slow = 1 - slowPercentage;
            Anim.speed = slow;
            moveSpeed *= slow;
            yield return new WaitForSeconds(slowDuration);
            Anim.speed = 1;
            moveSpeed = defaultMoveSpeed;
        }
    }


        
}
