using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class pathFinding : MonoBehaviour {

    Grid grid;
    public Transform start;
    public Transform target;

    void Awake()
    {
        grid=GetComponent<Grid>();
    }
    void Update()
    {
        FindPath(start.position,target.position);
        
    }

    void FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        Node startNode = grid.wolrdToGridPosition(startPosition);
        Node targetNode = grid.wolrdToGridPosition(targetPosition);
        grid.neightbor = grid.getNeighbors(startNode);

        Heap<Node> openSet = new Heap<Node>(grid.maxSize);
        List<Node> closedSet = new List<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode=openSet.RemoveFirst();
            /*Node currentNode = openSet[0];
            //trouver le current node
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fcost < currentNode.fcost || openSet[i].fcost == currentNode.fcost && openSet[i].hcost < currentNode.hcost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);*/
            closedSet.Add(currentNode);

            if(currentNode==targetNode){
                retracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbor in grid.getNeighbors(currentNode))
            {
                if(!neighbor.walkable || closedSet.Contains(neighbor)){
                    continue;
                }

                //ajouter ce neighbor au openset ou bien changer la route vers ce neighber dans le cas ou il est une route plus optimal  a partir du current node

                int newCostToNeighbor=currentNode.gcost+getDistance(currentNode,neighbor);
                if(newCostToNeighbor<neighbor.gcost || !openSet.Contains(neighbor))
                {
                    neighbor.gcost=newCostToNeighbor;
                    neighbor.hcost =  getDistance(neighbor, targetNode);
                    neighbor.parent=currentNode;
                    if(!openSet.Contains(neighbor)){
                        openSet.Add(neighbor);
                    }
                    else
                    {
                        openSet.UpdateItem(neighbor);
                    }
                }
            }
        }
    }

    public int getDistance(Node a, Node b)
    {
        return Mathf.RoundToInt(Vector3.Distance(a.worldPosition, b.worldPosition));
    }

    public void retracePath(Node start,Node target)
    {
        List<Node> path = new List<Node>();
        Node tmp=target;
        while (tmp != start)
        {
            path.Add(tmp);
            tmp = tmp.parent;
        }

        path.Reverse();

        grid.path = path;
    }
}
