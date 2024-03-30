using System.Collections;
using System.Collections.Generic;
// using System.Diagnostics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum Type{Shiled, normal};
    public Type enemyType;
    public int maxHealth;
    public int curHelath;
    public SpriteRenderer[] sprite;
    public GameObject particle;
    public bool isDead;
    public bool isEnter;
    public Rigidbody2D rigid;
    public GameObject MSword;
    void Awake(){
        sprite = GetComponentsInChildren<SpriteRenderer>();//MeshRenderer에서 material을 뽑아올 때는 소문자로 작성

    }
    void Update(){
        if(enemyType == Type.normal){
            particle.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag =="Hammer"){
            Player weapon = other.GetComponentInParent<Player>();
        if(weapon != null){
            switch(enemyType){
                case Type.normal:
                    curHelath -= weapon.Hammerdamgae;
                break;
                case Type.Shiled:
                    curHelath -= weapon.Hammerdamgae + 5;
                break;
        }
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec));
        }

    }
    else if(other.tag =="Sword"){
        Player weapon = other.GetComponentInParent<Player>();
        if(weapon != null){
            switch(enemyType){
                case Type.normal:
                    curHelath -= weapon.Sworddamage + 3;
                break;
                case Type.Shiled:
                    curHelath -= weapon.Sworddamage;
                break;
        }
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec));
        }

    } 
    else if(other.tag == "Bullet"){
            bullet bulletd = other.GetComponent<bullet>();
            if(bulletd != null){
                if(bulletd.type == bullet.Type.Skill){
                    isEnter = true;
                    curHelath -= bulletd.bulletdamgae;
                    Debug.Log("총알닿음1?");
                    Vector3 reactVec = transform.position - other.transform.position;
                    // MSword.SetActive(true);
                    // Destroy(other.gameObject);

                    StartCoroutine(OnDamage(reactVec));
                    
                    StartCoroutine("DestroySwd");
                    Debug.Log("dddd");
                    
                }else if(bulletd.type == bullet.Type.Arrow){
                    curHelath -= bulletd.bulletdamgae;
                    
                    Vector3 reactVec = transform.position - other.transform.position;
                    StartCoroutine(OnDamage(reactVec));
                    Debug.Log("총알닿음2?");
                }
            }
        }
        else if(other.tag =="Skill"){
            Player weapon = other.GetComponentInParent<Player>();
            if(weapon != null){
                switch(enemyType){
                    case Type.normal:
                        curHelath -= weapon.Sworddamage;
                    break;
                    case Type.Shiled:
                        curHelath -= weapon.Sworddamage;
                    break;
            }
                isEnter = true;
                Vector3 reactVec = transform.position - other.transform.position;
                StartCoroutine(OnDamage(reactVec));
                
                StartCoroutine("isEnterdel");
            }
        } 
    }
    

    IEnumerator OnDamage(Vector3 reactVec){
        foreach(SpriteRenderer mesh in sprite)
            mesh.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if(curHelath > 0){
            foreach(SpriteRenderer mesh in sprite)
                mesh.color = Color.white;
            reactVec = reactVec.normalized;
            rigid.AddForce(reactVec * 5, ForceMode2D.Impulse);
        }
        else{
            foreach(SpriteRenderer mesh in sprite)
                mesh.color = Color.gray;
            gameObject.layer = 12;
            isDead = true;
        }

    }
    IEnumerator DestroySwd(){
        yield return new WaitForSeconds(2.5f);
        isEnter = false;
        // MSword.SetActive(false);
    }
    IEnumerator isEnterdel(){
        yield return new WaitForSeconds(2.5f);
        isEnter = false;
        // MSword.SetActive(false);
    }
}