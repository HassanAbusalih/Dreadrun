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

    bool isTakingDamage;
    float elapsedTime;
    Color startColor;
    [SerializeField] MeshRenderer meshRenderer;
    float startZScale;
    float startScaleX;


    private void OnEnable()
    {
        IDamagable.onDamageTaken += EnableTakeDamageEffects;
        startZScale = transform.localScale.z;
        startScaleX = transform.localScale.x;
        if (meshRenderer == null)
        {
            TryGetComponent(out meshRenderer);
        }
        if (meshRenderer == null) return;
        startColor = meshRenderer.material.color;
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
        }
    }

    private void Update()
    {
        ShowTakeDamageEffects();
    }

    void ShowTakeDamageEffects()
    {
        if (!isTakingDamage) return;
        if (meshRenderer == null) return;
        if (duration >= elapsedTime)
        {
            elapsedTime += Time.deltaTime;

            float _lerpValue = damageCurve.Evaluate(elapsedTime / duration);
            meshRenderer.material.color = Color.Lerp(takeDamageColor, startColor, _lerpValue);

            if (!animateScale) return;
            float _localScaleZ = transform.localScale.z;
            float _localScaleX = transform.localScale.x;

            _localScaleZ = Mathf.Lerp(takeDamageScale, startZScale, _lerpValue);
            _localScaleX = Mathf.Lerp(takeDamageScale, startZScale, _lerpValue);
            transform.localScale = new Vector3(_localScaleX, transform.localScale.y, _localScaleZ);
        }
        else { isTakingDamage = false; }
    }

}