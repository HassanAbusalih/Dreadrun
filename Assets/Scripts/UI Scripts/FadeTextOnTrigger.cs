using UnityEngine;
using TMPro;

public class FadeTextOnTrigger : MonoBehaviour
{
    [SerializeField] TextMeshPro[] text;
    [SerializeField] float DelayDuration;
    [SerializeField] float fadeDuration;

    private Color[] initialColors = new Color[2];

    private bool fadeIn;
    private bool fadeOut;

    private float elapsedTime;
    private float targetAlpha;

    void Start()
    {
        InitializeTextColor();
    }

    void Update()
    {
        CheckWhetherToFadeOrNot();
    }

    void CheckWhetherToFadeOrNot()
    {
        if (fadeIn || fadeOut)
        {
            lerpTextColorTransparency();

            // If the fade has completed, stop fading
            if (elapsedTime >= fadeDuration)
            {
                fadeIn = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // If the trigger has been entered and the text is not already fading in, start fading in
        if (other.TryGetComponent(out Player player))
        {
            StartFadeIn();
        }
    }

    void OnTriggerExit(Collider other)
    {
        // If the trigger has been exited and the text is not already fading out, start fading out
        if (other.TryGetComponent(out Player player))
        {
            CancelInvoke(nameof(DelayToEnableFading));
            StartFadeOut();
        }
    }

    void StartFadeIn()
    {
        for (int i = 0; i < text.Length; i++)
        {
            fadeOut = false;
            Invoke(nameof(DelayToEnableFading), DelayDuration);
            initialColors[i] = text[i].color;

            elapsedTime = 0f;
            targetAlpha = 1f;
        }
    }

    void StartFadeOut()
    {
        for (int i = 0; i < text.Length; i++)
        {
            initialColors[i] = text[i].color;
            fadeIn = false;
            fadeOut = true;
            elapsedTime = 0f;
            targetAlpha = 0f;
        }
    }

    void lerpTextColorTransparency()
    {

        for (int i = 0; i < text.Length; i++)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;

            text[i].color = new Color(initialColors[i].r, initialColors[i].g, initialColors[i].b, Mathf.Lerp(initialColors[i].a, targetAlpha, t));
        }
    }

    void DelayToEnableFading()
    {
        fadeIn = true;
    }

    private void InitializeTextColor()
    {
        for (int i = 0; i < text.Length; i++)
        {
            text[i].color = new Color(text[i].color.r, text[i].color.g, text[i].color.b, 0);
            initialColors[i] = text[i].color;
        }
    }

    private void OnEnable()
    {
        LevelRotate.GiveLevelDirectionToPlayer += DetermineFaceDirection;
    }

    private void OnDisable()
    {
        LevelRotate.GiveLevelDirectionToPlayer -= DetermineFaceDirection;
    }

    void DetermineFaceDirection(Transform levelTransform, bool customDirectionEnabled)
    {
        LevelRotate levelRotate = GetComponentInParent<LevelRotate>();
        if (levelRotate != null && levelTransform.localScale.x < 0f)
        {
            foreach (TextMeshPro textMesh in text)
            {
                textMesh.transform.eulerAngles = -textMesh.transform.eulerAngles;
            }
        }
    }
}