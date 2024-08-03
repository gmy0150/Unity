using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FollowCamera : MonoBehaviour
{
    public static FollowCamera Instance{get; private set;}
    public GameObject player;
    Vector3 originPos;
    public float ShakeAmount = 3.0f;
    public float ShakeTime = 2.0f;
    bool isShaking;
    float saveSahke;
    public Image damageimg;
    // public float fadespeed = 5f;
    float targetAlpha = 0f;
    Coroutine damageCoroutine;
    private void Awake() {
        if(Instance == null){
            Instance = this;
            originPos = transform.position;
        }else  
            Destroy(gameObject);
    }
    void Start()
    {
        // saveSahke = ShakeTime;

    }

    void Update()
    {
        player = GameObject.FindWithTag("Player");
        transform.position = new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z - 10);
        if(!isShaking){
        originPos = transform.position;
        }
    }

    public void ShakeCamera(){
        if(!isShaking){
            // ShakeTime = saveSahke;
            ShowDamage();

            isShaking = true;
            StartCoroutine(PerformShake());
        }
    }
    private IEnumerator PerformShake() {
        float shakeEndTime = Time.time + ShakeTime;
        while (Time.time < shakeEndTime)
    {
        // 플레이어의 현재 위치를 기반으로 흔들림을 적용
        Vector3 followPlayerPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 10);
        transform.position = followPlayerPos + Random.insideUnitSphere * ShakeAmount;
        yield return null;
    }
        transform.position = originPos;
        isShaking = false;
    }
    void ShowDamage(){
        if(damageCoroutine != null){
            StopCoroutine(damageCoroutine);
        }
        damageCoroutine = StartCoroutine(DamageEffect());
    }
    IEnumerator DamageEffect(){
        SetAlpha(5/255.0f);
        yield return new WaitForSeconds(0.2f);
        SetAlpha(0f);
    }
    void SetAlpha(float alpha){
        Color color = damageimg.color;
        color.a = alpha;
        damageimg.color = color;
    }
}
