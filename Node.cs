using UnityEngine;
using System.Collections;

public class Node : IHeapItem<Node>{
    public bool walkable;
    public Vector3 worldPosition;
    public int gcost;
    public int hcost;
    public Node parent;
    public int gridX;
    public int gridY;
    int heapIndex;

    public Node(bool walkable, Vector3 worldPosition,int gridX,int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int fcost
    {
        get
        {
            return gcost + hcost;
        }
    }
    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node node)
    {
        int c = this.fcost.CompareTo(node.fcost);

        if (c == 0)
        {
            c = this.hcost.CompareTo(node.hcost);
        }

        return -c;
    }
}
