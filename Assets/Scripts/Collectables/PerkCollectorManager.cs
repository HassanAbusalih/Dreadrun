using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.Animations;
using System;

public class PerkCollectorManager : MonoBehaviour
{
    [SerializeField]
    List<Perk> acquiredPerks = new List<Perk>();
    private Player player;

    public Sprite blank;

    public Image[] AbilityIcon;
    public TextMeshProUGUI[] cooldownText;
    public Image[] AbilityGreyout;

    public Animator[] AbilityVFX;
    public GameObject abilityDescription;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    public bool AcquireablePerk(Perk perk)
    {
        return (perk.unique && !acquiredPerks.Contains(perk)) || !perk.unique;
    }

    public void AcquirePerk(Perk perk)
    {
        INeedUI needUI = perk.ApplyPlayerBuffs(player);
        if (needUI != null)
        {
            for (int i = 0; i < AbilityIcon.Length && i < cooldownText.Length; i++)
            {
                if (AbilityIcon[i].sprite == blank)
                {
                    AbilityIcon[i].sprite = perk.icon;
                    float abilityCooldown = perk.FetchCooldown();
                    cooldownText[i].text = "";
                    AbilityGreyout[i].fillAmount = 0f;
                    if (AcquireablePerk(perk))
                    {
                        needUI.OnCoolDown += () => HandleCD(perk.FetchCooldown(), i);
                        break;
                    }
                }
            }
        }
        acquiredPerks.Add(perk);
    }

    private void OnTriggerEnter(Collider other)
    {
        ICollectable collectable = other.GetComponent<ICollectable>();
        if (collectable != null)
        {
            Perk perk = collectable.Collect() as Perk;

            if (perk != null && AcquireablePerk(perk))
            {
                AcquirePerk(perk);
                perk.ApplyPlayerBuffs(player);
                Destroy(other.gameObject);
            }
        }
    }
    private void HandleCD(float cooldown, int index)
    {
        StartCoroutine(StartCD(cooldown, index));
    }

    private IEnumerator StartCD(float cooldown, int index)
    {
        float timer = cooldown;
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            int roundedTimer = Mathf.FloorToInt(timer);
            cooldownText[index].text = roundedTimer.ToString();
            AbilityGreyout[index].fillAmount = timer / cooldown;
            AbilityVFX[index].SetBool("AbilityRefreshed", false);

            if (timer < 1)
            {
                cooldownText[index].text = "";
                AbilityVFX[index].SetBool("AbilityRefreshed", true);
            }
            yield return null;
        }
    }
}