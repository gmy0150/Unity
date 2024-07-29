using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;

public class Boss : EnemyHP
{
    [SerializeField]private GameObject rockStart;
    [SerializeField]private GameObject tlaser;
    [SerializeField]private GameObject laser;
    [SerializeField]private GameObject brickstart;

    TPlayer player;
    public int flooringdmg;
    BoxCollider2D sadFlooring;
    public float dropInterval = 3f;
    public float laserinterval = 6f;
    public int laserDmg;
    public bool angerdoor {get;private set;}
    public bool saddoor {get;private set;}
    public bool happydoor {get;private set;}
    string angry = "angry";
    string sad = "sad";
    string happy = "happy";
    string die = "die";
    public LayerMask layerMask;
    bool isFloor;
    GameObject sadflooreffect;
    bool islaser;
    int lasery;
    float timer;
    float dmgtimer;
    public static float pattern1timer;
    bool atk;
    public float transtimer = 0;
    Rigidbody2D rigid;
    private Vector2 boxCenter;
    private Vector2 boxSize;
    private Vector2 laserSize;     // 레이저 크기 저장
    private Vector3 laserPosition;
    public bool hold{get; private set;}
    public GameObject shiledimage;
    public Sprite saveUI;
    public GameObject weakness;
    bool bossdie;
    bool bossGroggy;
    // public Sprite angryImage;
    // public Sprite saveImage;

