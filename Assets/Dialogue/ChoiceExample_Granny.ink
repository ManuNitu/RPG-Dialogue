INCLUDE Global.ink
VAR QuestReward = "Green Potion"
VAR ItemWanted = "Yellow Potion"
{grannyTalked == 0:-> notTalked |-> Talked}

=== notTalked ===
Hey kid, you want to earn a rare item? #speaker: Granny #portrait: Granny_Neutral #layout:right
~grannyTalked = 1
->Dialog

=== Talked ===
{grannyQuest:
  - 0: Hello again.  #speaker: Granny #portrait: Granny_Neutral #layout: right
  So you thought about my offer?  #speaker: Granny #portrait: Granny_Neutral #layout: right
  - 1:-> Quest
  - 2:-> QuestComplete
}
->Dialog

=== Dialog ===
... #speaker: Kevin #portrait: Kevin_Neutral  #layout:left
    *[Oh Yeah]
       ~grannyQuestItemNeeded = ItemWanted
       ~grannyQuest = 1
       Hell yeah. #speaker: Kevin #portrait: Kevin_Happy  #layout:left
       Great, you just need to find a Yellow Potion for me. #speaker: Granny #portrait: Granny_Happy #layout:right
       Alright, I'm on it. #speaker: Kevin #portrait: Kevin_Happy  #layout:left
       ->DONE
    *[Nope]
       Yeah... no thank you. #speaker: Kevin #portrait: Kevin_Neutral  #layout:left
       It's ok I understand. #speaker: Granny #portrait: Granny_Neutral #layout:right
       But... if you ever change your mind I will be here. #speaker: Granny #portrait: Granny_Happy #layout:right
       ->DONE
->END

=== Quest ===
You got the Yellow Potion? #speaker: Granny #portrait: Granny_Neutral #layout:right
... #speaker: Kevin #portrait: Kevin_Neutral  #layout:left
    *[Here]
       Here it is. #speaker: Kevin #portrait: Kevin_Happy  #layout:left 
       {grannyHaveQuestItem == 1: 
       ~grannyQuest = 2
       Great! #speaker: Granny #portrait: Granny_Happy #layout:right #removeItem: Yellow Potion
       Thank you so much.
       No problem. #speaker: Kevin #portrait: Kevin_Happy  #layout:left
       Now where is my rare item? 
       Oh right, here. #speaker: Granny #portrait: Granny_Neutral #layout:right #giveItem: Green Potion
       You recevied a {QuestReward} from Granny #layout: item
       Sweet, thank you. #speaker: Kevin #portrait: Kevin_Happy  #layout:left
       - else: 
       ~mentalHealth -= 20
       Nice try but you don't got any. #speaker: Granny #portrait: Granny_Neutral #layout:right
       Come back when you have a Yellow Potion. 
       }
       ->DONE
    *[I don't]
      Still looking for it.  #speaker: Kevin #portrait: Kevin_Neutral  #layout:left
      Alright I will wait. #speaker: Granny #portrait: Granny_Neutral #layout:right
      Come back when you have a Yellow Potion.
      ->DONE
->END

=== QuestComplete ===
Thanks again for the {ItemWanted} kid. #speaker: Granny #portrait: Granny_Happy #layout:right
You're welcome.  #speaker: Kevin #portrait: Kevin_Happy  #layout:left
->END