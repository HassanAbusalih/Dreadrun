using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PayloadFeedback : MonoBehaviour
{
    [SerializeField] GameObject healthUI;
    [SerializeField] Image healthBar;
    [SerializeField] Image distanceBar;
    [SerializeField] Renderer objectRenderer;
    Color stopColor = Color.red;
    Color moderateColor = Color.yellow;
    Color fastColor = Color.green;

    private void Start()
    {
        objectRenderer.material.color = stopColor;
    }


    private void Update()
    {
        LookAtCamera(healthUI);
    }

    public void ChangeColor(float speed, float maxSpeed)
    {
        float normalizedSpeed = Mathf.Clamp01(speed / maxSpeed);
        Color payloadColor = new();
        if (normalizedSpeed == 1)
        {
            payloadColor = fastColor;
        }
        else if (normalizedSpeed > 0 && normalizedSpeed < 1)
        {
            payloadColor = moderateColor;
        }
        else
        {
            payloadColor = stopColor;
        }
        //Color lerpedColor = Color.Lerp(stopColor, moderateColor, normalizedSpeed);
        //lerpedColor = Color.Lerp(lerpedColor, fastColor, normalizedSpeed);

        if (objectRenderer != null)
        {
            objectRenderer.material.color = payloadColor;
            //objectRenderer.material.color = lerpedColor;
        }

    }

    public void SetColor(Color color)
    {
        objectRenderer.material.color = color;
    }

    public void UpdateHealth(float health, float maxHealth)
    {
        healthBar.fillAmount = health / maxHealth;
    }

    private void LookAtCamera(GameObject gameObject)
    {
        Vector3 lookPos = Camera.main.transform.position - gameObject.transform.position;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        // gameObject.transform.rotation = rotation;
    }
}