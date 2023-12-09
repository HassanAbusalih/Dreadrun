using System.Collections;
using UnityEngine;

public class DamageFeedback : MonoBehaviour
{
    [Header("Color Animation Settings")]
    [SerializeField] float duration;
    [SerializeField] Color takeDamageColor;
    [Header("scale Animation  Settings")]
    [SerializeField] float takeDamageScale;
    [SerializeField] AnimationCurve damageCurve;
    [SerializeField] bool animateScale;

    [Header("PauseTime settings")]
    [SerializeField] float pauseTimeDuration;
    [SerializeField] bool canPauseTime;

    bool isTakingDamage;
    float elapsedTime;
    Color startColor;
    Color[] startColors;
    [SerializeField] MeshRenderer meshRenderer;

    [SerializeField] bool givingMeAIDS = false;
    [SerializeField] MeshRenderer[] meshRenderers;
    float startZScale;
    float startXScale;

    float[] startZScales;
    float[] startXScales;


    private void OnEnable()
    {
        IDamagable.onDamageTaken += EnableTakeDamageEffects;
        if (meshRenderer == null && !givingMeAIDS)
        {
            TryGetComponent(out meshRenderer);
        }
        else if (givingMeAIDS)
        {
            meshRenderers = GetComponentsInChildren<MeshRenderer>();
            startZScales = new float[meshRenderers.Length];
            startXScales = new float[meshRenderers.Length];
        }
        if (meshRenderer == null && meshRenderers.Length == 0) return;
        if (givingMeAIDS)
        {
            startColors = new Color[meshRenderers.Length];
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                startColors[i] = meshRenderers[i].material.color;
                startZScales[i] = meshRenderers[i].transform.localScale.z;
                startXScales[i] = meshRenderers[i].transform.localScale.x;
            }
        }
        else
        {
            startZScale = transform.localScale.z;
            startXScale = transform.localScale.x;
            startColor = meshRenderer.material.color;
        }
    }

    private void OnDisable()
    {
        IDamagable.onDamageTaken -= EnableTakeDamageEffects;
    }

    void EnableTakeDamageEffects(GameObject _damageable)
    {
        if (_damageable == gameObject)
        {
            elapsedTime = 0;
            isTakingDamage = true;
            StartCoroutine(PauseTimeFeedback());
        }
    }

    private void Update()
    {
        ShowTakeDamageEffects();
    }

    void ShowTakeDamageEffects()
    {
        if (!isTakingDamage) return;
        if (meshRenderer == null && meshRenderers.Length == 0) return;
        if (duration >= elapsedTime)
        {
            elapsedTime += Time.deltaTime;
            float _lerpValue = damageCurve.Evaluate(elapsedTime / duration);
            if (givingMeAIDS)
            {
                for (int i = 0; i < meshRenderers.Length; i++)
                {
                    meshRenderers[i].material.color = Color.Lerp(takeDamageColor, startColors[i], _lerpValue);
                }
            }
            else
            {
                meshRenderer.material.color = Color.Lerp(takeDamageColor, startColor, _lerpValue);
            }

            if (!animateScale) return;
            if (givingMeAIDS)
            {
                for (int i = 0; i < meshRenderers.Length; i++)
                {
                    float _localScaleZ = Mathf.Lerp(takeDamageScale, startZScales[i], _lerpValue);
                    float _localScaleX = Mathf.Lerp(takeDamageScale, startXScales[i], _lerpValue);
                    meshRenderers[i].transform.localScale = new Vector3(_localScaleX, meshRenderers[i].transform.localScale.y, _localScaleZ);
                }
            }
            else
            {
                float _localScaleZ = Mathf.Lerp(takeDamageScale, startZScale, _lerpValue);
                float _localScaleX = Mathf.Lerp(takeDamageScale, startXScale, _lerpValue);
                transform.localScale = new Vector3(_localScaleX, transform.localScale.y, _localScaleZ);
            }
        }
        else { isTakingDamage = false; }
    }

    IEnumerator PauseTimeFeedback()
    {
        Time.timeScale = canPauseTime ? 0 : 1;
        yield return new WaitForSecondsRealtime(pauseTimeDuration);
        Time.timeScale = 1;
    }

}