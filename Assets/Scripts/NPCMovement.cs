using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/*
 * Name: NPCMovement
 * Author: Isaac Drury
 * Date: November 2024
 * Description:
 * This file contains logic for NPCMovement. There are two main behaviours described:
 * Idle and Patrol. In patrol, an npc will travel from node to node along a path and
 * will enter idle for a set duration of time before moving to the next node. In idle,
 * an npc will simply remain idle for a set duration of time. This script holds the
 * logic of the two npc behaviours, but more npc-specifc code will be found in other
 * scripts and will reference the methods in this script.
 */
public class NPCMovement : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> patrolPath; //Will probably just add multiple lists for npcs with multiple patrol paths.
    [SerializeField]
    private int moveCount; //Use -1 to specify no move limit
    [SerializeField]
    private int lapCount; //Use -1 to specify infinite laps
    [SerializeField]
    private float idleDuration;
    [SerializeField]
    private float moveSpeed;

    private int iterator;
    private int movesLeft;
    private int lapsLeft;

    private GameObject currentNode;
    private Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        iterator = 0;
        movesLeft = moveCount;
        lapsLeft = lapCount;
        currentNode = null;
        originalPosition = this.transform.position;
    }

    //The patrol behaviour. Can be configured using the serialized fields above.
    IEnumerator Patrol()
    {
        currentNode = patrolPath[iterator];
        while (lapsLeft != 0)
        {
                originalPosition = this.transform.position;
                float distance = Vector3.Distance(originalPosition, currentNode.transform.position);
                float duration = distance / moveSpeed;
                float time = 0f;

                while (time < duration)
                {
                    time += Time.deltaTime;
                    this.transform.position = Vector2.Lerp(originalPosition, currentNode.transform.position, time / duration);
                    yield return null;
                }
                if (movesLeft == 0)
                {
                    yield return new WaitForSeconds(idleDuration);
                    movesLeft = moveCount;
                }
                if (iterator == patrolPath.Count - 1)
                {
                    iterator = 0;
                    lapsLeft--;
                    movesLeft--;
                    currentNode = patrolPath[iterator];
                }
                else
                {
                    iterator++;
                    movesLeft--;
                    currentNode = patrolPath[iterator];
                }
        }
        yield return null;
    }

    IEnumerator RandomMove()
    {
        int rand = Random.Range(0, patrolPath.Count);
        currentNode = patrolPath[rand];
        originalPosition = this.transform.position;
        float distance = Vector3.Distance(originalPosition, currentNode.transform.position);
        float duration = distance / moveSpeed;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            this.transform.position = Vector2.Lerp(originalPosition, currentNode.transform.position, time / duration);
            yield return null;
        }
    }

    public void startPatrol()
    {
        StopAllCoroutines();
        StartCoroutine("Patrol");
    }

    public void StartRandomMove()
    {
        StopAllCoroutines();
        StartCoroutine("RandomMove");
    }
}
