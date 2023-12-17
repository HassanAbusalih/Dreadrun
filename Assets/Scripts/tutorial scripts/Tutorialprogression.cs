using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorialprogression : MonoBehaviour
{
    [SerializeField] int conditions;
    [SerializeField] int maxConditions;
    [SerializeField] GameObject wall;

    // Update is called once per frame
    void Update()
    {
        if(conditions == maxConditions)
        {
            wall.SetActive(false);
        }
    }

    public void ConditionUpdate() 
    {
        conditions++;
    }
}