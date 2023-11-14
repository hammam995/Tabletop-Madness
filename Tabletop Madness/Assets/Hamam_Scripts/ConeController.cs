using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeController : MonoBehaviour
{
    public Tile[] tiles;
    public AudioClip[] clickingSFX;
    public GameManager gameManager;
    public bool isPlayer = true;
    public float moveSpeed = 1f;    

    private AudioSource source;
    private float journeyTime;
    private int currentWaypoint = 0;
    private int targetWaypoint = 0;
    private float startTime = 0f;
    private bool isFinished = false;

    private void Awake()
    {
        source = gameObject.GetComponent<AudioSource>();
        journeyTime = 1 / moveSpeed;
    }

    void Update()
    {
        if (currentWaypoint != targetWaypoint)
            MoveInArch();
    }

    public void MoveXTiles (int tilesToMove)
    {
        targetWaypoint = Mathf.Clamp(targetWaypoint + tilesToMove, 0, tiles.Length - 1);

        if (targetWaypoint == tiles.Length - 1)
            isFinished = true;
    }

    // to reset the waypoints system
    public void ResetCone()
    {
        currentWaypoint = 0;
        targetWaypoint = 0;
        isFinished = false;
        transform.position = tiles[0].GetWaypoint(isPlayer).position;
    }
    // this to do the movement from one point to other one look like in animated way but there is no animation is in arch
    private void MoveInArch()
    {
        if(currentWaypoint <= tiles.Length - 1)
        {
            if (startTime == 0)
                startTime = Time.time;
            Vector3 currentWaypointPosition = tiles[currentWaypoint].GetWaypoint(isPlayer).position;
            Vector3 nextWaypointPosition = tiles[currentWaypoint + 1].GetWaypoint(isPlayer).position;

            Vector3 center = (currentWaypointPosition + nextWaypointPosition) * 0.5f;
            center -= new Vector3(0, 1, 0);

            Vector3 currentRelativeWaypoint = currentWaypointPosition - center;
            Vector3 nextRelativeWaypoint = nextWaypointPosition - center;
            float fracComplete = (Time.time - startTime) / journeyTime;

            transform.position = Vector3.Slerp(currentRelativeWaypoint, nextRelativeWaypoint, fracComplete);
            transform.position += center;

            if (transform.position == nextWaypointPosition)
            {
                currentWaypoint++;
                startTime = 0;
                source.PlayOneShot(clickingSFX[UnityEngine.Random.Range(0, clickingSFX.Length)]);
            }
                            
            if (currentWaypoint == targetWaypoint)
                gameManager.ReachedDestination(isPlayer, isFinished);                

                
        }
    }
}
