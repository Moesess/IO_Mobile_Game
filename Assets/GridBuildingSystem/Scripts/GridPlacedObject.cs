using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlacedObject : MonoBehaviour{
    public static GridPlacedObject Create(Vector3 worldPosition, Vector2Int origin, GridPlacedObjectSO gridPlacedObjectSO){
        Transform gridPlacedObjectTransform = Instantiate(gridPlacedObjectSO.prefab, worldPosition, Quaternion.identity);
        GridPlacedObject gridPlacedObject = gridPlacedObjectTransform.GetComponent<GridPlacedObject>();

        gridPlacedObject.gridPlacedObjectSO = gridPlacedObjectSO;
        gridPlacedObject.origin = origin;
        // gridPlacedObject.building = new Building(0, 0);
        
        gridPlacedObject.Setup();
        return gridPlacedObject;
    }


    private GridPlacedObjectSO gridPlacedObjectSO;
    private Vector2Int origin;
    private BuildingSO building;

    protected virtual void Setup(){
        // Debug.Log("GridPlacedObject.Setup() " + transform);
    }

    public virtual void GridSetupDone(){
        // Debug.Log("GridPlacedObject.GridSetupDone()" + transform);
    }

    public Vector2Int GetGridPosition() {
        return origin;
    }

    protected virtual void TriggerGridObjectChanged() {
        foreach (Vector2Int gridPos in GetGridPositionList()) {
            GridBuildingSystem.Instance.GetGridObject(gridPos).TriggerGridObjectChanged();
        }
    }
    
    public List<Vector2Int> GetGridPositionList() {
        return gridPlacedObjectSO.GetGridPositionList(origin);
    }

    public virtual void DestroySelf() {
        Destroy(gameObject);
    }

    public override string ToString() {
        return gridPlacedObjectSO.nameString + building.getLevel();
    }
}
