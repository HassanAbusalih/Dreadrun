using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveRoom : MonoBehaviour
{
    public GameObject[] enemiesArray;
    public GameObject[] rewardsArray;
    public GameObject succeedVFX;
    public GameObject failVFX;

    private bool isToggling = false;
    private float toggleTime = 2.0f;
    private float pressTime = 0.0f;
    private bool isPlayerInside = false;

    [SerializeField]
    [Range(0f, 1f)]
    float loseChance;

    public Image barFill;

    void Update()
    {
        if (isPlayerInside && Input.GetKey(KeyCode.F))
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

    private void OnTriggerExit(Collider other)
    {
        isPlayerInside = !other.TryGetComponent(out Player player);
    }

    void FaithDecider()
    {
        float randomValue = Random.value;

        if (randomValue < loseChance)
        {
            ToggleObjects(enemiesArray, true);
            ToggleObjects(rewardsArray, false);
            failVFX.SetActive(true);
            Destroy(this.gameObject);
        }
        else
        {
            ToggleObjects(enemiesArray, false);
            ToggleObjects(rewardsArray, true);
            succeedVFX.SetActive(true);
            Destroy(this.gameObject);
        }
    }

    void ToggleObjects(GameObject[] objects, bool enable)
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(enable);
        }
    }
}

