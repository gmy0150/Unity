using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UIElements;

public class Boss : EnemyHP
{
    [SerializeField]private GameObject rockStart;
    [SerializeField]private GameObject tlaser;
    [SerializeField]private GameObject laser;
    TPlayer player;
    public int flooringdmg;
    BoxCollider2D sadFlooring;
    public float dropInterval = 3f;
    public float laserinterval = 6f;
    public int laserDmg;
    public bool angerdoor = true;
    public bool saddoor = false;
    public bool happydoor = false;

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
    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<TPlayer>();
        rigid = GetComponent<Rigidbody2D>();
        base.Awake();
    }
    private void Start() {
        StartCoroutine(InitializeBoxCollider());
        sadflooreffect.SetActive(false);

        enemyType = Type.Boss;
    }
    private IEnumerator InitializeBoxCollider()
    {
        sadflooreffect = BossEffectPool.Instance.GetfloorEffect();

        sadflooreffect.transform.position = new Vector3(rockStart.transform.position.x, -2.5f, 0);
        yield return new WaitForSeconds(1f);

        sadFlooring = sadflooreffect.GetComponent<BoxCollider2D>();
        ParticleSystem sadFlooreff = sadFlooring.GetComponent<ParticleSystem>();
        sadFlooring.enabled = true;

        boxCenter = sadFlooring.bounds.center;
        boxSize = sadFlooring.size;
    }
    void Update() {
        timer += Time.deltaTime;
        transtimer += Time.deltaTime;
        if(angerdoor){
            if(!islaser){
                pattern1timer += Time.deltaTime;
                }
            if(pattern1timer >= laserinterval){
                StartCoroutine("Laser");
                pattern1timer = 0;
            }
            if(timer >= dropInterval){   
                Vector3 dropPosition = GetRandomPointInBox();
                StartCoroutine(DropWithEffect(dropPosition));
                timer = 0;
            }
            if(transtimer > 5){
                saddoor = true;
                angerdoor = false;
                happydoor = false;
                transtimer = 0;
            }
        }
        if(saddoor){
            if(isFloor)
                DrawBox();
            if(timer > 5f){
                if(!isFloor){
                    Vector3 droppos = rockStart.transform.position;
                    droppos.y = 0;
                    StartCoroutine(sadfloor(droppos));
                    // timer = 0;
                    isFloor = true;
                }
            }
            if(!player.isStun){
                pattern1timer += Time.deltaTime;
                if(pattern1timer >= 3f){
                    StartCoroutine("SadPattern2");
                    // player.isStun = true;
                }
            }
            // if(transtimer >= 20f){
            //     saddoor = false;
            //     angerdoor = true;
            //     happydoor = false;
            //     BossEffectPool.Instance.ReturnEffectall();
            //     transtimer = 0;
            // }
        }
    }

    public void patternoff(){
        pattern1timer = 0;
        Debug.Log(pattern1timer);
        Debug.Log("작동중");
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
        
        laser.SetActive(true);
        float laserDuration = 2f; 
        float elapsedTime = 0f;

        while (elapsedTime < laserDuration)
        {
            RaycastHit2D hit =Physics2D.Raycast(laser.transform.position, 
            -laser.transform.right, 10f,LayerMask.GetMask("Player"));
            if(hit.collider != null){
                player = hit.collider.GetComponent<TPlayer>();
                if(player!=null){
                    if(!atk){
                        Debug.Log("확인");
                        player.getDamage(laserDmg);
                        atk = true;
                    }
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
    Vector3 GetRandomPointInBox(){
        BoxCollider2D collider = rockStart.GetComponent<BoxCollider2D>();
        if (collider != null) {
            Vector2 size = collider.size;
            Vector2 offset = collider.offset;
            float x = Random.Range(-size.x / 2, size.x / 2) + offset.x;
            float y = Random.Range(-size.y / 2, size.y / 2) + offset.y;
            Vector3 dropPosition = new Vector3(x, y, 0); 
            
            return rockStart.transform.TransformPoint(dropPosition); 
        }
        return rockStart.transform.position;
    }
    IEnumerator sadfloor(Vector3 position){
        sadflooreffect.SetActive(true);
        
        // sadflooreffect = BossEffectPool.Instance.GetfloorEffect();
        // sadflooreffect.transform.position = new Vector3(position.x,-2.5f,0);
        yield return new WaitForSeconds(1f);
        // sadFlooring = sadflooreffect.GetComponent<BoxCollider2D>();
        // sadFlooring.enabled = true;
        if(transtimer >= 16f){
        }
    }
    private void DrawBox()
    {
        Vector2 topEdgeCenter = boxCenter + new Vector2(-boxSize.x/2, 0.5f);
        Vector2 rayDirection = Vector2.right;
        float rayDistance = 20;

        RaycastHit2D hit = Physics2D.Raycast(topEdgeCenter, rayDirection, rayDistance);
        Debug.DrawRay(topEdgeCenter, rayDirection * rayDistance, Color.red);

        // Transform hitTransform = hit.transform;

        if (hit.collider.CompareTag("Player"))
        {
            TPlayer player = hit.transform.GetComponentInParent<TPlayer>();
        }
        if(player != null ){
            player.getDamage(5);
        }

    }
    
}
