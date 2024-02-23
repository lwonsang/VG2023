using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap
{
    private int width;
    private int height;
    private int[,] gridArray;
    private float cellSize; // Used for calculation of GetWorldPosition

    public GridMap(int width, int height){
        this.width = width;
        this.height = height;

        gridArray = new int[width, height];

        for(int x = 0; x < gridArray.GetLength(0); x++){

        }
    }
    private Vector3 GetWorldPosition(int x, int y){
        return new Vector3(x,y) * cellSize;
    }
}
