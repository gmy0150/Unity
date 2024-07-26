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
    public void SetPlayerX(float x)
    {
        playerX = x;
    }
    private void Start() {
        getx = player.facingDir;
        Debug.Log("재생");
    }
    private void Awake() {
        
        player = GameObject.FindWithTag("Player").GetComponent<TPlayer>();

        enemy  = GameObject.FindWithTag ("Enemy").GetComponent<Enemy>();

        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>(); // 게임 매니저 초기화

        rigid = GetComponent<Rigidbody2D>();
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
            Debug.Log("적중?");
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
                 
                    if (gameManager.ArrowCount() > 1)
                    {
                        GameObject oldestArrow = gameManager.GetOldestArrow();
                        gameManager.RemoveArrow(oldestArrow);
                        Destroy(oldestArrow);
                    }
                PlayerArrowPool.Instance.ReturnArrow(gameObject);
                
            }
            PlayerArrowPool.Instance.ReturnArrow(gameObject);

            }
            else if(type == Type.Skill){
                enemy.isEnter = true;
            // Destroy(gameObject,5f);
            }
        }else if(collision == null){
            PlayerArrowPool.Instance.ReturnArrow(gameObject,5f);

        }
    }
}
   