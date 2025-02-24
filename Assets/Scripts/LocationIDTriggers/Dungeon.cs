using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    /*
        * LOCATION ID - KEY
        * Fill in as needed, if we need to have unique combat encounters. 
        * Change the location ID on trigger events based on where the player is
        * For example, change to desert when you walk into new biome, and back
        *          when you come back to Graville
        * 
        * Graville         - 0
        * Desert (South )  - 1
        * Dungeon          - 2
        * Dungeon Boss     - 3
        * 
        * 
    */
    public void EnterDungeon()
    {
        PlayerPrefs.SetInt("LocID", 2);
    }

    public void ExitDungeon()
    {
        PlayerPrefs.SetInt("LocID", 0);
    }
}
