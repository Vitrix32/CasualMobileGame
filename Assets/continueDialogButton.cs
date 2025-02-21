using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class continueDialogButton : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject choices1;
    [SerializeField] private GameObject choices2;
    [SerializeField] private GameObject choices3;


    [SerializeField] private GameObject button;   

    // Update is called once per frame
    void Update()
    {
        if(!choices1.activeInHierarchy&& !choices2.activeInHierarchy&& !choices3.activeInHierarchy)
        {
            button.SetActive(true);
        }
        else
        {
            button.SetActive(false);   
        }
    }


}
