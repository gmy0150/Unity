using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour{
    public GameObject arrowPrefab;
    public float arrowOffset = 1f;
    public int bulletdamgae;
    public Type type;
    public enum Type{Arrow,Skill}
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


    private void OnEnable() {
        getx = player.facingDir;
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
    }
    void OnCollisionEnter2D(Collision2D collision) {
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Wall"){
            PlayerArrowPool.Instance.ReturnArrow(gameObject);
            // Destroy(gameObject);
        }
        if(collision.gameObject.tag == "Floor"){
            // Destroy(gameObject);
            PlayerArrowPool.Instance.ReturnArrow(gameObject);
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
                    }
                    if (gameManager.ArrowCount() > 1)
                    {
                        GameObject oldestArrow = gameManager.GetOldestArrow();
                        gameManager.RemoveArrow(oldestArrow);
                        Destroy(oldestArrow);
                    }
                PlayerArrowPool.Instance.ReturnArrow(gameObject);
                }
                

            }
            else if(type == Type.Skill){
                enemy.isEnter = true;
            // Destroy(gameObject,5f);
            }
        }else if(collision.CompareTag("BossHeat")){
            boss.getDamage(20,1);
            checkdmg = true;
        }
        if(collision == null){
            PlayerArrowPool.Instance.ReturnArrow(gameObject,5f);

        }
    }
}
   