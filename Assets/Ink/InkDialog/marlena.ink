===marlena===

{expoQuestStep:
    - 1 : 
    Oh hello Player! It s so good to see you! Are you staying warm today?
    * [Jeff said you needed some help]
    -> takeJacklunch
    - 3 :
    Oh hello Player! Did you give Jackie his lunch?
    * [Yes I did he ate it in five seconds]
    -> gaveJackLunch
    - 7 :
    Oh hello Player! Did you find that cat that scampered off?
    * [Yea... What's the deal with the lake over there?]
    -> askAboutLake
    
    - else : -> defaultMarlena
}


= takeJacklunch
Oh that s so sweet of you youngster! I do have a few things I needed to do but if you want to help I ll make sure to give you some of my nice and fresh cookies when you get back!
I packed lunch for little Jacky but he must have forgotten it on his way east to the forest. Could you take it to him?
    *[Yes I can]
    Thanks so much sweetie!
    ~ AdvanceQuest(expoQuestId)
    ->END

= gaveJackLunch

Oh thanks for your help Player! You re so kind as to lend an old woman a hand! Here, have some cookies!
Oh I saw the little kitty running off up west, probably to go get a glass of milk from the inn. You should go get him.
~ AdvanceQuest(expoQuestId)

->END

= askAboutLake
Oh, that's a great question. Many things happened in this town long ago, when I was just a girl. 
My husband remembers it a little better, you should go ask him. He's at home to the south a bit.
~AdvanceQuest(expoQuestId)
->END


=defaultMarlena
Hey sweetie, be sure to eat your vegetables! //try out randomly generated
->END