    Animator animator;
    bool init;
    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<TPlayer>();
        animator = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        base.Awake();
    }
    void setDoor(string Pdoor){
    angerdoor = false;
    saddoor = false;
    happydoor = false;
    bossdie = false;
    switch(Pdoor) {
        case "angry":
            angerdoor = true;
            break;
        case "sad":
            saddoor = true;
            break;
        case "happy":
            happydoor = true;
            break;
        case "die":
            bossdie = true;
            break;
        default:
            break;
    }
}
    private void Start() {
        StartCoroutine(InitializeBoxCollider());
        sadflooreffect.SetActive(false);
        setDoor(angry);
        enemyType = Type.Boss;
        base.Awake();
        shiledimage.SetActive(false);
        weakness.SetActive(false);
    }
    private IEnumerator InitializeBoxCollider()
    {
        sadflooreffect = BossEffectPool.Instance.GetfloorEffect();
        RaycastHit2D hit = Physics2D.Raycast(
            rockStart.transform.position, 
            Vector2.down, 
            Mathf.Infinity, 
            LayerMask.GetMask("Floor")
        );

        if (hit.collider != null)
        {
            // 충돌한 floor 레이어의 y 좌표를 사용
            float floorY = hit.collider.bounds.max.y;
            sadflooreffect.transform.position = new Vector3(rockStart.transform.position.x, floorY, 0);
        }
        yield return new WaitForSeconds(1f);

        sadFlooring = sadflooreffect.GetComponent<BoxCollider2D>();
        ParticleSystem sadFlooreff = sadFlooring.GetComponent<ParticleSystem>();
        sadFlooring.enabled = true;

        boxCenter = sadFlooring.bounds.center;
        boxSize = sadFlooring.size;
    }

    void Update() {
        timer += Time.deltaTime;
        if(angerdoor){
            Angrydoor();
        }
        if(saddoor){
            Saddoor();
        }
        if(happydoor){
            Happydoor();
            }
        }
    void Angrydoor(){
        // transtimer += Time.deltaTime;
        if(!islaser){
            pattern1timer += Time.deltaTime;
            }
        if(pattern1timer >= laserinterval){
            StartCoroutine("Laser");
            pattern1timer = 0;
        }
        if(timer >= dropInterval){   
            Vector3 dropPosition = GetRandomPointInBox(rockStart);
            StartCoroutine(DropWithEffect(dropPosition));
            timer = 0;
        }
        if(transtimer > 5){
            setDoor(sad);
            transtimer = 0;
        }
    }
    void Saddoor(){
        if(!init){
            maxShiled = 100;
            curShiled = 100;
            shiledimage.GetComponent<Image>().sprite = saveUI;
            shiledimage.SetActive(true);
            healthbar.UpdateShieldBar(curShiled,maxShiled);
            init = true;
        }
        if(timer > 5f){
            if(!isFloor){
                sadfloor();
            }
        }
        if(!player.isStun&&!bossGroggy){
            pattern1timer += Time.deltaTime;
            if(pattern1timer >= 3f){
                StartCoroutine("SadPattern2");
            }
        }
        if(curShiled <= 0){
        transtimer += Time.deltaTime;
        isFloor = true;
        bossGroggy = true;
        BossEffectPool.Instance.ReturnEffectall();
        pattern1timer = 0;
        player.getStunoff();
            if(transtimer >= 7f){
                int range = Random.Range(0,2);
                if(range == 0){
                    setDoor(happy);
                    transtimer = 0;
                    init = false;
                }
                if(range == 1){
                    setDoor(angry);
                    transtimer = 0;
                    init = false;
                }

            }
        }

    }
    void Happydoor(){
        transtimer += Time.deltaTime;
        animator.SetBool("Stun",true);
        weakness.SetActive(true);      
        if(timer > 3f){
            for(int i = 1; i<= 3; i++){
                Vector3 brick = GetRandomPointInBox(brickstart);
                Vector3 rock = GetRandomPointInBox(rockStart);
                StartBrick(brick, rock);
            }
        timer = 0;
        }
        if(transtimer >= 7f){
                int range = Random.Range(0,2);
                if(range == 0){
                    setDoor(sad);
                    Debug.Log("슬픔");
                    transtimer = 0;
                }
                if(range == 1){
                    setDoor(angry);
                    Debug.Log("화");
                    transtimer = 0;
                }

            }
    }
    public void getDamage(int hp){
        if(curShiled >= 0){
            curShiled -= hp;
            healthbar.UpdateShieldBar(curShiled,maxShiled);
        }
        else if(curHealth >= 0){
            curHealth -= hp;
            healthbar.UpdateHealthBar(curHealth,maxHealth);
        }
        if(curHealth <= 0){
            Debug.Log("죽음");
            setDoor(die);
        }

    }
    public void patternoff(){
        pattern1timer = 0;
    }
    IEnumerator Laser(){
        int y = Random.Range(0,3);
        islaser = true;
        pattern1timer = 0;
        if(y == 0){
            lasery = -1;
            tlaser.transform.position = new Vector3(tlaser.transform.position.x,lasery,0);
        }
        if(y == 1){
            lasery = 2;
            tlaser.transform.position = new Vector3(tlaser.transform.position.x,lasery,0); 
        }
        
        if(y == 2){
            lasery = 5;
            tlaser.transform.position = new Vector3(tlaser.transform.position.x,lasery,0); 
        }

        tlaser.SetActive(true);
        yield return new WaitForSeconds(2f);
        tlaser.SetActive(false);
        laser.transform.position = new Vector3(laser.transform.position.x,lasery,0);
        
        laserSize = laser.GetComponent<SpriteRenderer>().bounds.size;
        laserPosition = laser.transform.position;
        // 박스 캐스트 실행

        laser.SetActive(true);
        float laserDuration = 1f; 
        float elapsedTime = 0f;

        while (elapsedTime < laserDuration)
        {
            RaycastHit2D rayed = Physics2D.BoxCast(
                laser.transform.position, 
                laserSize, 
                0f, 
                Vector2.left, 
                0f, 
                layerMask
            );
            if (rayed.collider != null)
            {
                if (rayed.collider.CompareTag("Player"))
                {
                    player.getDamage(laserDmg);
                }
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        laser.SetActive(false);
        pattern1timer = 0;
        islaser = false;
        atk = false;
    }

    private void OnDrawGizmos() {
        Vector2 rayOrigin = tlaser.transform.position;
        Vector2 rayDirection = -tlaser.transform.right;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(rayOrigin, rayOrigin + rayDirection * 10f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(laserPosition, laserSize);
    }
    IEnumerator DropWithEffect(Vector3 position) {
        GameObject effect = BossEffectPool.Instance.GetEffect();
        effect.transform.position = new Vector3(position.x,-2.5f,0);
        yield return new WaitForSeconds(2f); 
        BossEffectPool.Instance.ReturnEffect(effect);
        yield return new WaitForSeconds(0.3f); 
        GameObject drop = BossRockPool.Instance.GetFromPool();
        drop.transform.position = position;
        yield return new WaitForSeconds(2f); 
        BossRockPool.Instance.AddToPool(drop);
    }
    void StartBrick(Vector3 position, Vector3 positions){
        GameObject drop = BossRockPool.Instance.GetBrickFromPool();
        drop.transform.position = position;
        GameObject drops = BossRockPool.Instance.GetBrickFromPool();
        drops.transform.position = positions;
    }
    
    IEnumerator SadPattern2() {
        // GameObject effect = BossEffectPool.Instance.GetSadPattern2();
        // effect.transform.position = new Vector3(effect.transform.position.x,-2.5f,0);
        // yield return new WaitForSeconds(2f); 
        // BossEffectPool.Instance.ReturnSadPattern2();
        // player = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<TPlayer>();
        player.getStun();
        yield return new WaitForSeconds(0.3f); 

        // GameObject drop = BossRockPool.Instance.GetFromPool();

        // drop.transform.position = position;
        // yield return new WaitForSeconds(2f); 
        // BossRockPool.Instance.AddToPool(drop);
    }
    Vector3 GetRandomPointInBox(GameObject gameObject){
        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
        if (collider != null) {
            Vector2 size = collider.size;
            Vector2 offset = collider.offset;
            float x = Random.Range(-size.x / 2, size.x / 2) + offset.x;
            float y = Random.Range(-size.y / 2, size.y / 2) + offset.y;
            Vector3 dropPosition = new Vector3(x, y, 0); 
            
            return gameObject.transform.TransformPoint(dropPosition); 
        }
        return gameObject.transform.position;
    }
    void sadfloor(){
        sadflooreffect.SetActive(true);
        DrawBox();

    }
    private void DrawBox()
    {
        Vector2 topEdgeCenter = boxCenter + new Vector2(-boxSize.x/2, 0.5f);
        Vector2 rayDirection = Vector2.right;
        float rayDistance = 20;

        RaycastHit2D hit = Physics2D.Raycast(topEdgeCenter, rayDirection, rayDistance,LayerMask.GetMask("Player"));
        Debug.DrawRay(topEdgeCenter, rayDirection * rayDistance, Color.red);


        if (hit.collider!=null&&hit.collider.CompareTag("Player"))
        {
            player.getDamage(5);
        Debug.Log("확인");
        }else{
            Debug.Log("플레이어없음");
        }

    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.transform.tag == "Player"){
            player.getDamage(20);
        }
    }
}
