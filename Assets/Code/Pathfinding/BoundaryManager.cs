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
    void Awake(){
        Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        PathfindingOld.Instance.setUnwalkableNodes(GlobalBoundaryCoords);
    }
}
