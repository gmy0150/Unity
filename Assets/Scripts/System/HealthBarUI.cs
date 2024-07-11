using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider Healthslider;
    [SerializeField] private Slider backgroundSlider;
    [SerializeField] private Slider Shiledslider;
    [SerializeField] private GameObject Shiled;
    Animator animator;
    public float lerpSpeed = 1.0f;
    private void Awake() {
        animator = Shiled.GetComponent<Animator>();   
    }

    public void destroyShield() {
        animator.SetBool("anim",false);
        Shiled.SetActive(false);
    }

    public void UpdateHealthBar(float curHelath,float maxHealth){
        Healthslider.value = curHelath/maxHealth;
        SlowHP(curHelath / maxHealth);
    }
    public void UpdateShieldBar(float curShiled,float maxShiled){
        Shiledslider.value = curShiled / maxShiled;
        if(Shiledslider.value == 1f){
            animator.SetBool("UI",true);
            animator.SetFloat("Blend",0f);
        }
        if(Shiledslider.value <= 0.7f){
            animator.SetFloat("Blend",0.5f);
        }
        if(Shiledslider.value <= 0.3f){
            animator.SetFloat("Blend",1f);
        }
        if(Shiledslider.value == 0f){
            animator.SetBool("UI",false);
            animator.SetBool("anim",true);
            
        }
    }
    void SlowHP(float hp){
        StartCoroutine(SlowHPCoroutine(hp));
    }
    IEnumerator SlowHPCoroutine(float targetHP) {
    float currentHP = backgroundSlider.value;
    while (Mathf.Abs(currentHP/targetHP) > 0.05f)  {
            currentHP = Mathf.Lerp(currentHP, targetHP, Time.deltaTime * lerpSpeed);
            backgroundSlider.value = currentHP;
            yield return null;
        }
    backgroundSlider.value = targetHP;
    }
    void Update()
    {
    }
}
