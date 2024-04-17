using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class GetSelfBoundaries : MonoBehaviour
{
    // Outlets
    // Collider _collider = GetComponent<Collider>();
    BoxCollider2D _collider;

    List<int2> boundcoords;

    // Start is called before the first frame update
    
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        Bounds boxBounds = _collider.bounds;
        // int2 topRight = new int2(((int) Mathf.Ceil(boxBounds.center.x + boxBounds.extents.x)) + 1, ((int) Mathf.Ceil(boxBounds.center.y + boxBounds.extents.y)) + 1);
        // int2 bottomRight = new int2((int) Mathf.Ceil(boxBounds.center.x + boxBounds.extents.x) + 1, ((int) Mathf.Floor(boxBounds.center.y - boxBounds.extents.y)) - 1);
        // int2 topLeft = new int2((int) Mathf.Floor(boxBounds.center.x - boxBounds.extents.x) - 1, ((int) Mathf.Ceil(boxBounds.center.y + boxBounds.extents.y)) + 1);
        // int2 bottomLeft = new int2((int) Mathf.Floor(boxBounds.center.x - boxBounds.extents.x) - 1, ((int) Mathf.Floor(boxBounds.center.y - boxBounds.extents.y)) - 1);
        // Debug.Log("Top right: " + topRight + " Top left: " + topLeft + " bottomRight: " + bottomRight + " bottomLeft: " + bottomLeft);

        int2 topRight = new int2(((int) Mathf.Ceil(boxBounds.center.x + boxBounds.extents.x)), ((int) Mathf.Ceil(boxBounds.center.y + boxBounds.extents.y)));
        int2 bottomRight = new int2((int) Mathf.Ceil(boxBounds.center.x + boxBounds.extents.x), ((int) Mathf.Floor(boxBounds.center.y - boxBounds.extents.y)));
        int2 topLeft = new int2((int) Mathf.Floor(boxBounds.center.x - boxBounds.extents.x), ((int) Mathf.Ceil(boxBounds.center.y + boxBounds.extents.y)));
        int2 bottomLeft = new int2((int) Mathf.Floor(boxBounds.center.x - boxBounds.extents.x), ((int) Mathf.Floor(boxBounds.center.y - boxBounds.extents.y)));
        Debug.Log("Top right: " + topRight + " Top left: " + topLeft + " bottomRight: " + bottomRight + " bottomLeft: " + bottomLeft);

        boundcoords = new List<int2>
        {
            topRight,
            topLeft,
            bottomRight,
            bottomLeft
        };

        // Add top of boundary
        for(int i = topLeft.x + 1; i < topRight.x; i++){
            int2 newcoord = new int2(i, topLeft.y);
            boundcoords.Add(newcoord);
        }
        // Add bottom of boundary
        for(int i = bottomLeft.x + 1; i < bottomRight.x; i++){
            int2 newcoord = new int2(i, bottomLeft.y);
            boundcoords.Add(newcoord);
        }
        // Add right side of boundary
        for(int i = bottomRight.y + 1; i < topRight.y; i++){
            int2 newcoord = new int2(bottomRight.x, i);
            boundcoords.Add(newcoord);
        }
        // Add left side of boundary
        for(int i = bottomLeft.y + 1; i < topLeft.y; i++){
            int2 newcoord = new int2(bottomLeft.x, i);
            boundcoords.Add(newcoord);
        }
        PathfindingOld.Instance.GlobalBoundaryCoords.AddRange(boundcoords);
        // BoundaryManager.Instance.GlobalBoundaryCoords.AddRange(boundcoords);
        // BoundaryManager.Instance.updateWalkable();
    }
    

    
}
