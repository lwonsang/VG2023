using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{
     private PathfindingOld pathfinding;
     Vector3 originalPosition = new Vector3(-10, -10);
     int offsetX;
     int offsetY;
    // Start is called before the first frame update
    private void Start(){
        Debug.Log("Pathfinding Started");
        pathfinding = new PathfindingOld(20, 20, originalPosition);
        offsetX = (int) originalPosition.x;
        offsetY = (int) originalPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
