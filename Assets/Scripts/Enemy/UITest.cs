using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class UITest : MonoBehaviour
{
    float timer;
    Animator animator = new Animator();
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        if(animator!= null)
        Debug.Log("?11");

    }

    // Update is called once per frame
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
            animator.SetFloat("Blend",1f);
        }
        else if(timer > 5){
            animator.SetFloat("Blend",0.5f);
        }
        else if(timer > 3){
            animator.SetBool("UI",true);
            animator.SetFloat("Blend",0f);
        Debug.Log("?");

        }
    }
}
