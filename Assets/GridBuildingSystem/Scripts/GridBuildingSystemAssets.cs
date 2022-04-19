using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildingSystemAssets : MonoBehaviour{
    public static GridBuildingSystemAssets Instance {get; private set;}

    private void Awake(){
        Instance = this;
    }

    public GridPlacedObjectSO[] gridPlacedObjectSOArray;

    public GridPlacedObjectSO house;
    public GridPlacedObjectSO mansion;
    public GridPlacedObjectSO castle;

    public GridPlacedObjectSO GetGridPlacedObjectTypeSOFromName(string placedObjectTypeSOName) {
        foreach (GridPlacedObjectSO gridPlacedObjectSO in gridPlacedObjectSOArray) {
            if (gridPlacedObjectSO.name == placedObjectTypeSOName) {
                return gridPlacedObjectSO;
            }
        }
        return null;
    }
}
