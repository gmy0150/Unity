using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossStun : MonoBehaviour
{
    // Start is called before the first frame update
    Boss boss;
    TPlayer player;
    public Transform target; // 스케일을 변경할 오브젝트
    public float targetScaleY = 4f; // 목표 y 스케일 배율
    public float duration = 2f; // 스케일 변경 시간

    private Vector3 originalScale;

    void Start()
    {
        originalScale = target.localScale;
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<TPlayer>();
        IncreaseScale();
    }

    public void IncreaseScale()
    {
        StartCoroutine(IncreaseScaleCoroutine());
    }

    private IEnumerator IncreaseScaleCoroutine()
    {
        float elapsedTime = 0f;
        Vector3 initialScale = originalScale;
        Vector3 finalScale = new Vector3(originalScale.x, originalScale.y * targetScaleY, originalScale.z);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            target.localScale = Vector3.Lerp(initialScale, finalScale, t);
            yield return null;
        }

        // 마지막 프레임에 정확한 스케일 적용
        target.localScale = finalScale;
    }

    // private void OnCollisionEnter2D(Collider2D other) {
    //     if(other.tag == "Player"){
    //         player.getStun();
    //     }
    // }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.transform.tag == "Player"){
            player.getStun();
            player.getDamage(5);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
