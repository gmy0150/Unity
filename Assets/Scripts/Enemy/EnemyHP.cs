using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    // [SerializeField]protected enum Type{melee,ranger,elite,Boss};
    // [SerializeField]protected Type enemyType;
    [SerializeField]protected enum Type{melee,ranger,elite,Boss};
    [SerializeField]protected Type enemyType;
    [SerializeField]protected int maxHealth = 100;
    [SerializeField]protected int curHealth;
    [SerializeField]protected int maxShiled = 100;
    [SerializeField]protected int curShiled;
    [SerializeField]protected HealthBarUI healthbar;
    // [SerializeField]protected GameObject particle;
    Boss boss;

    public virtual void Start()
    {
        
        healthbar.UpdateHealthBar(curHealth,maxHealth);
        if(enemyType == Type.elite)
            healthbar.UpdateShieldBar(curShiled,maxShiled);
    }
    public virtual void Awake() {
        healthbar = GetComponentInChildren<HealthBarUI>();
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
    }
    public virtual void getDamage(int hp,int shiledhp){
        if(curShiled > 0){
            curShiled -= shiledhp;
            healthbar.UpdateShieldBar(curShiled,maxShiled);
        }
        else if(curHealth >= 0){
            curHealth -= hp;
            healthbar.UpdateHealthBar(curHealth,maxHealth);
        }
        // if(curHealth <= 0){
        //     setDoor(die);
        //     Debug.Log("죽음");
        // }
    
    }
    void Update()
    {
        
    }
}
