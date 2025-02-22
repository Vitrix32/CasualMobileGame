

VAR jeffQuestId="jeffQuest"
VAR jeffQuestStep=-1
VAR jeffQuestState="REQUIREMENTS_NOT_MET"


=== jeff ===
{ jeffQuestState :
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
My name jeff. I want to pet the cat that is standing next to me. 
    * [Sure]
        Jeff: Thanks brother
        ~ StartQuest("jeffQuest")
        -> DONE
    * [Seems like its not a cat petting day.]
        -> jj

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

= jeffDefault
Hey player, what's up? //put demo dialogue here
    * What can I do around town?
    -> jj 
    === jj ===
    some stuff that Michael will implement later
    ->END



