using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    [SerializeField] public int stepCount;
    [SerializeField] private float timeGameplay;

    [SerializeField] private int condition1, condition2, condition3;
    [SerializeField] private int star;

    [SerializeField] private bool nullStar;

    private void Awake()
    {
        Instance = this;
        nullStar = false;
    }

    void Update()
    {
        IncreaseTime();
        CheckCondition();
        NullStar();
    }

    public void AddStep()
    {
        stepCount++;
    }

    public int getStepCount()
    {
        return stepCount;
    }

    public float getTimeGameplay()
    {
        return timeGameplay;
    }

    public void IncreaseTime()
    {
        timeGameplay += (Time.deltaTime * 1000);
    }

    public void CheckCondition()
    {
        if (stepCount <= condition1) star = 3;
        else if (condition1 < stepCount && stepCount <= condition2) star = 2;
        else if (condition2 < stepCount && stepCount <= condition3) star = 1;
        else star = 0;
    }

    public int getStar()
    {
        return star;
    }

    public bool IsNull()
    {
        return (getStar() == 0) ? true : false;
    }

    public void NullStar()
    {
        if (nullStar) this.star = 0;
    }

    public void setNullStar(bool set)
    {
        this.nullStar = set;
    }

}
