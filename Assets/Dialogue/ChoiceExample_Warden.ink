INCLUDE Global.ink
{talkedToWarden == "": ->main |-> talked}

=== main ===
What do you want? #speaker: Warden #portrait: Warden_Neutral #layout:right
Speak!
... #speaker: Kevin #portrait: Kevin_Neutral  #layout:left
    *[Annoy Him]
       ~mentalHealth -= 20
       ~talkedToWarden = "Yes"
       Well I don't know, what do you want? #speaker: Kevin #portrait: Kevin_Smug  #layout:left
       Yeah, very funny. #speaker: Warden #portrait: Warden_Angry #layout:right
       Come here you imbecile!
       ->DONE
    *[Step Away]
       Nothing... #speaker: Kevin #portrait: Kevin_Sad  #layout:left
       Then move away.#speaker: Warden  #portrait: Warden_Neutral #layout:right
       ->DONE
->END

=== talked ===
Get lost! #speaker: Warden #portrait: Warden_Angry #layout:right}
->END
