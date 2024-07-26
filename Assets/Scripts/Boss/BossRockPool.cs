using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRockPool : MonoBehaviour
{
    [SerializeField]private GameObject BossRockPrefab;
    [SerializeField]private GameObject BossBrick;

    private Queue<GameObject> availableObjects = new Queue<GameObject>();
    private Queue<GameObject> availObjects = new Queue<GameObject>();
    public static BossRockPool Instance{get;private set;}

    private void Awake() {
        Instance = this;
        GrowPool();
    }
    private void Update() {
        // BossRockPrefab.
    }
    private void GrowPool(){
        for(int i =0; i<10; i++){
            var instanceToAdd = Instantiate(BossRockPrefab);
            instanceToAdd.transform.localScale = new Vector3(1f ,1f,1f);
            instanceToAdd.transform.SetParent(transform);
            AddToPool(instanceToAdd);
        }
    }
    public void AddToPool(GameObject instance){
        instance.SetActive(false);
        availableObjects.Enqueue(instance);
    }
    public GameObject GetFromPool(){
        if(availableObjects.Count == 0){
            GrowPool();
        }
        var instance = availableObjects.Dequeue();
        instance.transform.localScale = new Vector3(1f ,1f,1f);
        instance.SetActive(true);
        return instance;
    }
    private void BrickPool(){
        for(int i =0; i<10; i++){
            var instanceToAdd = Instantiate(BossBrick);
            instanceToAdd.transform.localScale = new Vector3(1f ,1f,1f);
            instanceToAdd.transform.SetParent(transform);
            AddBrickToPool(instanceToAdd);
        }
    }
    public void AddBrickToPool(GameObject instance){
        instance.SetActive(false);
        availObjects.Enqueue(instance);
    }
    public GameObject GetBrickFromPool(){
        if(availObjects.Count == 0){
            BrickPool();
        }
        var instance = availObjects.Dequeue();
        instance.transform.localScale = new Vector3(1f ,1f,1f);
        instance.SetActive(true);
        return instance;
    }
}
