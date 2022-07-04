using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGridManager : MonoBehaviour
{
    private Pathfinding _pathfinding;

    private void Start()
    {
        _pathfinding = new Pathfinding(70, 70);
    }
}
