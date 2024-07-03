using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrowPool : MonoBehaviour
{
    public static PlayerArrowPool Instance { get; private set; }
    public GameObject ArrowPrefeb;
    private Queue<GameObject> ArrowPool = new Queue<GameObject>();
    public int poolSize = 20;

    private void Awake() {
        Instance = this;
        InitializePool();
    }

    private void InitializePool() {
        for (int i = 0; i < poolSize; i++) {
            GameObject Arrow = Instantiate(ArrowPrefeb);
            Arrow.transform.SetParent(transform);
            Arrow.SetActive(false);
            ArrowPool.Enqueue(Arrow);
        }
        Debug.Log("확인");
    }
    public GameObject GetArrow() {
        if (ArrowPool.Count > 0) {
            GameObject Arrow = ArrowPool.Dequeue();
            Arrow.SetActive(true);
        Debug.Log("확인");
            return Arrow;
        } else {
            GameObject Arrow = Instantiate(ArrowPrefeb);
            Arrow.SetActive(true);
        Debug.Log("확인");
            return Arrow;
        }
    }
    public void ReturnArrow(GameObject Arrow) {
        Arrow.SetActive(false);
        ArrowPool.Enqueue(Arrow);
    }
}
