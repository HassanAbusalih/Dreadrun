using System.Collections;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Teleport : MonoBehaviour
{
    [SerializeField] Transform teleportDestination;
    [SerializeField] float secondsToTeleport = 180f;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] GameObject teleportEffect;
    [SerializeField] float timeToShowEffect = 5f;
    //[SerializeField] Image blackScreen;
    //[SerializeField] float fadeTime = 1f;
    GameObject effectInstance;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
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
        yield return new WaitForSeconds(1f);
        Destroy(effectInstance);
    }

    IEnumerator Timer()
    {
        float timer = secondsToTeleport;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            timerText.text = "Exploration Time: " + Mathf.RoundToInt(timer);
            if (timer <= timeToShowEffect && !effectInstance.activeSelf)
            {
                effectInstance.SetActive(true);
            }
            yield return null;
        }
    }
    /*
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
    }*/
}