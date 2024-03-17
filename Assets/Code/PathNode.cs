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
    public bool isWalkable;
    public PathNode(GridMap<PathNode> grid, int x, int y){
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;
    }

    
    public void CalculateFCost() {
        if (cameFromNode != null){
            fCost = gCost + hCost;
            if (prevDirection == currDirection){
                // Prioritize same direction rather than diagonal
                fCost = fCost/80;
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
