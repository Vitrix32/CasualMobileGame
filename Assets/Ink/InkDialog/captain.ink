===captain===

{expoQuestState=="CAN_FINISH":
    Oh howdy Player! Have you seen those guards at the gate lately? They don't know how easy they have it here, you know back in my day... huh? Did you need something?
    *[No not really]
    Oh, then get out my sunlight, eh
    ->END
    *[What's the deal with the lake in the forest?]
    ->asktheCaptainAboutLake
  - else:
    Hmmmmm...
}

=asktheCaptainAboutLake
Well that's a long story that I don't wanna make up right now oopsie poopsie.
~FinishQuest(expoQuestId)
->END