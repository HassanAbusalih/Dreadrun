using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PerkSelector : MonoBehaviour
{
    [SerializeField]
    private Perk[] perkPool;
  
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void RandomPerkSelector(int perkSelection) //gets three of em and puts them into the perkselection array
    {
        Perk[] selectedPerks = new Perk[perkSelection];

        for (int perkIndex = 0; perkIndex < perkSelection; perkIndex++)
        {
            int randomindexNum = UnityEngine.Random.Range(0, perkPool.Length);
            selectedPerks[perkIndex] = perkPool[randomindexNum];
        }
    }
}
