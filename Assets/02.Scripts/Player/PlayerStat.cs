using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [Header("Movement")]

    public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseMP(float value)
    {
        StartCoroutine(SpeedUp(value));
    }

    IEnumerator SpeedUp(float value)
    {
        float prevSpeed = moveSpeed;
        moveSpeed += value;


        yield return new WaitForSeconds(10f);

        moveSpeed = prevSpeed;
    }
}
