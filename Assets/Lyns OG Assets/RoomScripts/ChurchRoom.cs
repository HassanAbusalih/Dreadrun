using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChurchRoom : MonoBehaviour
{
    public GameObject[] enemiesArray; 
    public GameObject reward;
    public GameObject succeedVFX;
    public GameObject failVFX;

    private bool isToggling = false;
    private float toggleTime = 1.0f;
    private float pressTime = 0.0f;
    private bool isPlayerInside = false;

    public Image barFill;
    void Update()
    {
        if (isPlayerInside && Input.GetKey(KeyCode.E))
        {
            
            pressTime += Time.deltaTime;
            barFill.fillAmount = pressTime / toggleTime;
            if (pressTime >= toggleTime && !isToggling)
            {
                FaithDecider();
                isToggling = true;
            }
        }
        else
        {
            barFill.fillAmount = 0;
            pressTime = 0.0f;
            isToggling = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isPlayerInside = other.TryGetComponent(out Player player);
    }

    void FaithDecider()
    {
        float randomValue = Random.value;

        if (randomValue < 0.5f)
        {
            EnableGameobjects(enemiesArray, true);
            ToggleObjects(reward, failVFX, false);
            Destroy(this.gameObject);
        }
        else
        {
            EnableGameobjects(enemiesArray, false);
            ToggleObjects(reward, succeedVFX, true);
            Destroy(this.gameObject);
        }
    }

    void EnableGameobjects(GameObject[] objects, bool enable)
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(enable);
        }
    }

    void ToggleObjects(GameObject obj, GameObject VFX, bool enable)
    {
        obj.SetActive(enable);
        VFX.SetActive(true);
    }

}

