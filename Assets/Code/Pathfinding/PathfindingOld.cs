using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using System;

public class PathfindingOld: MonoBehaviour
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public static PathfindingOld Instance;

    public GridMap<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;
    public Vector3 OriginalPositionCoord;
    public List<int2> GlobalBoundaryCoords;

    Boolean instantiated = false;



    // public PathfindingOld(int width, int height, Vector3 originPosition) {
        
    //     // 
    //     grid = new GridMap<PathNode>(width, height, 1f, originPosition, (GridMap<PathNode> g, int x, int y) => new PathNode(g, x, y));
    //     OriginalPositionCoord = originPosition;
    //     Debug.Log("OriginalPosition: " + OriginalPositionCoord);
        
    // }

    private PathfindingOld(){

    }

    void Awake(){
        Instance = this;
    }

    public void createGrid(int width, int height, Vector3 originPosition){
        grid = new GridMap<PathNode>(width, height, 1f, originPosition, (GridMap<PathNode> g, int x, int y) => new PathNode(g, x, y));
        OriginalPositionCoord = originPosition;
        // debug.log.Log("OriginalPosition: " + OriginalPositionCoord);
    }

    public void setUnwalkableNodes(List<int2> boundcoords){
        // Set unwalkable nodes
        // debug.log.Log("Unwalkable Node Count: " + boundcoords.Count);
        
        //  = BoundaryManager.Instance.GlobalBoundaryCoords;
        for(int i = 0; i < boundcoords.Count; i++){
            // int xcoord = boundcoords[i].x - (int)OriginalPositionCoord.x;
            // Debug.Log("Original Position coordinates: " + OriginalPositionCoord.x + ", " + OriginalPositionCoord.y);
            // int ycoord = boundcoords[i].y - (int)OriginalPositionCoord.y;
            int xcoord = boundcoords[i].x + 35;
            // debug.log.Log("Original Position coordinates: " + OriginalPositionCoord.x + ", " + OriginalPositionCoord.y);
            int ycoord = boundcoords[i].y + 18;
            // print("original coord: " + boundcoords[i].x + ", " + boundcoords[i].y + " adjusted cell coords: "+ xcoord + ", " + ycoord);
            // Sometimes the boundaries go off the pathfinding grid, just ignore if that's the case.
            if(xcoord >= 0 && ycoord >= 0 && xcoord < 168 && ycoord < 87){
                GetNode(xcoord, ycoord).SetIsWalkable(false);
            }
            
        }
    }

    public GridMap<PathNode> GetGrid() {
        return grid;
    }

    public void BlahBlahBlahTest(){
        // debug.log.Log("Blah blah blah test");
    }

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition) {
        if(!instantiated){
            setUnwalkableNodes(GlobalBoundaryCoords);
            instantiated = true;
        }
        grid.GetXY(startWorldPosition, out int startX, out int startY);
        grid.GetXY(endWorldPosition, out int endX, out int endY);
        // debug.log.Log("Running FindPath...");
        List<PathNode> path = FindPath(startX, startY, endX, endY);
        // debug.log.Log("FindPath Completed");
        if (path == null) {
            // debug.log.Log("Path is Null");
            return null;
        } else {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (PathNode pathNode in path) {
                vectorPath.Add(new Vector3(pathNode.x, pathNode.y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f);
            }
            // debug.log.Log("Ready to return");
            return vectorPath;
        }
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY) {
        // debug.log.Log("Starting FisndPath...");
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);

        if (startNode == null || endNode == null) {
            // // debug.log.Log("Start or End is Null");
            // Invalid Path
            return null;
        }

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                PathNode pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = 99999999;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();
        
        // PathfindingDebugStepVisual.Instance.ClearSnapshots();
        // PathfindingDebugStepVisual.Instance.TakeSnapshot(grid, startNode, openList, closedList);

        while (openList.Count > 0) {
            // Debug.Log("Got node");
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode) {
                // Reached final node
                // PathfindingDebugStepVisual.Instance.TakeSnapshot(grid, currentNode, openList, closedList);
                // PathfindingDebugStepVisual.Instance.TakeSnapshotFinalPath(grid, CalculatePath(endNode));
                // Debug.Log("Found Path!");
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode)) {
                // Debug.Log("Checked Neighbor");
                if (closedList.Contains(neighbourNode)){
                    // Debug.Log("Neighbor is in ClosedList");
                    continue;
                } 
                if (!neighbourNode.isWalkable) {
                    // Debug.Log("Neighbor Not Walkable");
                    closedList.Add(neighbourNode);
                    continue;
                }
                // Debug.Log("Neighbor Viable");
                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost) {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.prevDirection = currentNode.currDirection;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode)) {
                        openList.Add(neighbourNode);
                    }
                }
                // PathfindingDebugStepVisual.Instance.TakeSnapshot(grid, currentNode, openList, closedList);
            }
        }

        // Out of nodes on the openList
        // Debug.Log("Out of Nodes on List");
        return null;
    }

    // private List<PathNode> GetNeighbourList(PathNode currentNode) {
    //     List<PathNode> neighbourList = new List<PathNode>();

    //     if (currentNode.x - 1 >= 0) {
    //         // Left
    //         neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
    //         // Left Down
    //         if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
    //         // Left Up
    //         if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
    //     }
    //     if (currentNode.x + 1 < grid.GetWidth()) {
    //         // Right
    //         neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
    //         // Right Down
    //         if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
    //         // Right Up
    //         if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
    //     }
    //     // Down
    //     if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
    //     // Up
    //     if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));

    //     return neighbourList;
    // }


    // Tank version
    private List<PathNode> GetNeighbourList(PathNode currentNode) {
        List<PathNode> neighbourList = new List<PathNode>();

        if (currentNode.x - 1 >= 0) {
            // Left
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            currentNode.currDirection = "Left";
           
        }
        if (currentNode.x + 1 < grid.GetWidth()) {
            // Right
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
            currentNode.currDirection = "Right";
            
        }
        // Down
        if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        currentNode.currDirection = "Down";
        // Up
        if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));
        currentNode.currDirection = "Up";

        return neighbourList;
    }

    public PathNode GetNode(int x, int y) {

        return grid.GetGridObject(x, y);
    }

    private List<PathNode> CalculatePath(PathNode endNode) {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.cameFromNode != null) {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b) {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        // return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        return MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList) {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++) {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost) {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }
}

