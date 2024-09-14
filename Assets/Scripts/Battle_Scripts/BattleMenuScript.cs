using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class BattleMenu : MonoBehaviour
{
    public void Attack()
    {
        SceneManager.LoadScene(2);
    }
    public void Magic()
    {
        SceneManager.LoadScene(3);
    }

    public void Items()
    {
        SceneManager.LoadScene(4);
    }
}