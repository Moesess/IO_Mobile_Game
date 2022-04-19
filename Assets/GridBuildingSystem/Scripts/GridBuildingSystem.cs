using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridBuildingSystem : MonoBehaviour{
    
    public static GridBuildingSystem Instance {get; private set;}

    public event EventHandler OnObjectPlaced;
    public event EventHandler OnSelectedChanged;

    [SerializeField] private List<GridPlacedObjectSO> gridPlacedObjectSOList = null;
    private GridPlacedObjectSO gridPlacedObjectSO;
    private GridXZ<GridObject> grid;

    private void Awake() {
        // Set default grid sizes and create new grid instance
        Instance = this;

        int gridWidth = 50;
        int gridHeight = 50;
        float cellSize = 1f;
        grid = new GridXZ<GridObject>(gridWidth, gridHeight, cellSize, Vector3.zero, (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z));
        gridPlacedObjectSO = null;
    }

    private void Start() {
        TryPlaceObject(new Vector2Int(0, 0), GridBuildingSystemAssets.Instance.castle, out GridPlacedObject gridPlacedObject);
    }

    public class GridObject {
        // Object populating the grid, eg. building, tree. One per grid cell.
        private GridXZ<GridObject> grid;
        private int x;
        private int z;
        public GridPlacedObject gridPlacedObject;

        public GridObject(GridXZ<GridObject> grid, int x, int z){
            this.grid = grid;
            this.x = x;
            this.z = z;
            gridPlacedObject = null;
        }

        public override string ToString(){
            return x + ", " + z + "\n" + gridPlacedObject;
        }

        public bool CanBuild(){
            return gridPlacedObject == null;
        }

        public void TriggerGridObjectChanged() {
            grid.TriggerGridObjectChanged(x, z);
        }

        public void SetPlacedObject(GridPlacedObject gridPlacedObject){
            this.gridPlacedObject = gridPlacedObject;
            TriggerGridObjectChanged();
        }

        public GridPlacedObject GetPlacedObject(){
            return gridPlacedObject;
        }
    }


    private void Update() {
        UpdateObjectPlacement();

        if (Input.GetMouseButtonDown(1)) {
            DeselectObjectType();
        }
    }
    
    private void UpdateObjectPlacement(){
        if(Input.GetMouseButton(0) && gridPlacedObjectSO != null && !UtilsClass.IsPointerOverUI()){
            Vector3 mousePos = Mouse3D.GetMouseWorldPosition();
            grid.GetXZ(mousePos, out int x, out int z);

            Vector2Int gridPlacedObjectOrigin = new Vector2Int(x, z);
            if(TryPlaceObject(gridPlacedObjectOrigin, gridPlacedObjectSO, out GridPlacedObject gridPlacedObject)){
                // Debug.Log("Building Placed!");
            }else{
                // UtilsClass.CreateWorldTextPopup("Cannot Build Here!", mousePos);
            }
        }

        if(Input.GetMouseButton(0) && gridPlacedObjectSO == null){

        }
    }

    private void DeselectObjectType() {
        gridPlacedObjectSO = null;
        RefreshSelectedObjectType();
    }

    private void RefreshSelectedObjectType() {

        if (gridPlacedObjectSO == null) {
           
        } else {
          
        }

        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool TryPlaceObject(
            Vector2Int gridPlacedObjectOrigin,
            GridPlacedObjectSO gridPlacedObjectSO,
            out GridPlacedObject gridPlacedObject
        ){
        List<Vector2Int> gridPosList = gridPlacedObjectSO.GetGridPositionList(gridPlacedObjectOrigin);
        bool canBuild = true;
        
        foreach(Vector2Int gridPos in gridPosList){ // Check if whole area that object occupies is buildable
            bool isValidPos = grid.IsValidGridPosition(gridPos);

            if(!isValidPos){
                canBuild = false;
                break;
            }

            if(!grid.GetGridObject(gridPos.x, gridPos.y).CanBuild()){
                canBuild = false;
                break;
            }
        }
        
        if (canBuild){ // If subgrid is buildable, instantiate every single one with scriptable object transform
            Vector3 gridPlacedObjectWorldPosition = grid.GetWorldPosition(gridPlacedObjectOrigin.x, gridPlacedObjectOrigin.y) * grid.GetCellSize();

            gridPlacedObject = GridPlacedObject.Create(gridPlacedObjectWorldPosition, gridPlacedObjectOrigin, gridPlacedObjectSO);
            // Debug.Log(gridPlacedObject);
            foreach (Vector2Int gridPos in gridPosList){
                grid.GetGridObject(gridPos.x, gridPos.y).SetPlacedObject(gridPlacedObject);
            }

            gridPlacedObject.GridSetupDone();

            OnObjectPlaced?.Invoke(gridPlacedObject, EventArgs.Empty);

            return true;
        }
        else{
            gridPlacedObject = null;
            return false;
        }
    }

    public void SelectPlacedObjectSO(GridPlacedObjectSO gridPlacedObjectSO) {
        this.gridPlacedObjectSO = gridPlacedObjectSO;
        RefreshSelectedObjectType();
    }

    public GridPlacedObjectSO GetGridPlacedObjectSO(){
        return gridPlacedObjectSO;
    }

    public void SetSelectedPlacedObject(GridPlacedObjectSO gridPlacedObjectSO) {
        this.gridPlacedObjectSO = gridPlacedObjectSO;
        RefreshSelectedObjectType();
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition) {
        grid.GetXZ(worldPosition, out int x, out int z);
        return new Vector2Int(x, z);
    }

    public Vector3 GetWorldPosition(Vector2Int gridPosition) {
        return grid.GetWorldPosition(gridPosition.x, gridPosition.y);
    }

    public GridObject GetGridObject(Vector2Int gridPosition) {
        return grid.GetGridObject(gridPosition.x, gridPosition.y);
    }

    public GridObject GetGridObject(Vector3 worldPosition) {
        return grid.GetGridObject(worldPosition);
    }

    public bool IsValidGridPosition(Vector2Int gridPosition) {
        return grid.IsValidGridPosition(gridPosition);
    }
}
