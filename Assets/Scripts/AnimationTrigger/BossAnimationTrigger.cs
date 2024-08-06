using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    private Boss boss => GetComponentInParent<Boss>();


    private void bossTrigger(){
        boss.pauseTrigger();
    }
    void destroyTrigger(){
        // boss.destroyTrigger();
        gameObject.SetActive(false);
    }
    void LoopTrigger(){
        boss.LoopTrigger();
    }
}
