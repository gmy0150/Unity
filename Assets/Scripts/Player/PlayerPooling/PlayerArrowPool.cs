using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerArrowPool : MonoBehaviour
{
    public static PlayerArrowPool Instance { get; private set; }
    public GameObject ArrowPrefeb;
    private Queue<GameObject> ArrowPool = new Queue<GameObject>();
    public int poolSize = 20;
    float timer;
    TPlayer player;
    private void Awake() {
        Instance = this;
        InitializePool();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<TPlayer>();
    }

    private void InitializePool() {
        for (int i = 0; i < poolSize; i++) {
            GameObject Arrow = Instantiate(ArrowPrefeb);
            Arrow.transform.SetParent(transform);
            Arrow.transform.position = gameObject.transform.position;
            Arrow.SetActive(false);
            ArrowPool.Enqueue(Arrow);
        }
    }
    public GameObject GetArrow() {
        if (ArrowPool.Count == 0) {
            InitializePool();
        }
        GameObject Arrow = ArrowPool.Dequeue();
        if(player.facingRight){
            Arrow.transform.rotation = Quaternion.Euler(0,0,0);
            Debug.Log("확인");
        }else{
            Arrow.transform.rotation = Quaternion.Euler(0,180,0);
        }
        Arrow.SetActive(true);
        Arrow.transform.SetParent(null);

        return Arrow;
    }
    private void Update() {
        timer += Time.deltaTime;
    }
    public void ReturnArrow(GameObject Arrow, float seconds = 0) {
    if (seconds < timer) {
        Arrow.SetActive(false);
        Arrow.transform.SetParent(transform);
        Arrow.transform.localPosition = new Vector3(0, 0, 0);
        ArrowPool.Enqueue(Arrow);
        }
    }


}
