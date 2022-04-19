using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridXZ<TGridObject> {
    public const int HEAT_MAP_MAX_VALUE = 100;
    public const int HEAT_MAP_MIN_VALUE = 0;


    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs{
        // Event, evaluates position of tile changed in grid
        public int x;
        public int z;
    }


    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private TGridObject[,] gridArray;
    private TextMesh[,] debugTextArray;


    public GridXZ(
        int width, int height, float cellSize, Vector3 originPosition,
        Func<GridXZ<TGridObject>, int, int, TGridObject> createGridObject // Function creates grid object with its grid reference, x and y position, and the type of object
    ) {
        // Map grid constructor
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height];
        
        for(int x = 0; x < gridArray.GetLength(0); x++){
            for(int z = 0; z < gridArray.GetLength(1); z++){
                gridArray[x, z] = createGridObject(this, x, z);
            }
        }

        // !!!! DEBUG MODE !!!!
        bool showDebug = false;
        if(showDebug){
            TextMesh[,] debugTextArray = new TextMesh[width, height];

            for(int x = 0; x < gridArray.GetLength(0); x++){
                for(int z = 0; z < gridArray.GetLength(1); z++){
                    debugTextArray[x, z] = UtilsClass.CreateWorldText(
                        gridArray[x, z]?.ToString(),
                        null,
                        GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * .5f,
                        10,
                        Color.white,
                        TextAnchor.MiddleCenter,
                        TextAlignment.Center
                    );

                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 100f);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => {
                debugTextArray[eventArgs.x, eventArgs.z].text = gridArray[eventArgs.x, eventArgs.z]?.ToString();
            };
        }

        // !!!! DEBUG MODE !!!!
    }

    public int GetWidth() {
        return this.width;
    }


    public int GetHeight() {
        return this.height;
    }


    public float GetCellSize() {
        return cellSize;
    }


    public Vector3 GetWorldPosition(int x, int z) {
        // Getter of single cell position in a grid
        return new Vector3(x, 0, z) * cellSize + originPosition;
    }


    public void GetXZ(Vector3 worldPosition, out int x, out int z) {
        // Getter of world coordinates within a grid
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
    }


    public void SetGridObject(int x, int z, TGridObject value) {
        // Set Value of a Tile based on x and y coords within grid
        if(x >= 0 && z >= 0 && x < width && z < height){
            gridArray[x, z] = value;
            TriggerGridObjectChanged(x, z);
        }
    }


    public void TriggerGridObjectChanged(int x, int z){
        if(OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, z = z });
    }


    public void SetGridObject(Vector3 worldPosition, TGridObject value) {
        // Set Value of a Tile based on world position coords
        GetXZ(worldPosition, out int x, out int z);

        SetGridObject(x, z, value);
    }


    public TGridObject GetGridObject(int x, int z) {
        // Get Value of a Tile based on x and y coords within grid
        if(x >= 0 && z >= 0 && x < width && z < height){
            return gridArray[x, z];
        } else {
            return default(TGridObject);
        }
    }


    public TGridObject GetGridObject(Vector3 worldPosition) {
        // Get Value of a Tile based on world position coords
        GetXZ(worldPosition, out int x, out int z);
        return GetGridObject(x, z);
    }

    public bool IsValidGridPosition(Vector2Int gridPos) {
        // Check if given position is part of grid
        int x = gridPos.x;
        int z = gridPos.y;

        if (x >= 0 && z >= 0 && x < width && z < height) {
            return true;
        } else {
            return false;
        }
    }
}
