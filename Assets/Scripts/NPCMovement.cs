using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NPCMovement : MonoBehaviour
{
    public List<GameObject> patrolPath;
    private int iterator;
    public GameObject currentNode;
    private float moveSpeed;
    public bool Patrolling;
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 3;
        Patrolling = true;
        iterator = 0;
        currentNode = patrolPath[iterator];
    }

    // Update is called once per frame
    void Update()
    {
        if (Patrolling)
        {
            if (Vector3.Distance(this.transform.position, currentNode.transform.position) < 0.001f)
            {
                if (iterator == patrolPath.Count - 1)
                {
                    iterator = 0;
                    currentNode = patrolPath[iterator];
                }
                else
                {
                    iterator++;
                    currentNode = patrolPath[iterator];
                }
            }
            else
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, currentNode.transform.position, moveSpeed * Time.deltaTime);
            }
        }
    }
}
