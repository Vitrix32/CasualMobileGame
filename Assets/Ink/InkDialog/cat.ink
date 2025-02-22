=== cat ===

I am a cat. meow. I think jeff's quest is at state {jeffQuestStep}
{jeffQuestStep==0: 
    * [Please let Jeff pet your belly]
    Finneee
    ~AdvanceQuest(jeffQuestId)
    ->END
    - else : 
    -> catDefault
}




=== catDefault ===

    * [You're too cute I forgot why im here]
        dumb
        ->DONE