using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private TPlayer player => GetComponentInParent<TPlayer>();
    private HealthBarUI healthBar =>GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBarUI>();
    private void AnimationTrigger(){
        player.AnimationTrigger();
    }
    private void moveTrigger(){
        player.MoveTrigger();
    }
    void animationEnd(){
        healthBar.destroyShield();
    }
}
