using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatSystem
{
    private string statName;
    private float currentAmount;
    private float maxAmount;

    public StatSystem(string statName)
    {
        this.statName = statName;
        this.maxAmount = 0;
        this.currentAmount = 0;
    }

    public StatSystem(string statName, float maxAmount, float currentAmount = 0)
    {
        this.statName = statName;
        this.maxAmount = maxAmount;
        this.currentAmount = currentAmount;
    }

    public void AddAmount(float value)
    {
        currentAmount += value;
        if(currentAmount > maxAmount) currentAmount = maxAmount;
    }

    public void SubstractAmount(float value)
    {
        currentAmount -= value;
        if(currentAmount < 0) currentAmount = 0;
    }

    public float GetAmountPercentage(){return (float)(currentAmount / maxAmount);}

    public string GetStatName(){return this.statName;}

    public float GetCurrentAmount(){return this.currentAmount;}

    public float GetMaxAmount(){return this.maxAmount;}

    public void SetStatName(string statName){this.statName = statName;}

    public void SetCurrentAmount(float currentAmount){this.currentAmount = currentAmount;}

    public void SetMaxAmount(float maxAmount){this.maxAmount = maxAmount;}

}
