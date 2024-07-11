using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class UITest : MonoBehaviour
{
    float timer;
    Animator animator = new Animator();
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        if(animator == null)
        Debug.Log("애니매이션 없음");

    }

    void Update()
    {
        timer += Time.deltaTime;

        if(timer > 15){
            animator.SetBool("anim",false);
        }
        else if(timer > 10){
            animator.SetBool("UI",false);
            animator.SetBool("anim",true);
        }
        else if(timer > 8){
        }
        else if(timer > 5){
        }
        else if(timer > 3){


        }
    }
}
