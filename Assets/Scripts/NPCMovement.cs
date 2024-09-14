using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NPCMovement : MonoBehaviour
{
    private int iterator;
    private int moveCount;
    public GameObject currentNode;
    public List<GameObject> patrolPath;
    private float moveSpeed;
    private float idleTime;
    public bool activeCoroutine;
    public bool idleing;
    public bool patrolling;
    public bool wandering;
    private Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        iterator = 0;
        moveCount = 1;
        currentNode = patrolPath[iterator];
        moveSpeed = 3;
        idleTime = 2;
        activeCoroutine = false;
        idleing = true;
        patrolling = false;
        wandering = false;
        originalPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (patrolling && !activeCoroutine)
        {
            activeCoroutine = true;
            StartCoroutine(Patrol());
        }
        else if (idleing && !activeCoroutine)
        {
            activeCoroutine = true;
            StartCoroutine(Idle(idleTime));
        }
        else if (wandering && !activeCoroutine)
        {
            activeCoroutine = true;
            StartCoroutine(Wander());
        }
    }

    IEnumerator Patrol()
    {
        while (moveCount > 0)
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
            if (iterator == patrolPath.Count - 1)
            {
                iterator = 0;
                moveCount--;
                currentNode = patrolPath[iterator];
            }
            else
            {
                iterator++;
                moveCount--;
                currentNode = patrolPath[iterator];
            }
        }
        moveCount = 1;
        idleing = true;
        patrolling = false;
        activeCoroutine = false;
    }

    IEnumerator Wander()
    {
        int index = Random.Range(0, patrolPath.Count);
        while (patrolPath[index] == currentNode)
        {
            index = Random.Range(0, patrolPath.Count);
        }
        currentNode = patrolPath[index];
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
        idleing = true;
        wandering = false;
        activeCoroutine = false;
    }

    IEnumerator Idle(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        idleing = false;
        //patrolling = true;
        wandering = true;
        activeCoroutine = false;
    }
}
