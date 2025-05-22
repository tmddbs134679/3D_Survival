using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uicondition;

    Condition health {  get { return uicondition.health; } }
    Condition hunger { get { return uicondition.hunger; } }
    Condition stamina { get { return uicondition.stamina; } }

    public float noHungerHealthDecay;

    // Update is called once per frame
    void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if(hunger.curValue <= 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if(health.curValue < 0f)
        {
            Die();
        }
    }


    public void Die()
    {
        Debug.Log("Á×À½");
    }

    public void Heal(float hungerHealth)
    {
        health.Add(hungerHealth);
    }


    public void Eat(float hungerHealth)
    {
        hunger.Add(hungerHealth);
    }

 

}
