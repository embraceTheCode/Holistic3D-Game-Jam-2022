using UnityEngine;

public class PathNode
{
    private Grid _grid;

    public int x;
    public int y;
    public int gCost;
    public int hCost;
    public int fCost;
    public bool isWalkable;
    public PathNode previousPathNode;

    public PathNode(Grid grid, int x, int y)
    {
        _grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = Physics2D.OverlapBox(new Vector2(x - 30.4f, y - 40.4f), new Vector2(0.1f, 0.1f), 0f, 1 << LayerMask.NameToLayer("Wall")) == null;
    }

    public void SetIsWalkable()
    {
        isWalkable = !isWalkable;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + "," + y;
    }
}
