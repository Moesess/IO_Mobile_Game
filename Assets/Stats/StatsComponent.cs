using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class StatsComponent : MonoBehaviour
{
    private StatSystem health = new StatSystem("Health",100,100);
    public float wait = 1f;
    public StatBar healthBar;
    void Start() 
    {
        healthBar.Setup(health);
        Debug.Log(health.GetStatName()+": "+health.GetCurrentAmount());
    }
}
