using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Testing : MonoBehaviour


{
    
    private Pathfinding pathfinding;
     Vector3 originalPosition = new Vector3(0, 0);
    private void Start(){
        Debug.Log("Pathfinding Started");
        pathfinding = new Pathfinding(50, 50, originalPosition);
    }

    private void Update(){
        if (Input.GetMouseButtonDown(0)) {
            
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            Debug.Log("Mouse X: " + x + " Mouse Y: " + y);
            List<PathNode> path = pathfinding.FindPath(0, 0, x, y);
            if (path != null) {
                Debug.Log("Path Coords:");
                for (int i=0; i<path.Count - 1; i++) {
                    Debug.Log(path[i].x + ", " + path[i].y + " to " + path[i+1].x + ", " + path[i+1].y);
                    // Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 1f + Vector3.one * 5f, new Vector3(path[i+1].x, path[i+1].y) * 1f + Vector3.one * 5f, Color.green, 5f);
                    
                    // coord * cellsize + offset of half of cell size
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 1f + Vector3.one * 0.5f, new Vector3(path[i+1].x, path[i+1].y) * 1f + Vector3.one * 0.5f, Color.green, 10f);

                    Debug.Log("Path Drawn");
                }
            }
            else{
                Debug.Log("Path Not Working");
            }
            // characterPathfinding.SetTargetPosition(mouseWorldPosition);
        }

        if (Input.GetMouseButtonDown(1)) {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            pathfinding.GetNode(x, y).SetIsWalkable(!pathfinding.GetNode(x, y).isWalkable);
        }
    }
//     private GridMap grid;
//     // Start is called before the first frame update

//     // Where you want the path map grid to start.

//     private float mouseMoveTimer;
//     private float mouseMoveTimerMax = .01f;
//     // x, y
//     Vector3 originalPosition = new Vector3(-20, -10);
//     void Start()
//     {
//         grid = new GridMap(50, 50, 1f, originalPosition);
//     }

//     // Update is called once per frame
// private void Update() {
//         //HandleClickToModifyGrid();
//         // HandleHeatMapMouseMove();
//         if (Input.GetMouseButtonDown(0)) {
//             grid.SetValue(GetMouseWorldPosition(), 1);
//         }
//         if (Input.GetMouseButtonDown(1)) {
//             Debug.Log(grid.GetValue(GetMouseWorldPosition()));
//         }
//     }

//     private void HandleClickToModifyGrid() {
        
//     }

//     private void HandleHeatMapMouseMove() {
//         mouseMoveTimer -= Time.deltaTime;
//         if (mouseMoveTimer < 0f) {
//             mouseMoveTimer += mouseMoveTimerMax;
//             int gridValue = grid.GetValue(GetMouseWorldPosition());
//             grid.SetValue(GetMouseWorldPosition(), gridValue + 1);
//         }
//     }

        // Get Mouse World Position for testing
    public static Vector3 GetMouseWorldPosition(){
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    public static Vector3 GetMouseWorldPositionWithZ(){
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera){
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera){
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

// DON"T UNCOMMENT THIS
    // private class HeatMapVisual {

    //     private GridMap grid;
    //     private Mesh mesh;

    //     public HeatMapVisual(GridMap grid, MeshFilter meshFilter) {
    //         this.grid = grid;
            
    //         mesh = new Mesh();
    //         meshFilter.mesh = mesh;

    //         UpdateHeatMapVisual();

    //         grid.OnGridValueChanged += Grid_OnGridValueChanged;
    //     }

    //     private void Grid_OnGridValueChanged(object sender, System.EventArgs e) {
    //         UpdateHeatMapVisual();
    //     }

    //     public void UpdateHeatMapVisual() {
    //         Vector3[] vertices;
    //         Vector2[] uv;
    //         int[] triangles;

    //         MeshUtils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out vertices, out uv, out triangles);

    //         for (int x = 0; x < grid.GetWidth(); x++) {
    //             for (int y = 0; y < grid.GetHeight(); y++) {
    //                 int index = x * grid.GetHeight() + y;
    //                 Vector3 baseSize = new Vector3(1, 1) * grid.GetCellSize();
    //                 int gridValue = grid.GetValue(x, y);
    //                 int maxGridValue = 100;
    //                 float gridValueNormalized = Mathf.Clamp01((float)gridValue / maxGridValue);
    //                 Vector2 gridCellUV = new Vector2(gridValueNormalized, 0f);
    //                 MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x, y) + baseSize * .5f, 0f, baseSize, gridCellUV, gridCellUV);
    //             }
    //         }

    //         mesh.vertices = vertices;
    //         mesh.uv = uv;
    //         mesh.triangles = triangles;
    //     }

    // }


}
