using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPos;
    public int gridX, gridY;

    public int gCost;
    public int hCost;
    public Node parent;

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPos = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {
        get 
        {
            return HeapIndex;
        }
        set
        {
            HeapIndex = value;
        }
    }

    public int CompareTo(Node n)
    {
        int compare = fCost.CompareTo(n.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(n.hCost);
        }
        return -compare;
    }
}
