
using UnityEngine;

public class FogTrigger : MonoBehaviour
{
    [Header("Fog Particle Settings")]
    [SerializeField] Vector2 fogBoxScale;
    [SerializeField] float particleEmissionRate;
    [SerializeField] Color fogColor;

    [Header("Fog Trigger Settings")]
    [SerializeField] float delayToFadeOut;
    [SerializeField] float fadeOutDuration;
    [SerializeField] bool destroyAfterFadeOut;
    
    ParticleSystem fog;
    BoxCollider boxCollider;
    float delayToDestroy;
    float elapsedTime;
    bool startFading;
    
    void Start()
    {
        delayToDestroy = fadeOutDuration + 4f;
    }

    private void OnValidate()
    {
        fog = GetComponentInChildren<ParticleSystem>();
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;

        var fogParticleEmission = fog.emission;
        fogParticleEmission.rateOverTime = particleEmissionRate;

        boxCollider.size = new Vector3(fogBoxScale.x, 1, fogBoxScale.y);
        var fogParticleShape = fog.shape;
        fogParticleShape.scale = new Vector3(fogBoxScale.x, fogBoxScale.y, 0);

        var fogMain = fog.main;
        fogMain.startColor = fogColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            Invoke(nameof(EnableFogFadingOut), delayToFadeOut);
        }
    }

    void EnableFogFadingOut()
    {
        startFading = true;
    }

    private void Update()
    {
        FadeOutFogParticles();
    }

    void FadeOutFogParticles()
    {
        if (!startFading) return;
        if (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            var _emission = fog.emission;
            float _emissionRate = _emission.rateOverTime.constant;
            _emission.rateOverTime = Mathf.Lerp(_emissionRate, 0, elapsedTime / fadeOutDuration);
            return;
        }
        DestroyAfterFadingOut();
    }

    void DestroyAfterFadingOut()
    {
        startFading = false;
        if (!destroyAfterFadeOut)
        {
            fog.Stop();
            return;
        }
        Destroy(gameObject, delayToDestroy);
    }
}

