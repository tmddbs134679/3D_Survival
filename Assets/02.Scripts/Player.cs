using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;
    public ItemData data;
    public Action addItem;



    public Transform dropPos;
    private void Awake()
    {
        CharacterManager.Inst.Player = this;
        controller = GetComponent<PlayerController>();  
        condition = GetComponent<PlayerCondition>();
    }


}
