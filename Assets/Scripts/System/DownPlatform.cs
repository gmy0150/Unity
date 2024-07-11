using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DownPlatform : MonoBehaviour
{
    PlatformEffector2D platformobj;

    void Start()
    {
        platformobj = GetComponent<PlatformEffector2D>();
        
    }

    
    public void ChangeLayer(){
        platformobj.rotationalOffset = 180f;
        StartCoroutine(ReturnLayer());
    }
    IEnumerator ReturnLayer(){
        yield return new WaitForSeconds(0.5f);
        platformobj.rotationalOffset = 0f;
    }
}
