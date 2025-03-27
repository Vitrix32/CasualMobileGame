using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class environmentBackground : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] 
    private Image image;
    [SerializeField]
    private Sprite[] backgrounds;
    void Start()
    {
        int loc = PlayerPrefs.GetInt("LocID");
        Debug.Log("location ID: " + loc);
        Sprite locSprite = backgrounds[0];

        switch(loc)
        {
            case 0:
                locSprite = getSpriteByName("trees");
                break;
            case 1:
                locSprite = getSpriteByName("cave");
                break;
            case 2:
                locSprite = getSpriteByName("dungeon");
                break;
            case 3:
                locSprite = getSpriteByName("desert");
                break;
            case 4:
                locSprite = getSpriteByName("church");
                break;
            case 5:
                locSprite = getSpriteByName("snow");
                break;
        }

        image.sprite = locSprite;

    }

    private Sprite getSpriteByName(string name)
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (backgrounds[i].name == name)
                return backgrounds[i];
        }
        return null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
