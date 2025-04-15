using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Help : MonoBehaviour
{
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private UnityEngine.UI.Image nextImg;
    [SerializeField] private UnityEngine.UI.Image prevImg;

    private Color nextColor;
    private Color prevColor;

    private int index;
    private string str;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        str = "";
        TextChange();
    }

    public void TextChange()
    {
        // Change displayed text
        switch (index) 
        { 
            case 0:
                str = "In combat, your party fights a single enemy. Both receive one action per round.";
                break;
            case 1:
                str = "There are three categories of ACTIONS your party can take:\n-ATTACKS\n-SPELLS\n-FLEE";
                break;
            case 2:
                str = "Some actions apply STATUS EFFECTS. These are displayed above the party and enemy healthbars. TAP THEM for more information.";
                break;
            case 3:
                str = "Most actions have a COOLDOWN that is displayed with the action. During this cooldown, the action cannot be used.";
                break;
            case 4:
                str = "In general, actions that deal DAMAGE have a small VARIATION in the damage they deal.";
                break;
            case 5:
                str = "There are FOUR ATTACKS you can choose. Each attack corresponds to a party member.";
                break;
            case 6:
                str = "SLASH- Deals STANDARD damage to the enemy.\nSHADOW SCRATCH- Deals HALF damage to the enemy and applies the POISON effect.";
                break;
            case 7:
                str = "HEAVY AXE- Deals DOUBLE damage to the enemy.\nSHARP SHOT- Deals HALF damage to the enemy and applies the WEAKNESS effect.";
                break;
            case 8:
                str = "There are FOUR SPELLS you can choose. Most are defensive in their nature.";
                break;
            case 9:
                str = "INSPIRE- HEALS the party for a SMALL amount and applies the EMPOWER effect to the party.\nHEALING WINDS- HEALS the party for a LARGE amount.";
                break;
            case 10:
                str = "SHIELD PARTY- Applies the SHIELD effect to the party.\nTIME RIFT- Deals HALF damage and SKIPS the enemy's turn.";
                break;
            case 11:
                str = "The final action you can take is FLEE. Fleeing will allow the party to ESCAPE combat and return to their quest.";
                break;
            case 12:
                str = "Fleeing can FAIL. A failed flee attempt will DAMAGE the party and FORFEIT their action for the round. Fleeing should be a LAST RESORT.";
                break;
            case 13:
                str = "Combat will end upon a successful flee, the death of the enemy, or the death of the party.";
                break;
        }
        textBox.text = str;

        // Update button transparency
        nextColor = nextImg.color;
        prevColor = prevImg.color;

        if (index == 13)
        {
            nextColor.a = 0.4f;
            nextImg.color = nextColor;
        }
        else
        {
            nextColor.a = 1.0f;
            nextImg.color = nextColor;
        }
        if (index == 0)
        {
            prevColor.a = 0.4f;
            prevImg.color = prevColor;
        }
        else
        {
            prevColor.a = 1.0f;
            prevImg.color = prevColor;
        }
    }

    public void IncrementIndex()
    {
        if (index < 13)
        {
            index++;
        }
    }

    public void DecrementIndex()
    {
        if (index > 0)
        {
            index--;
        }
    }

    public void ResetIndex()
    {
        index = 0;
    }
}
