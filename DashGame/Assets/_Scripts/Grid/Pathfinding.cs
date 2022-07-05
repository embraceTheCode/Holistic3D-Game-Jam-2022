using UnityEngine;
using System.Collections.Generic;

public class Pathfinding
{

    public static Pathfinding Instance;

    private const int MoveCost = 10;
    private const int DiagonalMoveCost = 14;

    private Grid _grid;
    private List<PathNode> _openNodes;
    private List<PathNode> _closedNodes;

    public Pathfinding(int width, int height)
    {
        _grid = new Grid(width,height,1f,(Grid grid, int x, int y) => new PathNode(grid, x, y),new Vector3(-30.5f,-40.5f,0));
    }

    public Grid GetGrid()
    {
        return _grid;
    }

    public List<PathNode> CalculatePath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = _grid.GetGridObject(startX,startY);
        PathNode endNode = _grid.GetGridObject(endX,endY);

        _openNodes = new List<PathNode> {startNode};
        _closedNodes = new List<PathNode>();

        for(int x=0; x < _grid.GetWidth(); x++)
        {
            for(int y=0; y < _grid.GetHeight(); y++)
            {
                PathNode pathNode = _grid.GetGridObject(x,y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.previousPathNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateHCost(startNode,endNode);

        while(_openNodes.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(_openNodes);
            if(currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            _openNodes.Remove(currentNode);
            _closedNodes.Add(currentNode);

            foreach (var node in GetNeighbours(currentNode))
            {
                if(_closedNodes.Contains(node)) continue;

                if(!node.isWalkable)
                {
                    _closedNodes.Add(node);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateHCost(currentNode, node);
                if(tentativeGCost < node.gCost)
                {
                    node.gCost = tentativeGCost;
                    node.previousPathNode = currentNode;
                    node.hCost = CalculateHCost(node,endNode);
                    node.CalculateFCost();

                    if(!_openNodes.Contains(node))
                    {
                        _openNodes.Add(node);
                    }
                }
            }
        }

        return null;
    }

    private List<PathNode> GetNeighbours(PathNode currentNode)
    {
        List<PathNode> neighbours = new List<PathNode>();
        if(currentNode.x > 0)
        {
            //Left
            neighbours.Add(_grid.GetGridObject(currentNode.x - 1, currentNode.y));
            //Left Down
            if(currentNode.y > 0) neighbours.Add(_grid.GetGridObject(currentNode.x - 1, currentNode.y - 1));
            //Left Up
            if(currentNode.y < _grid.GetHeight() - 1) neighbours.Add(_grid.GetGridObject(currentNode.x - 1, currentNode.y + 1));
        }
        if(currentNode.x < _grid.GetWidth() - 1)
        {
            //Right
            neighbours.Add(_grid.GetGridObject(currentNode.x + 1, currentNode.y));
            //Right Down
            if(currentNode.y > 0) neighbours.Add(_grid.GetGridObject(currentNode.x + 1, currentNode.y - 1));
            //Right Up
            if(currentNode.y < _grid.GetHeight() - 1) neighbours.Add(_grid.GetGridObject(currentNode.x + 1, currentNode.y + 1));
        }
        //Down
        if(currentNode.y > 0) neighbours.Add(_grid.GetGridObject(currentNode.x,currentNode.y - 1));
        //Up
        if(currentNode.y < _grid.GetHeight() - 1) neighbours.Add(_grid.GetGridObject(currentNode.x, currentNode.y + 1));

        return neighbours;
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode> {endNode};
        PathNode currentNode = endNode;
        while(currentNode.previousPathNode != null)
        {
            path.Add(currentNode.previousPathNode);
            currentNode = currentNode.previousPathNode;
        }
        path.Reverse();
        return path;
    }

    private int CalculateHCost(PathNode start, PathNode end) //Distance to end node
    {
        int xDistance = Mathf.Abs(start.x - end.x);
        int yDistance = Mathf.Abs(start.y - end.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return DiagonalMoveCost * Mathf.Min(xDistance,yDistance) + MoveCost * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> openNodes)
    {
        PathNode lowestFCostNode = openNodes[0];
        for(int i=1; i < openNodes.Count; i++)
        {
            if(openNodes[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = openNodes[i];
            }
        }
        return lowestFCostNode;
    }
}