// public class GridMap<TGridObject> {

//     public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
//     public class OnGridObjectChangedEventArgs : EventArgs {
//         public int x;
//         public int y;
//     }

//     private int width;
//     private int height;
//     private float cellSize;  // Used for calculation of GetWorldPosition
//     private Vector3 originPosition;
//     private TGridObject[,] gridArray;
//     public const int sortingOrderDefault = 5000;

//     public GridMap(int width, int height, float cellSize, Vector3 originPosition, Func<GridMap<TGridObject>, int, int, TGridObject> createGridObject) {
//         this.width = width;
//         this.height = height;
//         this.cellSize = cellSize;
//         this.originPosition = originPosition;

//          gridArray = new TGridObject[width, height];

//         for (int x = 0; x < gridArray.GetLength(0); x++) {
//             for (int y = 0; y < gridArray.GetLength(1); y++) {
//                 gridArray[x, y] = createGridObject(this, x, y);
//             }
//         }

//         bool showDebug = false;
//         if (showDebug) {
//             TextMesh[,] debugTextArray = new TextMesh[width, height];

//             for (int x = 0; x < gridArray.GetLength(0); x++) {
//                 for (int y = 0; y < gridArray.GetLength(1); y++) {
//                     // debugTextArray[x, y] = CreateWorldText(gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 5, Color.white, TextAnchor.MiddleCenter);
//                     // Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
//                     // Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
//                 }
//             }
//             // Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
//             // Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

//             OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => {
//                 // debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
//             };
//         }
//     }

//     public int GetWidth() {
//         return width;
//     }

//     public int GetHeight() {
//         return height;
//     }

//     public float GetCellSize() {
//         return cellSize;
//     }

//     public Vector3 GetWorldPosition(int x, int y) {
//         return new Vector3(x, y) * cellSize + originPosition;
//     }

//     public void GetXY(Vector3 worldPosition, out int x, out int y) {
//         x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
//         y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
//     }

//     public void SetGridObject(int x, int y, TGridObject value) {
//         if (x >= 0 && y >= 0 && x < width && y < height) {
//             gridArray[x, y] = value;
//             if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
//         }
//     }

//     public void TriggerGridObjectChanged(int x, int y) {
//         if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
//     }

//     public void SetGridObject(Vector3 worldPosition, TGridObject value) {
//         int x, y;
//         GetXY(worldPosition, out x, out y);
//         SetGridObject(x, y, value);
//     }

//     public TGridObject GetGridObject(int x, int y) {
//         if (x >= 0 && y >= 0 && x < width && y < height) {
//             return gridArray[x, y];
//         } else {
//             return default(TGridObject);
//         }
//     }

//     public TGridObject GetGridObject(Vector3 worldPosition) {
//         int x, y;
//         GetXY(worldPosition, out x, out y);
//         return GetGridObject(x, y);
//     }

//     // public void SetValue(int x, int y, int value) {
//     //     if (x >= 0 && y >= 0 && x < width && y < height) {
//     //         gridArray[x, y] = value;
//     //         if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
//     //     }
//     // }

//     // public void SetValue(Vector3 worldPosition, int value) {
//     //     int x, y;
//     //     GetXY(worldPosition, out x, out y);
//     //     SetValue(x, y, value);
//     // }

//     // public int GetValue(int x, int y) {
//     //     if (x >= 0 && y >= 0 && x < width && y < height) {
//     //         return gridArray[x, y];
//     //     } else {
//     //         return 0;
//     //     }
//     // }

//     // public int GetValue(Vector3 worldPosition) {
//     //     int x, y;
//     //     GetXY(worldPosition, out x, out y);
//     //     return GetValue(x, y);
//     // }

    
//     public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 5, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault) {
//         if (color == null) color = Color.white;
//         return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
//     }
    
//     // Create Text in the World
//     public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder) {
//         GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
//         Transform transform = gameObject.transform;
//         transform.SetParent(parent, false);
//         transform.localPosition = localPosition;
//         TextMesh textMesh = gameObject.GetComponent<TextMesh>();
//         textMesh.anchor = textAnchor;
//         textMesh.alignment = textAlignment;
//         textMesh.text = text;
//         textMesh.fontSize = fontSize;
//         textMesh.color = color;
//         textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
//         return textMesh;
//     }
// }
