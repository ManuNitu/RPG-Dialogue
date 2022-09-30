INCLUDE Global.ink

{itemToBuy == "": -> main |-> buy}

===main===
Please take a look at my fine selection of potions. #speaker: NPC #portrait: NPC_Neutral #layout: right

->END
===buy===
Hi, you want to buy a {itemToBuy} for {itemToBuyCost}? #speaker: NPC #portrait: NPC_Neutral #layout: right
... #speaker: Kevin #portrait: Kevin_Neutral #layout: left
     *[Yes ({itemToBuyCost} coins)]
       Sure. #speaker: Kevin #portrait: Kevin_Neutral #layout: left
       {coins >= itemToBuyCost:
        ~ coins -= itemToBuyCost
        ~ itemPicked = itemToBuy
        Here you go. #speaker: NPC #portrait: NPC_Happy #layout: right
        You bought a {itemPicked} for {itemToBuyCost} coins. #layout: item
        Thanks. #speaker: Kevin #portrait: Kevin_Neutral #layout: left
        - else: Looks like you don't have enough money to buy a {itemToBuy}. #speaker: NPC #portrait: NPC_Sad #layout: right
        Please come back when you get more coins.
        Ok, sorry. #speaker: Kevin #portrait: Kevin_Sad #layout: left 
       }
       ->DONE
     *[No]
       No thank you. #speaker: Kevin #portrait: Kevin_Neutral #layout: left
       Ok, let me know if you want to buy anything. #speaker: NPC #portrait: NPC_Neutral #layout: right
       ->DONE
->END


