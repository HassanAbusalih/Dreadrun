using System;
using System.Collections;

using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class Teleport : MonoBehaviour
{
    [SerializeField] Transform teleportDestination;
    [SerializeField] float secondsToTeleport = 180f;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] GameObject teleportEffect;
    [SerializeField] float timeToShowEffect = 5f;
    [SerializeField] Image blackScreen;
    [SerializeField] float fadeStartTime = 2f;
    [SerializeField] float fadeInTime = 0.5f;
    [SerializeField] float fadeOutTime = 1f;
    [SerializeField] float originalFogDensity = 0.01f; 
    [SerializeField] float finalFogDensity = 0.1f;
    [SerializeField] bool debugMode;
    //[SerializeField] float fogChangeTime= 10f; 

    bool triggered = false;
    GameObject effectInstance;
    public Action OnTimerOver;

    public static Action<Transform, bool> ChangePlayerDirectionToDefault;

    private void Start()
    {
        blackScreen.enabled = true;
        blackScreen.color = new UnityEngine.Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 1f);
        StartCoroutine(Fade(0f, 3f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (triggered) return;
            triggered = true;
            StartCoroutine(TeleportPlayer(player));
        }
    }

    IEnumerator TeleportPlayer(Player player)
    {
        timerText.gameObject.SetActive(true);
        effectInstance = Instantiate(teleportEffect, player.transform.position, Quaternion.identity, player.transform);
        effectInstance.SetActive(false);
        yield return Timer();
        player.transform.position = teleportDestination.position;
        timerText.gameObject.SetActive(false);
        StartCoroutine(FogChange(originalFogDensity, fadeOutTime + 1));
        yield return new WaitForSeconds(1f);
        FindObjectOfType<CameraFocus>().UpdateRotation(teleportDestination);
        yield return Fade(0f, fadeOutTime);
        Destroy(effectInstance);
        OnTimerOver?.Invoke();
        ChangePlayerDirectionToDefault?.Invoke(teleportDestination, false);
    }

    IEnumerator Timer()
    {
        float timer = secondsToTeleport;
        while (timer > 0)
        {
            DebugMode(ref timer);
            timer -= Time.deltaTime;
            timerText.text = Mathf.RoundToInt(timer).ToString();

            if(timer<11)
            {
                timerText.color = Color.red;
            }

            float time = 1f - (timer / secondsToTeleport);
            RenderSettings.fogDensity = Mathf.Lerp(originalFogDensity, finalFogDensity, time);

            if (timer <= timeToShowEffect && !effectInstance.activeSelf)
            {
                effectInstance.SetActive(true);
            }
            if (timer <= fadeStartTime)
            {
                StartCoroutine(Fade(1f, fadeInTime));
            }
            yield return null;
        }
    }

    IEnumerator Fade(float target, float time)
    {
        float timer = 0f;
        float start = blackScreen.color.a;
        while (timer < time)
        {
            timer += Time.deltaTime;
            float current = Mathf.Lerp(start, target, timer/time);
            blackScreen.color = new(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, current);
            yield return null;
        }
        blackScreen.color = new(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, target);
    }
    IEnumerator FadeOut(float time)
    {
        float timer = 0f;
        UnityEngine.Color startColor = blackScreen.color;

        while (timer < time)
        {
            timer += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startColor.a, 0f, timer / time);
            blackScreen.color = new UnityEngine.Color(startColor.r, startColor.g, startColor.b, currentAlpha);
            yield return null;
        }

        blackScreen.color = new UnityEngine.Color(startColor.r, startColor.g, startColor.b, 0f);
    }

    IEnumerator FogChange(float target, float time)
    {
        float timer = 0f;
        float start = RenderSettings.fogDensity;
        while (timer < time)
        {
            timer += Time.deltaTime;
            float current = Mathf.Lerp(start, target, timer / time);
            RenderSettings.fogDensity = current;
            yield return null;
        }
        RenderSettings.fogDensity = target;
    }


    void DebugMode( ref float timer)
    {
        if(debugMode && Input.GetKeyDown(KeyCode.I))
        {
            timer = 1f;
        }
    }
}