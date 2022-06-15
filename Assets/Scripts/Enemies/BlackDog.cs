using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackDog : NormalEnemy
{
    [SerializeField] private float randomMoveRate;
    [SerializeField] private float moveRateNoise;
    [SerializeField] private GameObject thunderBullet;
    [SerializeField] private Transform shotPoint;
    [SerializeField] private GameObject text;
    private float finalRandomMoveRate;
    private float lastMove;
    private float defaultPosY = -7f;
    private Vector3 playerPos;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        finalRandomMoveRate = FinalMoveRateCal();
        // isMoveable = false;
        lastMove = Time.time;
    }

    // Update is called once per frame
    protected override void Update()
    {
        playerPos = WorldManager.Instance.player.transform.position;
        CheckMove();
        AttackAction();
        AttackCountdown();
        WallCheck();
        CheckFlip();
        // if(isMoveable == false)
        // {
        //     return;
        // }
        Move();
    }
    protected override void Move()
    {
        if(isMoveable)
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + Time.deltaTime * moveSpeed * moveDir, -MAX_FLOOR_SPEED, MAX_FLOOR_SPEED), rb.velocity.y);
        else
            rb.velocity = Vector2.zero;
    }
    private bool CheckMove()
    {
        if(lastMove + finalRandomMoveRate <= Time.time && ((enemyState == State.Normal) || (enemyState == State.Walking)))
        {
            lastMove = Time.time;
            finalRandomMoveRate = FinalMoveRateCal();
            isMoveable = !isMoveable;
            if(isMoveable)
            {
                anim.SetTrigger("Move");  
                enemyState = State.Walking;
            }
            else
            {
                anim.SetTrigger("Stop");
                enemyState = State.Normal;
            }
        }
        return isMoveable;
    }
    private float FinalMoveRateCal()
    {
        return finalRandomMoveRate + UnityEngine.Random.Range(-1f * moveRateNoise, moveRateNoise);
    }
    public override void AttackAction()
    {
        RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position, new Vector2(moveDir, 0), attackRange, LayerMask.GetMask("Player"));
        if(hit.collider == null)
        {
            if(attackCountdown > 0)
            {
                return;
            }
            // else
            // {
            //     Flip();
            // }
        }
        if ((attackCountdown <= 0) && ((enemyState == State.Normal) || (enemyState == State.Walking)))
        {
            attackCountdown = attackRate;
            if(hp >= defaultHP/2)
            {
                enemyState = State.Attacking;
                isMoveable = false;
                anim.SetTrigger("Attack");
                // isMoveable = false;
                // anim.SetTrigger("Combo");
                // Invoke("Appear", 5f);
                // Invoke("SpawnThunderBullet", 1f);
            }
            else
            {
                int attackStyle = UnityEngine.Random.Range(0,2);
                if(attackStyle == 0)
                {
                    enemyState = State.Attacking;
                    isMoveable = false;
                    anim.SetTrigger("Attack");
                }
                else
                {
                    enemyState =  State.SpecialMove1;
                    isMoveable = false;
                    anim.SetTrigger("Combo");
                    Invoke("Appear", 5f);
                }
            }
        }
    }
    public void SpawnThunderBullet()
    {
        Debug.Log("Alo Mực bắn bắn khổ vc");
        int bulletf = 4;
        for(int i = 0; i < bulletf; i ++)
        {
            // Debug.Log("Alo Mực bắn bắn");
            GameObject bullet = Instantiate(thunderBullet, shotPoint.position, Quaternion.identity);
            Vector3 dir = new Vector3(0f + i,bulletf - i, 0).normalized;
            Debug.Log("Vector là: " + dir);
            bullet.GetComponent<ThunderBullet>().SetDir(dir);
        }
        for(int i = 0; i < bulletf; i ++)
        {
            // Debug.Log("Alo Mực bắn bắn");
            GameObject bullet = Instantiate(thunderBullet, shotPoint.position, Quaternion.identity);
            Vector3 dir = new Vector3(0f - i,i - bulletf, 0).normalized;
            Debug.Log("Vector là: " + dir);
            bullet.GetComponent<ThunderBullet>().SetDir(dir);
        }
        for(int i = 0; i < bulletf; i ++)
        {
            // Debug.Log("Alo Mực bắn bắn");
            GameObject bullet = Instantiate(thunderBullet, shotPoint.position, Quaternion.identity);
            Vector3 dir = new Vector3(0f + i,i - bulletf, 0).normalized;
            Debug.Log("Vector là: " + dir);
            bullet.GetComponent<ThunderBullet>().SetDir(dir);
        }
        for(int i = 0; i < bulletf; i ++)
        {
            // Debug.Log("Alo Mực bắn bắn");
            GameObject bullet = Instantiate(thunderBullet, shotPoint.position, Quaternion.identity);
            Vector3 dir = new Vector3(0f - i,bulletf - i, 0).normalized;
            Debug.Log("Vector là: " + dir);
            bullet.GetComponent<ThunderBullet>().SetDir(dir);
        }
    }
    void Appear()
    {
        Vector3 playerPos = WorldManager.Instance.player.transform.position;
        transform.position = new Vector3(playerPos.x, defaultPosY, 0f);
        anim.SetTrigger("Tele");
    }
    public void Calmdown()
    {
        enemyState = State.Normal;
        CheckMove();
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player") && enemyState != State.SpecialMove1)
        {
            IDamageable player = col.gameObject.GetComponent<IDamageable>();
            player.TakeDmg(dmg, this.gameObject);
        }
    }
    public override void TakeDmg(int _dmg, GameObject attacker)
    {
        GetHitBehaviour(_dmg);
        CustomEvents.OnScreenShakeDanger?.Invoke(GameConst.SHAKE_ATTACK_AMOUNT, GameConst.SHAKE_ATTACK_TIME);
        EffectPool.Instance.GetHitEffectInPool(transform.position);
        if(isFacingRight && playerPos.x < transform.position.x)
            Flip();
        if(!isFacingRight && playerPos.x > transform.position.x)
            Flip();
        // Debug.Log("[Enemy] take dmg");
    }
    public override void CreateAttackPrefab()
    {
        // enemyState = State.Normal;
        // attackCountdown = attackRate;
        if(attackPrefab)
        {
            Instantiate(attackPrefab, attackSpawnPos.transform.position, attackSpawnPos.transform.rotation);
        }
    }
    protected override void Die()
    {
        // StartCoroutine(LoadYourAsyncScene());
        Despawn();
        ShowText();
    }
    // IEnumerator LoadYourAsyncScene()
    // {
    //     yield return new WaitForSeconds(10f);
    //     AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);
    //     while (!asyncLoad.isDone)
    //     {
    //         yield return null;
    //     }
    // }

    void ShowText()
    {
        text.SetActive(true);
    }
}
