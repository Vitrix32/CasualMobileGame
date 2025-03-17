using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationPointScript : MonoBehaviour
{
    public GameObject manager;
    private QuestManager QM;

    private void Start()
    {
        QM = manager.GetComponent<QuestManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        bool r = QM.TryQuest(this.name);
        Debug.Log(r);
        if (r)
        {
            Destroy(this.gameObject);
        }
    }
}
