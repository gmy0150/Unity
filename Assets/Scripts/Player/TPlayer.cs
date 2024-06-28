using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPlayer : MonoBehaviour
{
    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpforce;
    public float atkspeed;

    [Header("count")]
    
    public int attackACount;
    public float ComboTimer;
    public float SkillADCool;
    public float SkillSDCool;
    public float SkillASCool;
    public float SkillASDCool;
    public float lastskillad;
    public float lastskillsd;
    public float lastskillas;
    public float lastskillasd;
    float targetTime = -Mathf.Infinity;
    public List<Enemy> enemies = new List<Enemy>();


    [Header("Collision info")]
    [SerializeField]private Transform groundCheck;
    [SerializeField]private float groundCheckDistance;
    [SerializeField]private Transform WallCheck;
    [SerializeField]private float WallCheckDistance;
    [SerializeField]private LayerMask whatIsGround;
    [SerializeField]private LayerMask whatIsWall;
    [SerializeField]private GameObject ArrowObj;

    public float AttackRange;
    public int facingDir {get;private set;} = -1;
    private bool facingRight = true;
    public bool isBusy;
    #region Components
    public Animator anim{get; private set;}
    public Rigidbody2D rigid{get;private set;}
    #endregion

    #region States

    public PlayerStateMachine stateMachine {get;private set;}
    public PlayerIdleState idleState{get;private set;}
    public PlayerMoveState moveState{get;private set;}
    public PlayerJumpState jumpState{get;private set;}
    public PlayerAirState airState{get;private set;}
    // public PlayerAttackState attackState{get; private set;}
    public PlayerAttackA attackAState{get; private set;}
    public PlayerAttackS attackSState{get; private set;}
    public PlayerAttackD attackDState{get; private set;}
    public PlayerAD skillAD{get; private set;}
    public PlayerAS skillAS{get; private set;}
    public PlayerSD skillSD{get; private set;}
    public PlayerASD skillASD{get; private set;}

    [SerializeField]private GameObject playerPrefab;




    #endregion
    public float timer;
    private void Awake() {
        stateMachine = new PlayerStateMachine();
        
        idleState = new PlayerIdleState(this, stateMachine,"Idle");
        moveState = new PlayerMoveState(this, stateMachine,"Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        attackAState = new PlayerAttackA(this, stateMachine, "AttackA");
        attackSState = new PlayerAttackS(this, stateMachine, "AttackS");
        attackDState = new PlayerAttackD(this, stateMachine, "AttackD");
        skillAD = new PlayerAD(this,stateMachine);
        skillAS = new PlayerAS(this,stateMachine);
        skillSD = new PlayerSD(this,stateMachine);
        skillASD = new PlayerASD(this,stateMachine);

        Enemy[] allEnemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in allEnemies) {
            enemies.Add(enemy);
        }

    }

    private void Start() {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        stateMachine.Initalize(idleState);
    }
    private void Update() {
        stateMachine.currentState.Update();
        FlipController();
    }
    public IEnumerator BusyFor(){
        isBusy = true;
        yield return new WaitForSeconds(0.2f);
        isBusy = false;
    }
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinsihTrigger();//함수 호출후 animationFinishTrigger를 작동하게 함

    public void MoveTrigger() => stateMachine.currentState.AnimationMoveTrigger();

    public void SetVelocity(float _xVelocity,float _yVelocity){
        rigid.velocity = new Vector2(_xVelocity,_yVelocity);
    }
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down,groundCheckDistance,whatIsGround);
    private void OnDrawGizmos() {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(WallCheck.position, new Vector3(WallCheck.position.x + WallCheckDistance, WallCheck.position.y));
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, transform.position + transform.right * AttackRange);

    }
    public void Flip(){
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        
        transform.Rotate(0,180,0);
    }

    public void FlipController(){
        if(rigid.velocity.x > 2 && !facingRight){//양수로 가고 facingRight가 false일 때 기본이 false니까 오른쪽을 보고있을 때, 오른쪽 이동일 때
            Flip();
        }else if(rigid.velocity.x < -2 && facingRight){//음수로 가고 facingRight가 true일 때 왼쪽이동 transform.flip을 쓰면 문제가 되는 부분이 많으니까 이렇게 처리해서 하는 부분 이건 공부를 해야겠다.
            Flip();
        }
    }
    public void detailAD(){
        for(int i = 0; i< 8;i++){
            Vector3 bulletPosition = transform.position + transform.right;
            GameObject bullet = Instantiate(ArrowObj,bulletPosition,Quaternion.identity);
            Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
            float randomAngle = Random.Range(-45f,10f);
            Quaternion randomRotation = Quaternion.Euler(0f,0f,randomAngle);
            bullet.transform.rotation = randomRotation;
            Vector2 forceDirection = randomRotation * transform.right;
            bulletRigidbody.AddForce(forceDirection * 20, ForceMode2D.Impulse);
            bullet.transform.localScale = new Vector3(-1 * facingDir, 1, 1);
            
        }
    }
    IEnumerator SkillCheck(Enemy enemy){
        while(!enemy.isEnter){
            
            yield return null;
        }

    }
    public void SDSkill(){
        StartCoroutine(detailSD());
    }
    private IEnumerator detailSD(){
        yield return new WaitForSeconds(0.1f);
        rigid.AddForce(Vector2.left * 10f * facingDir, ForceMode2D.Impulse);

        
        // maxSpeed *= 2;

        targetTime = Time.time;
        while(!checkWallAhead()){
            
            if(skillsdTime()){
                break;
            }
            
            RaycastHit2D hit =Physics2D.Raycast(transform.position, 
            transform.right, AttackRange,LayerMask.GetMask("Enemy"));
            if(hit.collider != null){
                Enemy hits = hit.collider.GetComponent<Enemy>();
                if(hits!=null){
                    // Collider2D collider = hits.GetComponent<Collider2D>();
                    hits.isEnter = true;
                    // collider.isTrigger = true;1
                    hits.transform.position = transform.position;
                }
            }
            foreach (Enemy enemy in enemies){
                if (enemy != null && enemy.isEnter){
                    Debug.Log("반응 체크");
                    Collider2D collider = enemy.GetComponent<Collider2D>();
                    Transform enemyTransform = enemy.transform;
                    enemyTransform.SetParent(transform);
                    collider.isTrigger = true;
                    enemy.rigid.isKinematic = true;
                }
            }
            
            yield return new WaitUntil(() => checkWallAhead()||skillsdTime()||checkbossAhead());

        }
        foreach (Enemy enemy in enemies){
            if (enemy != null && enemy.isEnter){
                Transform enemyTransform = enemy.transform;
                enemyTransform.SetParent(null);
                Collider2D collider = enemy.GetComponent<Collider2D>();
                collider.isTrigger = false;
                enemy.rigid.isKinematic = false;
                enemy.isEnter = false;
            }
        }
    
    }
    bool checkbossAhead() {
        float direction = Mathf.Sign(transform.localScale.x); // 플레이어의 방향을 구함
        Vector2 raycastStart = rigid.position + Vector2.right * 0.5f * facingDir;

        RaycastHit2D rayHit = Physics2D.Raycast(raycastStart, Vector2.right * facingDir, 1, LayerMask.GetMask("Wall")|LayerMask.GetMask("Boss"));

        return (rayHit.collider != null);

    }
    bool CheckEnter(){
        foreach (Enemy enemy in enemies){
            if (enemy != null && enemy.isEnter){
                return true;
            }
        }
        return false;
    }
    bool skillsdTime() {
        if (Time.time - targetTime >= 1.5f) { 
            return true;
        } else {
            return false;
        }
    }    
    public bool CoolTime(string cool){
        if(cool == "SkillAD"){
            if(Time.time >lastskillad + SkillADCool){
                Debug.Log("확인");
                return true;
            }else{
                return false;
            }
        }else if(cool == "SkillSD"){
            if(Time.time >lastskillsd + SkillSDCool){
                return true;
            }else{
                return false;
            }
        }else if(cool == "SkillAS"){
            if(Time.time >lastskillas + SkillADCool){
                return true;
            }else{
                return false;
            }
        }else if(cool == "SkillASD"){
            if(Time.time >lastskillasd + SkillASDCool){
                return true;
            }else{
                return false;
            }
        }else{
            return false;
        }
    }
    bool checkWallAhead() {
        Vector2 raycastStart = rigid.position + Vector2.left * 1f * facingDir; 

        RaycastHit2D rayHit = Physics2D.Raycast(raycastStart, Vector2.left * facingDir, 1, LayerMask.GetMask("Wall")|LayerMask.GetMask("Boss"));

        return (rayHit.collider != null);

    }
}
