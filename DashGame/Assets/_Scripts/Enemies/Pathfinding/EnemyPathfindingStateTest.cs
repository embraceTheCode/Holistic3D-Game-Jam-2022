using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfindingStateTest : MonoBehaviour
{
    private Pathfinding _pathfinding;
    private float timer = 1f;
    [SerializeField] Transform player;

    private void Start()
    {
        _pathfinding = EnemyGridManager.pathfinding;
    }
    private void Update()
    {
        if(timer > 0) timer -= Time.deltaTime;
        else
        {
            timer = 1;
            Grid grid = _pathfinding.GetGrid();
            grid.GetXY(transform.position, out int x1, out int y1);
            grid.GetXY(player.position, out int x2, out int y2);

            List<PathNode> path = _pathfinding.CalculatePath(x1,y1,x2,y2);
            if(path != null)
            {
                for(int i=0; i<path.Count - 1; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].x -30.5f, path[i].y-40.5f) + Vector3.one * .5f, new Vector3(path[i+1].x -30.5f,path[i+1].y-40.5f) + Vector3.one * .5f, Color.green, 1);
                }
            }
        }
    }   
}
