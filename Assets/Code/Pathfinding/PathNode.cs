using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private GridMap<PathNode> grid;
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;
    public String prevDirection;
    public String currDirection;
    // Refers to previous node in path
    public PathNode cameFromNode;
    public bool isWalkable = true;
    public PathNode(GridMap<PathNode> grid, int x, int y){
        this.grid = grid;
        this.x = x;
        this.y = y;
        // isWalkable = true;
        // int[] unwalkableXnodes = {3,4,5,6,6,5,4,3,3,4,5,6,6,5,4,3,12,13,14,15,12,13,14,15,15,14,13,12,12,13,14,15};
        // int[] unwalkableYnodes = {7,7,7,7,8,8,8,8,9,9,9,9,10,10,10,10,8,8,8,8,9,9,9,9,10,10,10,10,11,11,11,11};
        // for(int i = 0; i < 32; i++){
        //     if (this.x == unwalkableXnodes[i] && this.y == unwalkableYnodes[i]){
        //         isWalkable = false;
        //     }
        // }
    }

    
    public void CalculateFCost() {
        if (cameFromNode != null){
            fCost = gCost + hCost;
            if (prevDirection == currDirection){
                // Prioritize same direction rather than diagonal
                fCost = fCost/500;
            }
        }
        else{
            fCost = gCost + hCost;
        }
        
    }

     public void CalculateFCost(bool sameDirection) {

        fCost = gCost + hCost;
    }

    public void SetIsWalkable(bool isWalkable) {
        this.isWalkable = isWalkable;
        grid.TriggerGridObjectChanged(x, y);
    }

    public override string ToString() {
        return x + "," + y;
    }

//     /* 
//     ------------------- Code Monkey -------------------

//     Thank you for downloading this package
//     I hope you find it useful in your projects
//     If you have any questions let me know
//     Cheers!

//                unitycodemonkey.com
//     --------------------------------------------------
//  */

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PathNode {

//     private Grid<PathNode> grid;
//     public int x;
//     public int y;

//     public int gCost;
//     public int hCost;
//     public int fCost;

//     public bool isWalkable;
//     public PathNode cameFromNode;

//     public PathNode(Grid<PathNode> grid, int x, int y) {
//         this.grid = grid;
//         this.x = x;
//         this.y = y;
//         isWalkable = true;
//     }


// }

}
