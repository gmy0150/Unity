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
    [SerializeField]protected int curHelath;
    [SerializeField]protected int maxShiled = 100;
    [SerializeField]protected int curShiled;
    [SerializeField]protected HealthBarUI healthbar;
    // [SerializeField]protected GameObject particle;
    public virtual void Start()
    {
        if(enemyType != Type.Boss)
            healthbar.UpdateShieldBar(curShiled,maxShiled);
    }
    public virtual void Awake() {
        healthbar = GetComponentInChildren<HealthBarUI>();
        if(healthbar != null)
        Debug.Log(healthbar.transform.parent.parent+ "헬스바있음");
        
    }
    void Update()
    {
        
    }
}
