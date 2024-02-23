using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour


{
    private GridMap grid;
    // Start is called before the first frame update

    // Where you want the path map grid to start.
    Vector3 originalPosition = new Vector3(-25, -25);
    void Start()
    {
        grid = new GridMap(100, 100, 1f, originalPosition);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            grid.SetValue(GetMouseWorldPosition(),56);
        }
        if (Input.GetMouseButtonDown(1)){
            Debug.Log(grid.GetValue(GetMouseWorldPosition()));
        }
    }


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
}
