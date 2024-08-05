using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.UIElements;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider backgroundSlider;
    [SerializeField] private Slider Shiledslider;
    [SerializeField] private Image Healthsliders;
    [SerializeField] private GameObject Shiled;
    bool delete;
    Animator animator;
    float timeA;
    public float lerpSpeed = 1.0f;
    private void Awake() {
        animator = GetComponentInChildren<Animator>();
        // if(animator == null)
        //Debug.Log(animator.transform.parent.parent+"애니매이션 없음");
    }
    private void Start() {

    }
    public void destroyShield() {
        if(delete){
            animator.SetBool("anim",false);
            Shiled.SetActive(false);
        }
    }
    public float getHealth(){
        Debug.Log(animator.transform.parent.parent.parent.name);
        return Healthsliders.fillAmount;
    }
    public void UpdateHealthBar(float curHelath,float maxHealth){
        // Healthslider.value = curHelath/maxHealth;
        Healthsliders.fillAmount = curHelath/maxHealth;
        SlowHP(curHelath / maxHealth);
    }
    public void UpdateShieldBar(float curShiled,float maxShiled){
        Shiledslider.value = curShiled / maxShiled;


        animator.SetBool("UI",true);
        animator.SetFloat("Blend",Shiledslider.value);

        if(Shiledslider.value == 0f){
            animator.SetBool("UI",false);
            animator.SetBool("anim",true);
            delete = true;
        }
    }
    void SlowHP(float hp){
        StartCoroutine(SlowHPCoroutine(hp));
    }
    IEnumerator SlowHPCoroutine(float targetHP) {
        float currentHP = backgroundSlider.value;
        float duration = 1f;
        float startTime = Time.time;
        float lerpSpeed = 1f / duration;

        while (Time.time - startTime < duration) {
            currentHP = Mathf.Lerp(currentHP, targetHP, Time.deltaTime * lerpSpeed);
            backgroundSlider.value = currentHP;
            yield return null;
        }

        // 최종적으로 targetHP로 설정
        backgroundSlider.value = targetHP;
    }
    void Update()
    {
    }
}
