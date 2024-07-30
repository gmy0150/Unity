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
    public virtual void Start()
    {
        
        healthbar.UpdateHealthBar(curHealth,maxHealth);
        if(enemyType == Type.elite)
            healthbar.UpdateShieldBar(curShiled,maxShiled);
    }
    public virtual void Awake() {
        healthbar = GetComponentInChildren<HealthBarUI>();
    }
    void Update()
    {
        
    }
}
