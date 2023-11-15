using UnityEngine;

public class DamageFeedback : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] Color takeDamageColor;
    [SerializeField] float takeDamageScale;
    [SerializeField] AnimationCurve damageCurve;
    [SerializeField] bool animateScale;

    bool isTakingDamage;
    float elapsedTime;
    Color startColor;
    MeshRenderer meshRenderer;
    float startZScale;


    private void OnEnable()
    {
        IDamagable.onDamageTaken += EnableTakeDamageEffects;
        startZScale = transform.localScale.z;
        TryGetComponent(out meshRenderer);
        if (meshRenderer == null) return;
        startColor = GetComponent<Renderer>().material.color;    
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
            float _localscaleZ = transform.localScale.z;
            _localscaleZ = Mathf.Lerp(takeDamageScale, startZScale, _lerpValue);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, _localscaleZ);
        }
        else { isTakingDamage = false; }
    }
}