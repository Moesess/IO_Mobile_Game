using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBar : MonoBehaviour
{
    private StatSystem statSystem;

    public void Setup(StatSystem statSystem)
    {
        this.statSystem = statSystem;
    }

    private void Update() 
    {
        gameObject.transform.Find("Bar").localScale = new Vector3(statSystem.GetAmountPercentage(),0.08f,1);
    }
}
