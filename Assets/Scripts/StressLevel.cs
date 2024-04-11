using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StressLevel : MonoBehaviour
{
    public Slider stress;
   
    private int maxHealth = 100;
    public static int mentalHealth = 80;

    private void Awake()
    {
        //set the stress slider's max value to the maxHealth
        stress.maxValue = maxHealth;
        //Set the mentalHealth variable from the Ink file to your mentalHealth static variable
        //that will save itslef when you enter another scene
        Ink.Runtime.Object obj = new Ink.Runtime.IntValue(mentalHealth);
        DialogueManager.GetInstance().SetVariableState("mentalHealth", obj);
    }

    private void Update()
    {
        //Get the mentalHealth variable from the Ink file and put it in an int variable
        mentalHealth = ((Ink.Runtime.IntValue)DialogueManager.GetInstance().GetVariableState("mentalHealth")).value;

        //Check if the value above is greater than the max value of the stress slider
        if (mentalHealth > maxHealth)
        {
            //If it is set the mentalHealth variable to stress slider max value
            Ink.Runtime.Object obj = new Ink.Runtime.IntValue(maxHealth);
            DialogueManager.GetInstance().SetVariableState("mentalHealth", obj);
            mentalHealth = maxHealth;
        }
        stress.value = mentalHealth;
    }
}
