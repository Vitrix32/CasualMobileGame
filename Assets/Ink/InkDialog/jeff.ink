

VAR jeffQuestId="jeffQuest"
VAR jeffQuestStep=-1
VAR jeffQuestState="REQUIREMENTS_NOT_MET"

VAR expoQuestId="expoQuest"
VAR expoQuestStep=-1
VAR expoQuestState= "REQUIREMENTS_NOT_MET"

=== jeff ===
{ jeffQuestState :  ///THIS IS HOW TO DO THE MULTIPLE STEPS AND STUFFFFF!!
    - "REQUIREMENTS_NOT_MET": -> requirementsNotMet
    - "CAN_START" : -> canStart
    - "IN_PROGRESS" : -> inProgress
    - "CAN_FINISH" : ->canFinish
    - "FINISHED" : -> finished
    - else: -> jeffDefault
}
  
=requirementsNotMet
->END

= canStart
Oh hey Player! Haven't seen you since, well, yesterday! How're you doing?
    * [Just trying to find something to do today]
    ->petCat
        

=petCat
Nice nice, well for one, I really want to pet the cat. Could' you ask him for me?
* [Sure]
Thanks! I'll think of something else for you to do while y'all talk.
    ~ StartQuest("jeffQuest")
-> DONE


= inProgress
Are you gonna ask him? My hands are untouched by cat fur.
->END

= canFinish
Well, what did he say?
    *[He said you could pet him]
    AWESOME!! Thanks so much brother!
    ~ FinishQuest(jeffQuestId)
    ->END
    
= finished
->jeffDefault
->END

=== jeffDefault ===
Hey player, what's up? {expoQuestState} //put demo dialogue here
    * [What can I do around town?]
     ->doingAroundTown
    
=== doingAroundTown ===
{ expoQuestState : 
- "REQUIREMENTS_NOT_MET" : ->requirementsNotMet
- "CAN_START" : -> canStart
- "IN_PROGRESS" : -> inProgress
- else : ->END
}
= requirementsNotMet
    Nothing right now, sorry.
    ->DONE

= canStart
Let me see if I can remember, Well I heard the at the south west part of the town has some sort of pest problem, if you're looking for a bit of action
but if you want a slower warm up to your day I hear old miss Marlena has some errands that she could probably use a hand with.
    *[I'll go do some varmint clearing!]
    //add varmint quest
        Awesome, I'll see you on the other side brother!
        ->END
    *[I suppose I'll go help Marlena]
        That Sounds great! See ya around Brother!
        ~StartQuest(expoQuestId)
        ->END
->END

= inProgress
Don't you already have somewhere to be right now, brother?
->END

->END



