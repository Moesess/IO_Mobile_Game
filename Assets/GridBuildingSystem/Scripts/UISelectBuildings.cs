using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class UISelectBuildings : MonoBehaviour{

    private Dictionary<GridPlacedObjectSO, Transform> gridPlacedObjectTransformDic;
    private bool active;

    private void Awake() {
        gridPlacedObjectTransformDic = new Dictionary<GridPlacedObjectSO, Transform>();
        gridPlacedObjectTransformDic[GridBuildingSystemAssets.Instance.house] = transform.Find("HouseBTN");
        gridPlacedObjectTransformDic[GridBuildingSystemAssets.Instance.mansion] = transform.Find("MansionBTN");

        this.active = false;

        foreach (GridPlacedObjectSO gridPlacedObjectSO in gridPlacedObjectTransformDic.Keys) {
            gridPlacedObjectTransformDic[gridPlacedObjectSO].GetComponent<Button_UI>().ClickFunc = () => {
                GridBuildingSystem.Instance.SelectPlacedObjectSO(gridPlacedObjectSO);
            };

            // gridPlacedObjectTransformDic[gridPlacedObjectSO].gameObject.SetActive(active);
        }

        // transform.Find("BuildPanel").gameObject.SetActive(active);
    }

    private void Start() {
        GridBuildingSystem.Instance.OnSelectedChanged += GridBuildingSystem_OnSelectedChanged;

        RefreshSelectedVisual();
    }

    private void GridBuildingSystem_OnSelectedChanged(object sender, System.EventArgs e){
        RefreshSelectedVisual();
    }

    private void RefreshSelectedVisual(){
        foreach (GridPlacedObjectSO gridPlacedObjectSO in gridPlacedObjectTransformDic.Keys) {
            gridPlacedObjectTransformDic[gridPlacedObjectSO].Find("Background").gameObject.SetActive(
                GridBuildingSystem.Instance.GetGridPlacedObjectSO() == gridPlacedObjectSO
            );
        }
    }

    public void ToggleBuildingMenu(){
        active = !active;
        GameObject backgroundPanel = transform.Find("BuildPanel").gameObject;

        if(active)
            backgroundPanel.GetComponent<Animation>().Play("Rise");
        else 
            backgroundPanel.GetComponent<Animation>().Play("Down");


        // foreach (GridPlacedObjectSO gridPlacedObjectSO in gridPlacedObjectTransformDic.Keys) {
        //     gridPlacedObjectTransformDic[gridPlacedObjectSO].gameObject.SetActive(active);
        // }

        // backgroundPanel.SetActive(active);
    }
}


