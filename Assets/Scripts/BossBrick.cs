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
            // Destroy(gameObject);
            BossRockPool.Instance.AddBrickToPool(gameObject);

        }
        if(other.tag == "Player"){
            // player.takeDamage(10);
            player.React();
            
            BossRockPool.Instance.AddBrickToPool(gameObject);
        }
    }

}
