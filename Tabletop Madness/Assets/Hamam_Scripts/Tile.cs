using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Transform playerWaypoint, enemyWaypoint;

    private void Awake()
    {
        playerWaypoint = transform.GetChild(0);
        enemyWaypoint = transform.GetChild(1);
    }

    public Transform GetWaypoint(bool isPlayer)
    {
        if (isPlayer)
            return playerWaypoint;
        else
            return enemyWaypoint;
    }
}
