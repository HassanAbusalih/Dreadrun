
using UnityEngine;


public class FogTrigger : MonoBehaviour
{
    [Header("Fog Particle Settings")]
    [SerializeField] Vector2 fogBoxScale;
    [SerializeField] float particleEmissionRate;

    [SerializeField] float particleLifetime;
    [SerializeField] Color fogColor;

    [Header("Fog Trigger Settings")]
    [SerializeField] float delayToFadeOut;
    [SerializeField] bool destroyAfterFadeOut;

    [Header("DONT CHANGE")]
    [SerializeField] float fadeOutDuration;
    [SerializeField] float sizeLerpSpeed;
   


    ParticleSystem fog;
    ParticleSystemRenderer fogRenderer;
    BoxCollider boxCollider;
    float delayToDestroy;
    float elapsedTime;
    bool startFading;

    void Start()
    {
        Initialize();
        delayToDestroy = fadeOutDuration + 4f;
        fog.time = 0.01f;
        fog.Play();
    }

    private void OnValidate()
    {
        Initialize();
    }

    void Initialize()
    {
        fog = GetComponentInChildren<ParticleSystem>();
        boxCollider = GetComponent<BoxCollider>();
        fogRenderer = fog.GetComponent<ParticleSystemRenderer>();
        boxCollider.isTrigger = true;

        var fogParticleEmission = fog.emission;
        fogParticleEmission.rateOverTime = particleEmissionRate;

        boxCollider.size = new Vector3(fogBoxScale.x, 5, fogBoxScale.y);
        var fogParticleShape = fog.shape;
        fogParticleShape.scale = new Vector3(fogBoxScale.x, fogBoxScale.y, 0);

        var lifeTime = fog.main;
        lifeTime.startLifetime = particleLifetime;

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

    private void FixedUpdate()
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
            var _lifetime = fog.main;
            var _color = fog.main;

            _lifetime.startLifetime = LeanTween.easeOutCubic(0, particleLifetime, elapsedTime / fadeOutDuration);
            _emission.rateOverTime = Mathf.Lerp(_emissionRate, 0, elapsedTime / fadeOutDuration);
            _color.startColor = Color.Lerp(_color.startColor.color, Color.clear, elapsedTime / fadeOutDuration);
            fogRenderer.maxParticleSize = Mathf.Lerp(fogRenderer.maxParticleSize, 0, elapsedTime / fadeOutDuration / sizeLerpSpeed);

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

