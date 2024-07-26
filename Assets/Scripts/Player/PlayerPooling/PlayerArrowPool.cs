using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerArrowPool : MonoBehaviour
{
    public static PlayerArrowPool Instance { get; private set; }
    public GameObject ArrowPrefeb;
    private Queue<GameObject> ArrowPool = new Queue<GameObject>();
    bullet bullets;
    public int poolSize = 20;
    float timer;
    private void Awake() {
        Instance = this;
        InitializePool();
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
            Debug.Log("왜 안생겨");
            InitializePool();
        }
        GameObject Arrow = ArrowPool.Dequeue();
        Debug.Log(ArrowPool.Count);
        Arrow.SetActive(true);
        Arrow.transform.SetParent(null);

        return Arrow;
    }
    private void Update() {
        timer += Time.deltaTime;
    }
    public void ReturnArrow(GameObject Arrow,float seconds = 0) {
        if(seconds < timer){
            Arrow.SetActive(false);
            Arrow.transform.localPosition = new Vector3(0,0,0);
            Arrow.transform.SetParent(transform);
            ArrowPool.Enqueue(Arrow);   
        }
    }
}
