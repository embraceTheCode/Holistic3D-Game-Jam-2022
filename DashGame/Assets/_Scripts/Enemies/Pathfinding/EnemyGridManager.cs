using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGridManager : MonoBehaviour
{
    public static Pathfinding pathfinding;

    private void Awake()
    {
        pathfinding = new Pathfinding(70, 70);
    }
}
