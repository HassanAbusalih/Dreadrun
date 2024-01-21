using UnityEngine;
using UnityEngine.UI;

public class PayloadFeedback : MonoBehaviour
{
    [SerializeField] GameObject healthUI;
    [SerializeField] Image healthBar;
    [SerializeField] Renderer objectRenderer;
    [SerializeField] TrailRenderer trailRenderer;
    Color stopColor = Color.red;
    Color moderateColor = Color.yellow;
    Color fastColor = Color.green;
    public PayloadState PayloadState { get; private set; }

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
            PayloadState = PayloadState.Normal;
        }
        else if (normalizedSpeed > 0 && normalizedSpeed < 1)
        {
            payloadColor = moderateColor;
            PayloadState = PayloadState.Contested;
        }
        else
        {
            payloadColor = stopColor;
            PayloadState = PayloadState.Stopped;
        }

        if (objectRenderer != null)
        {
            objectRenderer.material.color = payloadColor;
            trailRenderer.material.SetColor("_EmissionColor", payloadColor *4f);
            trailRenderer.material.color = payloadColor;
        }

    }

    public void SetColor(Color color)
    {
        objectRenderer.material.color = color;
        trailRenderer.material.SetColor("_EmissionColor", color * 4f);
        trailRenderer.material.color = color;
    }

    public void UpdateHealth(float health, float maxHealth)
    {
        healthBar.fillAmount = health / maxHealth;
    }

    private void LookAtCamera(GameObject gameObject)
    {
        Vector3 lookPos = Camera.main.transform.position - gameObject.transform.position;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
    }
}

public enum PayloadState 
{   
    Normal, 
    Stopped, 
    Contested 
}