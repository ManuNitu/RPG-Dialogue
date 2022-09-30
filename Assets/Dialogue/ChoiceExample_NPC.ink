INCLUDE Global.ink
VAR choice = 0 
{npcChoice == 0:-> main |-> talked}
=== main ===
Hi this is an example for the morallity system. #speaker: NPC #portrait: NPC_Neutral #layout:right
How are you feeling?
... #speaker: Kevin #portrait: Kevin_Neutral  #layout:left
    *[Sad]
       ~ mentalHealth -= 20
       ~ npcChoice = 1
       I'm not feeling so well. #speaker: Kevin #portrait: Kevin_Sad  #layout:left
       I'm sorry... #speaker: NPC #portrait: NPC_Sad #layout:right
       ->DONE
    *[Neutral]
       ~ mentalHealth -= 0
       ~ npcChoice = 2
       I'm ok. #speaker: Kevin #portrait: Kevin_Neutral  #layout:left
       Yeah, I'm ok too. #speaker: NPC #portrait: NPC_Neutral #layout:right
       ->DONE
    *[Happy]
       ~ mentalHealth += 20
       ~ npcChoice = 3
       I'm very happy! #speaker: Kevin #portrait: Kevin_Happy  #layout:left
       Glad to hear it! #speaker: NPC #portrait: NPC_Happy #layout:right
       ->DONE
->END
=== talked ===
{ npcChoice:
-1:I'm sorry you feel sad. #speaker: NPC #portrait: NPC_Sad #layout:right
   But I'm sure you'll get better. #portrait: NPC_Happy
   Hope you're right. #speaker: Kevin #portrait: Kevin_Sad  #layout:left
-2:Ahh...hello?  #speaker: NPC #portrait: NPC_Neutral #layout:right
   Hi. #speaker: Kevin #portrait: Kevin_Neutral  #layout:left
-3:Wow you stil look very excited. #speaker: NPC #portrait: NPC_Happy #layout:right
  Thanks. #speaker: Kevin #portrait: Kevin_Happy  #layout:left
}
->END