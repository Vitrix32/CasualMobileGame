using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class AttackMenuScript : MonoBehaviour
{
    public void back()
    {
        SceneManager.LoadScene(1);
    }
}