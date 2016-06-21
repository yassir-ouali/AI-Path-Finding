using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

    public LayerMask mask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;
    public List<Node> path;
    public List<Node> neightbor;
    float nodeDiameter;
    int gridSizeX, gridSizeY;
    public int maxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }
    public GameObject target;
    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        grid = new Node[gridSizeX, gridSizeY];
    }

    void Update()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
       // Vector3 bottomLeftNode = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
 
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
              //  Vector3 currentNodePosition = bottomLeftNode + new Vector3(i * nodeDiameter + nodeRadius, 0, j * nodeDiameter + nodeRadius);

                Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.forward * (j * nodeDiameter + nodeRadius);
                bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, mask);
                grid[i, j] = new Node(walkable, worldPoint, i, j);
            }
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null)
        {
            foreach (Node g in grid)
            {
                Gizmos.color = g.walkable ? Color.blue : Color.red;
                Node player = wolrdToGridPosition(target.transform.position);
                if (g.worldPosition == player.worldPosition)
                {
                    Gizmos.color = Color.green;
                }

                if(path!=null)
                {
                    if (path.Contains(g))
                    {
                        Gizmos.color = Color.white;
                    }
                }

                if(neightbor!=null)
                {
                    if(neightbor.Contains(g))
                    {
                        Gizmos.color = Color.magenta;
                    }
                }
                Gizmos.DrawCube(g.worldPosition, Vector3.one * (nodeDiameter-.1f));
            }
        }
    }

    public List<Node> getNeighbors(Node currentNode)
    {
        List<Node> neighbors = new List<Node>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }

                int checkX = currentNode.gridX + i;
                int checkY = currentNode.gridY + j;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        
        return neighbors;
    }

    public Node wolrdToGridPosition(Vector3 wdP)
    {
        float percentX = (wdP.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (wdP.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
 
        int x=Mathf.RoundToInt(percentX * (gridSizeX-1));
        int y=Mathf.RoundToInt(percentY * (gridSizeY-1));

        return grid[x,y];
    }
}
