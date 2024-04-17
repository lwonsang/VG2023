using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class BoundaryManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static BoundaryManager Instance;
    public List<int2> GlobalBoundaryCoords;
    public Boolean instantiated = false;
    void Awake(){
        Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if(instantiated == false){
        //     if(PathfindingOld.Instance != null){
        //         Debug.Log("Pathfinding instance ready to go");
        //         updateWalkable();
        //         instantiated = true;
        //     }
        // }
    }
    void OnDrawGizmos()
    {
        // Debug.Log("GlobalBoundaryCoordsSize: " + GlobalBoundaryCoords.Count);
        for(int i = 0; i < GlobalBoundaryCoords.Count; i++){
            Gizmos.color = Color.yellow;
            int2 currcoord = GlobalBoundaryCoords[i];
            Vector3 center = new Vector3(currcoord.x, currcoord.y);
            Gizmos.DrawSphere(center, 0.15f);
        }
    }
    public void updateWalkable(){
        Debug.Log("UpdateWalkeable called");
        if(PathfindingOld.Instance == null){
            Debug.Log("UpdateWalkeable failed");
            instantiated = false;
        }
        else{
            Debug.Log("UpdateWalkeable succeeded");
            if(instantiated == false){
                PathfindingOld.Instance.setUnwalkableNodes(GlobalBoundaryCoords);
                instantiated = true;
            }
            
        }
            
    }
}
