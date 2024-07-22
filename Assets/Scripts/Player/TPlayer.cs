using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Windows.Speech;

public class TPlayer : MonoBehaviour
{
    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpforce;
    #region skill
        
    static bool SkillADUsed;
    static bool SkillASDUsed;
    static bool SkillSDUsed;
    static bool SkillASUsed;
    static bool DashUsed;
    #endregion


    [Header("count")]
    bool atkdmg = false;
    #region Cooltime
        
    public int attackACount;
    public float ComboTimer;
    public float SkillADCool;
    public float SkillSDCool;
    public float SkillASCool;
    public float SkillASDCool;
    public float DashCool;
    int count;
    public static float lastskillad;
    public static float lastskillsd;
    public static float lastskillas;
    public static float lastskillasd;
    public static float lastDashTime;
    #endregion

    float targetTime = -Mathf.Infinity;
    float SDtimer = 0;
    private float dashTimeLeft;
    private float lastDash = -100f;
    public float dashTime;
    private float lastImageXpos;
    public float dashSpeed;
    public float distanceBetweenImages;
    public List<Enemy> enemies = new List<Enemy>();
    #region Collision
    [Header("Collision info")]
    [SerializeField]private Transform groundCheck;
    [SerializeField]private float groundCheckDistance;
    [SerializeField]private Transform WallCheck;
    [SerializeField]private float WallCheckDistance;
    [SerializeField]private LayerMask whatIsGround;
    [SerializeField]private LayerMask whatIsWall;
    #endregion
    [SerializeField]private GameObject ArrowObj;

    public float AttackRange;
    public int facingDir {get;private set;} = -1;
    public bool facingRight = true;
    public bool isBusy;
    #region Components
    public Animator anim{get; private set;}
    public Rigidbody2D rigid{get;private set;}
    #endregion
    #region HP
    public int curHP;
    public int maxHP;
    #endregion
    #region States

    public PlayerStateMachine stateMachine {get;private set;}
    public PlayerIdleState idleState{get;private set;}
    public PlayerMoveState moveState{get;private set;}
    public PlayerJumpState jumpState{get;private set;}
    public PlayerAirState airState{get;private set;}
    public PlayerDashState dashState{get;private set;}
    public PlayerAttackA attackAState{get; private set;}
    public PlayerAttackS attackSState{get; private set;}
    public PlayerAttackD attackDState{get; private set;}
    public PlayerAD skillAD{get; private set;}
    public PlayerAS skillAS{get; private set;}
    public PlayerSD skillSD{get; private set;}
    public PlayerASD skillASD{get; private set;}
    [SerializeField]private GameObject SkillObj;

    LineRenderer lineRenderer;
    float rayDelay = 0.5f;
    float lineLength  = 5.0f;
    [SerializeField]private GameObject playerPrefab;

    public bool isAS{get;private set;}

