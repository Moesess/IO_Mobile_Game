using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour{
    int level;
    int currentBuildCost;

    public void setLevel(int level){
        this.level = level;
    }
    
    public int getLevel(){
        return level;
    }


}
