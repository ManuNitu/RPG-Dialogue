using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayCoins : MonoBehaviour
{
    public TextMeshProUGUI coinsText;

    public static int coins = 25;

    void Start()
    {
        //Set the coins variable from the Ink file to your coins static variable that will
        //save itslef when you enter another scene
        Ink.Runtime.Object obj = new Ink.Runtime.IntValue(coins);
        DialogueManager.GetInstance().SetVariableState("coins", obj);
    }

    private void Update()
    {
        //Get the coins variable from the Ink file and put it the coinsText to show it on screen
        coins = ((Ink.Runtime.IntValue)DialogueManager.GetInstance().GetVariableState("coins")).value;
        coinsText.text = coins.ToString();
    }
}
