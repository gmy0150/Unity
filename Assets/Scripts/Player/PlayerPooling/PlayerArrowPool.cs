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
    bullet bullet;
    private void Awake() {
        Instance = this;
        InitializePool();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<TPlayer>();
        bullet = ArrowPrefeb.GetComponent<bullet>();
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
        }else{
            Arrow.transform.rotation = Quaternion.Euler(0,180,0);
        }
        Arrow.SetActive(true);
        Arrow.transform.SetParent(null);

        return Arrow;
    }
    private void Update() {
    }
    public void ReturnArrow(GameObject Arrow) {
            Debug.Log(00);
            Arrow.SetActive(false);
            Arrow.transform.SetParent(transform);
            Arrow.transform.localPosition = new Vector3(0, 0, 0);
            ArrowPool.Enqueue(Arrow);
        
    }


}
