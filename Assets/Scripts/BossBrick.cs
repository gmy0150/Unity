using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBrick : MonoBehaviour
{
    TPlayer player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<TPlayer>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Boss"){
            Destroy(gameObject);
        }
        if(other.tag == "Player"){
            player.getDamage(10);
            Destroy(gameObject);
        }
    }
}
