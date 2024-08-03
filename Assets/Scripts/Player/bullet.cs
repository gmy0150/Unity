using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour{
    public GameObject arrowPrefab;
    public float arrowOffset = 1f;
    public int bulletdamgae;
    public Type type;
    public enum Type{Arrow,Skill,None}
    public bool isPenetrate;
    public List<GameObject> instatarrow = new List<GameObject>();   
    TPlayer player;
    Enemy enemy;
    public GameManager gameManager;
    float playerX;
    Rigidbody2D rigid;
    float getx;
    Boss boss;
    bool checkdmg;
    float timer;
    private void OnEnable() {
        getx = player.facingDir;
        timer += Time.deltaTime;
    }
    private void OnDisable() {
        timer = 0;
        checkdmg = false;
    }
    private void Awake() {
        
        player = GameObject.FindWithTag("Player").GetComponent<TPlayer>();

        enemy  = GameObject.FindWithTag ("Enemy").GetComponent<Enemy>();

        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>(); // 게임 매니저 초기화

        rigid = GetComponent<Rigidbody2D>();

        boss = GameObject.FindWithTag("Boss").GetComponent<Boss>();
    }
    private void Update() {
        rigid.velocity = new Vector2(-getx * 20f,rigid.velocity.y);
        if(timer >= 3f){
            PlayerArrowPool.Instance.ReturnArrow(gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D collision) {
    }

    void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log(1);
        if(collision.gameObject.tag == "Wall"){
            if(type == Type.Arrow)
            PlayerArrowPool.Instance.ReturnArrow(gameObject);
            else
            Destroy(gameObject);
        }
        if(collision.gameObject.tag == "Floor"){
            if(type == Type.Arrow)
            PlayerArrowPool.Instance.ReturnArrow(gameObject);
            else
            Destroy(gameObject);
        }
        if(collision.CompareTag("Enemy")||collision.CompareTag("Boss")){
            if(type == Type.Arrow){

                Vector3 collisionPoint = collision.ClosestPoint(transform.position);
                Vector3 arrowPosition = collisionPoint;
                
                if(isPenetrate){
                    GameObject newArrow = Instantiate(arrowPrefab,arrowPosition,Quaternion.identity);
                    if(getx > 0){
                        newArrow.transform.localScale = new Vector3(-1,1,1);
                    }

                    newArrow.transform.parent = collision.transform;
                    gameManager.AddArrow(newArrow);

                    gameManager.a++;
                    if(!checkdmg){
                        if(collision.tag == "Boss")
                            boss.getDamage(10,1);
                            Debug.Log("데미지?");
                        if(collision.tag == "Enemy"){
                            enemy.getDamage(10,1);
                        }
                    }
                    if (gameManager.ArrowCount() > 1)
                    {
                        GameObject oldestArrow = gameManager.GetOldestArrow();
                        gameManager.RemoveArrow(oldestArrow);
                        Destroy(oldestArrow);
                    }
                }
                PlayerArrowPool.Instance.ReturnArrow(gameObject);
            }
            else if(type == Type.Skill){
                Vector3 bulletDir = rigid.velocity;
                player.PlayerMove(collision.transform,bulletDir);
                if(collision.tag == "Boss")
                    boss.getDamage(20,20);
                if(collision.tag == "Enemy")
                    enemy.getDamage(20,20);
                Destroy(gameObject);
            }else{
                Destroy(gameObject);
            }
        }else if(collision.CompareTag("BossHeat")){
            if(type != Type.None){
                boss.getDamage(20,1);
                checkdmg = true;
            }
            
        }
        
    }
}
   