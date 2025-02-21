using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bubbleManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject activeIcon;

    void Start()
    {
        activeIcon.SetActive(false);
    }

    // Update is called once per frame
    

    public void show()
    {
        activeIcon?.SetActive(true);
        //canFinishIcon?.SetActive(true);//test
    }
    public void hide()
    {
        activeIcon?.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            Debug.Log("wanna quest?");

            show();
        }
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {

            hide();
        }
    }

}
