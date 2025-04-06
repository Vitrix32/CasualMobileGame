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
                str = "In combat, your party fights a single enemy. Both receive an action per round.";
                break;
            case 1:
                str = "There are three categories of actions your party can take:\n-Attacks\n-Spells\n-Flee";
                break;
            case 2:
                str = "Some actions apply status effects. These are displayed above the party and enemy healthbars and can be tapped for more information.";
                break;
            case 3:
                str = "There are four attacks you can choose. Each attack corresponds to a party member.";
                break;
            case 4:
                str = "Slash- Deals standard damage to the enemy.\nShadow Scratch- Deals half damage to the enemy and applies the poison effect.";
                break;
            case 5:
                str = "Heavy Axe- Deals double damage to the enemy.\nSharp Shot- Deals half damage to the enemy and applies the weakeness effect.";
                break;
            case 6:
                str = "There are four spells you can choose. Most are defensive in their nature.";
                break;
            case 7:
                str = "Inspire- Heals the party for a small amount and applies the empower effect to the party.\nHealing Winds- Heals the party for a large amount.";
                break;
            case 8:
                str = "Shield Party- Applies the shield effect to the party.\nTime rift- Deals half damage and skips the enemy's turn.";
                break;
            case 9:
                str = "The final action you can take is flee. Fleeing will allow the party to escape combat and return to their quest.";
                break;
            case 10:
                str = "Fleeing is not guarenteed to succeed. Failing will damage the party and forfeit their action for the round. Fleeing should be a last resort";
                break;
            case 11:
                str = "Combat will end upon a successful flee, the death of the enemy, or the death of the party.";
                break;
        }
        textBox.text = str;

        // Update button transparency
        nextColor = nextImg.color;
        prevColor = prevImg.color;

        if (index == 11)
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
        if (index < 11)
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