    public bool isStun{get;private set;}
    #endregion
    bool isAlive = true;
    float timer;
    public Transform BossTransform;
    private Vector3 targetPosition; 
    Boss boss2;
    private void Awake() {
        #region StateMachine
            
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine,"Idle");
        moveState = new PlayerMoveState(this, stateMachine,"Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        attackAState = new PlayerAttackA(this, stateMachine, "AttackA",0.5f,10,3,3,2);
        attackSState = new PlayerAttackS(this, stateMachine, "AttackS");
        attackDState = new PlayerAttackD(this, stateMachine, "AttackD",0.2f,20,3,3,3);
        dashState = new PlayerDashState(this, stateMachine);
        skillAD = new PlayerAD(this,stateMachine);
        skillAS = new PlayerAS(this,stateMachine);
        skillSD = new PlayerSD(this,stateMachine);
        skillASD = new PlayerASD(this,stateMachine);
        #endregion

        lineRenderer = GetComponentInChildren<LineRenderer>();
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in allEnemies) {
            enemies.Add(enemy);
        }
    }

    private void Start() {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        stateMachine.Initalize(idleState);
        curHP = maxHP;
        boss2 = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
    }
    private void Update() {
        if(isAlive){
            if(!isStun){
                stateMachine.currentState.Update();
            }else{
                // stateMachine.ChangeState(idleState);
                dostun();
            }
            FlipController();
            if(curHP <= 0){
                maxHP = 0;
                isAlive = false;
            }
            if(atkdmg){
                timer += Time.deltaTime;
            }

            if(atkdmg&&timer >= 3){
                atkdmg = false;
                timer = 0;
            }
        }
        if(Input.GetKeyDown(KeyCode.Z)){
            boss2.getDamage(20);
        }

    }
    public IEnumerator BusyFor(){
        isBusy = true;
        yield return new WaitForSeconds(0.2f);
        isBusy = false;
    }
    public IEnumerator attackbusy(float seconds){
        isBusy = true;
        yield return new WaitForSeconds(seconds);
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
    public void AttemptToDash(){
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImgPool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;
    }
    public void CheckDash(){
        if(dashTimeLeft > 0){
                 
            rigid.velocity = new Vector2(-dashSpeed * facingDir ,0f);
            dashTimeLeft -= Time.deltaTime;

            if(Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages){
                PlayerAfterImgPool.Instance.GetFromPool();
                
                lastImageXpos = transform.position.x;
            }
        }
        if(dashTimeLeft <= 0 || checkWallAhead()){
            if(checkWallAhead())
                Debug.Log("벽확인");
            rigid.velocity = Vector2.zero;
            stateMachine.ChangeState(idleState);
            }
        }
    public void Holding(){
        float distanceX = BossTransform.position.x - transform.position.x;
        Vector2 direction = new Vector2(distanceX, 0f).normalized;
        SetVelocity(direction.x * 5f ,rigid.velocity.y);
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

        SDtimer += Time.deltaTime;
        while(!checkWallAhead()){
            rigid.velocity = new Vector2(facingDir * -20f,rigid.velocity.y);
            RaycastHit2D hit =Physics2D.Raycast(transform.position, 
            transform.right, AttackRange,LayerMask.GetMask("Enemy"));
            if(hit.collider != null){
                Enemy hits = hit.collider.GetComponent<Enemy>();
                if(hits!=null){
                    hits.isEnter = true;
                    hits.transform.position = transform.position;
                }
            }
            if(skillsdTime()){
                break;
            }
            foreach (Enemy enemy in enemies){
                if (enemy != null && enemy.isEnter){
                    Collider2D collider = enemy.GetComponent<Collider2D>();
                    Transform enemyTransform = enemy.transform;
                    enemyTransform.SetParent(transform);
                    collider.isTrigger = true;
                    enemy.rigid.isKinematic = true;
                    if(!enemy.touchSD){
                        enemy.TakeDamage(10);
                        enemy.touchSD = true;
                    }
                }
            }
            yield return new WaitUntil(() => checkWallAhead()||skillsdTime());
        }
        yield return new WaitForSeconds(0.2f);
        foreach (Enemy enemy in enemies){
            if (enemy != null && enemy.isEnter){
                Transform enemyTransform = enemy.transform;
                Collider2D collider = enemy.GetComponent<Collider2D>();
                enemyTransform.SetParent(null);
                collider.isTrigger = false;
                enemy.rigid.isKinematic = false;
                enemy.isEnter = false;
                if(!enemy.nottouchsd){
                    enemy.TakeDamage(5);
                    enemy.nottouchsd = true;
                }
                yield return new WaitForSeconds(0.2f);
                enemy.offtouch();
            }
        }
        rigid.velocity = Vector2.zero;
        stateMachine.ChangeState(idleState);
        SDtimer = 0;
    }
    bool checkbossAhead() {
        float direction = Mathf.Sign(transform.localScale.x); // 플레이어의 방향을 구함
        Vector2 raycastStart = rigid.position + Vector2.right * 0.5f * facingDir;

        RaycastHit2D rayHit = Physics2D.Raycast(raycastStart, Vector2.right * facingDir, 1, LayerMask.GetMask("Wall")|LayerMask.GetMask("Boss"));

        return (rayHit.collider != null);
    }
    public void checkDownPlatform() {
        RaycastHit2D rayHit = Physics2D.Raycast(groundCheck.position, Vector2.down * 0.5f, 1, LayerMask.GetMask("Floor"));
        Debug.DrawRay(groundCheck.position, Vector2.down * 0.5f, Color.blue);
        if(rayHit.collider != null){
            if(rayHit.collider.CompareTag("DownPlatform")){
                DownPlatform downPlatform = rayHit.collider.GetComponent<DownPlatform>();
                if(downPlatform != null){
                    downPlatform.ChangeLayer();
                }
            }
        }
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
        if (SDtimer >= 1.5f) { 
            return true;
        } else {
            return false;
        }
    }    
    public bool CoolTime(string cool){
        if(cool == "SkillAD"){
            if(!SkillADUsed||Time.time >lastskillad + SkillADCool){
                SkillADUsed = true;
                lastskillad = Time.time;
                Debug.Log("확인");
                return true;
            }else{
                return false;
            }
        }else if(cool == "SkillSD"){
            if(!SkillSDUsed||Time.time >lastskillsd + SkillSDCool){
                SkillSDUsed = true;
                lastskillsd = Time.time;
                return true;
            }else{
                return false;
            }
        }else if(cool == "SkillAS"){
            if(!SkillASUsed||Time.time >lastskillas + SkillADCool){
                SkillASUsed = true;
                lastskillas = Time.time;

                return true;
            }else{
                return false;
            }
        }else if(cool == "SkillASD"){
            if(!SkillASDUsed||Time.time >lastskillasd + SkillASDCool){
                SkillASDUsed = true;
                lastskillasd = Time.time;
                return true;
            }else{
                return false;
            }
        }else if(cool == "Dash"){
            if(!DashUsed||Time.time >lastDashTime + DashCool){
                DashUsed = true;
                lastDashTime = Time.time;
                return true;
            }else{
                return false;
            }
        }else{
            return false;
        }
    }
    public void detailAS(){

        isAS = true;
        lineRenderer.startWidth = 1f;
        lineRenderer.endWidth = 1f;

        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + transform.right * lineLength;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);

        StartCoroutine(DrawRayAfterDelay(startPos,endPos));
    }

    IEnumerator DrawRayAfterDelay(Vector3 startPos, Vector3 endPos){
        yield return new WaitForSeconds(rayDelay);
        lineRenderer.startWidth = 0f;
        lineRenderer.endWidth = 0f;
        Vector3 rayDirection = (endPos - startPos).normalized;

        Vector3 rayOrigin = startPos;

        RaycastHit2D hits = Physics2D.Raycast(rayOrigin, rayDirection, lineLength, LayerMask.GetMask("Enemy"));
        if(hits.collider != null){
            Enemy hit = hits.collider.GetComponent<Enemy>();
            if(hit != null){
                hit.TakeDamage(15);
            }
        }
        Debug.DrawRay(rayOrigin, rayDirection * lineLength, Color.blue, 0.5f);

        isAS = false;
    }

    public void useASD(){
        Vector3 bulletPosition = transform.position + transform.up * 0.5f;
        float direction = facingDir;

        Quaternion rotation = Quaternion.Euler(0, 0, direction*90f);
        if(SkillObj != null){
            GameObject bullet = Instantiate(SkillObj, bulletPosition, rotation);
            Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
            if(bulletRigidbody != null){
                bulletRigidbody.AddForce(transform.right * (facingDir * -50), ForceMode2D.Impulse);
            }
            bullet.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            StartCoroutine(detailASD());

        }else{
            Debug.Log("skillobj가 없음");
        }
    }
    IEnumerator detailASD(){
        bool hasTime = false;
        targetTime = Time.time;
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => CheckEnter()||skilalsdTime()||checkWallAhead());
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null && enemy.isEnter){
                hasTime = true;
                Debug.Log(enemy);
                Transform enemyTransform = enemy.transform;
                Vector3 targetPosition = enemyTransform.position - enemyTransform.forward * 2; // 적의 위치에서 앞으로 2만큼 떨어진 곳으로 설정
                GameObject playerClone = Instantiate(playerPrefab, targetPosition, Quaternion.identity);
                Destroy(gameObject);
                enemy.TakeDamage(50);
                enemy.isEnter = false;
            }
        }
        if(!hasTime)
            yield break;
        }
    
    bool skilalsdTime() {
        if (Time.time - targetTime >= 2f) { 
            return true;
        } else {
            return false;
        }
    }
    bool checkWallAhead() {

        Vector2 raycastStart = rigid.position + Vector2.left * 1f * facingDir; 

        RaycastHit2D rayHit = Physics2D.Raycast(raycastStart, Vector2.left * facingDir, 1, LayerMask.GetMask("Wall")|LayerMask.GetMask("Boss"));


        return (rayHit.collider != null);

    }

    public void getDamage(int hp){
        if(!atkdmg){
            atkdmg = true;
            curHP -= hp;
        }
        if(curHP <= 0){
            isAlive = false;
        }
    }
    public void getStun(){
        isStun = true;
        rigid.constraints = RigidbodyConstraints2D.FreezePositionY|RigidbodyConstraints2D.FreezeRotation;
        rigid.velocity = Vector2.zero;       
        Boss boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        boss.patternoff();
        if(facingDir == 1){
            Flip();
        }
    }
    public void getStunoff(){
        isStun = false;
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

        Debug.Log(isStun);
    }
    void dostun(){
        if(isStun){
            stateMachine.ChangeState(idleState);
            if(Input.GetButtonDown("Horizontal")){
                count++;
                Debug.Log(count);
            }
            if(count >= 8){
                getStunoff();
                Debug.Log("확인00");
                count = 0;
            }
        }
    }
}
