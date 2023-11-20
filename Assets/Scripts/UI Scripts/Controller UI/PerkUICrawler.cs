using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkUICrawler : MonoBehaviour
{
    int last;
    int first;
    [SerializeField] int current = 0;
    [SerializeField] bool pressed;
    [SerializeField] float timer;
    [SerializeField] KeyCode useItem;
    [SerializeField] PerkSelector perk;

    private void Start()
    {
        perk = FindObjectOfType<PerkSelector>();
        last = 2;
    }

    void Update()
    {
        float dpadVertical = Input.GetAxis("DPadVertical");

        if (dpadVertical == 1 && pressed == false)
        {
            pressed = true;
            StartCoroutine(Cooldown());
            if (last != current)
            {
                current++;
            }
            else
            {
                current = first;
            }
        }
        if (dpadVertical == -1 && pressed == false)
        {
            pressed = true;
            StartCoroutine(Cooldown());
            if (first != current)
            {
                current--;
            }
            else
            {
                current = last;
            }
        }
        if (Input.GetKeyDown(useItem))
        {
            perk.OnClick(current);
            Debug.Log(current);
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(timer);
        pressed = false;
    }
}