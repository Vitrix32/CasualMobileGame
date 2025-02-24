===jack===

{expoQuestStep :
    - 2 :
        Oh hey there fella, what brings you to my neck of the woods?
        *[Marlena packed you a lunch]
        Aw man I forgot it again?! I hope Marlena doesn t think I don t like her food, I m always just so pumped up to come to the forest again I rush out the door.
        Well if you wouldn t mind handing that over, I am quite hungry!
        ~AdvanceQuest(expoQuestId)
        -> END
    - 5 :
        Player: Hey Jack, have you seen that cat around here?
        `Jack: ...zzzzZZZzzz `Player: Oh he s asleep, do you think we should just wait or ?
        `Samantha: HEY LUMBER HEAD!
        `Jack: Huh? Oh hey guys how s it going?`Player: Lunch hit you that hard?
        `Jack: Oh I m guessing I fell asleep? Yeah, Marlena s cooking is just so good, and chopping trees is hard work you know? I figure a nap can t hurt, and it s not like I m actually IN the forest, that s where it starts to get dangerous.
        `Samantha: Well anyways, we re looking for that cat, have you seen him?
        `Jack: Not I don t think so-Magic Cat: Oh hey, you people are here, perfect! I need help getting through the forest!
        `Player: Huh, what are you going in there for?
        `Magic Cat: I need to get to the lake. It s magic dontcha know? Come on, we don t have much time!
        `Samantha: I wish this cat was this interesting when I was younger!
        `Jack: Woah guys! If you re going in there you need to be careful, there s dangerous creatures in there, unlike ones you ve seen elsewhere. You re gonna need to be able to protect yourself just in case, but I m going to come with you as well. Here, Player, I ve got a spare axe you can use. Sam I think I might have another one-
        `Samantha: No thanks, I think I ll manage with this here slingshot.
        `Jack: Are you a good shot with that?`Samantha: Better than you are at throwing that axe.
        `Jack: Well it s not a throwing axe it s a tree felling axe, and then there s your log splitting axe, your little hatchets for-
        `Magic Cat: Are you all done? We gotta go!
        `Jack: Ok sure! We can talk about the different types of axes later!
        `Samantha: Sure `Player: Jack, do you know the way?
        `Jack: Um  no. I mean, it s dangerous in there, and there s perfectly good trees right here.
        `Player: Ok, I guess I ll lead the way, but I don t know which way I m going so I make no promises to the speed of our arrival.
        ~AdvanceQuest(expoQuestId)
        -> END
    
    else : 
    ->defaultJack
}

===defaultJack===
I chop down trees. 
->END

