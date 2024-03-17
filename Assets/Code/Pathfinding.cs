using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
        private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public static Pathfinding Instance { get; private set; }

    private GridMap<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;

    public Pathfinding(int width, int height, Vector3 originPosition) {
        Instance = this;
        Debug.Log("Create Grid");
        grid = new GridMap<PathNode>(width, height, 1f, originPosition, (GridMap<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    public GridMap<PathNode> GetGrid() {
        return grid;
    }

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition) {
        Debug.Log("Running FindPath...");
        grid.GetXY(startWorldPosition, out int startX, out int startY);
        grid.GetXY(endWorldPosition, out int endX, out int endY);

        List<PathNode> path = FindPath(startX, startY, endX, endY);
        if (path == null) {
            Debug.Log("Path is Null");
            return null;
        } else {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (PathNode pathNode in path) {
                vectorPath.Add(new Vector3(pathNode.x, pathNode.y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f);
            }
            return vectorPath;
        }
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY) {
        Debug.Log("Starting FindPath...");
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);

        if (startNode == null || endNode == null) {
            Debug.Log("Start or End is Null");
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
            Debug.Log("Got node");
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode) {
                // Reached final node
                // PathfindingDebugStepVisual.Instance.TakeSnapshot(grid, currentNode, openList, closedList);
                // PathfindingDebugStepVisual.Instance.TakeSnapshotFinalPath(grid, CalculatePath(endNode));
                Debug.Log("Found Path!");
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode)) {
                Debug.Log("Checked Neighbor");
                if (closedList.Contains(neighbourNode)){
                    Debug.Log("Neighbor is in ClosedList");
                    continue;
                } 
                if (!neighbourNode.isWalkable) {
                    Debug.Log("Neighbor Not Walkable");
                    closedList.Add(neighbourNode);
                    continue;
                }
                Debug.Log("Neighbor Viable");
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
        Debug.Log("Out of Nodes on List");
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
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
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
