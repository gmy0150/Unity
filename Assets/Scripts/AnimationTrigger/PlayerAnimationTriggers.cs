using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private TPlayer player => GetComponentInParent<TPlayer>();
    Animator animator;
    public List<HealthBarUI> healthBars = new List<HealthBarUI>();

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("애니메이터가 없습니다.");
        }
        HealthBarUI[] allHealth = FindObjectsOfType<HealthBarUI>();
        foreach (HealthBarUI health in allHealth) {
            healthBars.Add(health);
        }
    }
    private void AnimationTrigger(){
        player.AnimationTrigger();
    }
    private void moveTrigger(){
        player.MoveTrigger();
    }
    
    void animationEnd(){
        foreach (HealthBarUI health in healthBars){
            // if(health.getHealth() >= 1f)
            health.destroyShield();
        }
    }
}
