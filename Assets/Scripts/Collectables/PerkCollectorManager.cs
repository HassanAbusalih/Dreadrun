using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
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
    [SerializeField] TextMeshProUGUI[] abilityKeybinds;
    Perk[] UIPerks = new Perk[3];

    public Animator[] AbilityVFX;
    public GameObject abilityDescription;
    public Action<Perk, Color> UpdateEquippedPerkUi;
    PerkSelector perkSelector;

    private void Start()
    {
        perkSelector = FindObjectOfType<PerkSelector>();
        player = FindObjectOfType<Player>();
    }
    public bool AcquireablePerk(Perk perk)
    {
        return (perk.unique && !acquiredPerks.Contains(perk)) || !perk.unique;
    }

    public void AcquirePerk(Perk perk)
    {
        INeedUI needUI = perk.ApplyPlayerBuffs(player);

        Color perkColor;
        switch (perk.perkRarity)
        {
            case PerkRarity.Legendary:
                perkColor = perkSelector.LegendaryColor;
                break;
            case PerkRarity.Rare:
                perkColor = perkSelector.RareColor;
                break;
            default:
                perkColor = Color.white;
                break;
        }
        UpdateEquippedPerkUi?.Invoke(perk, perkColor);

        if (needUI != null)
        {
            for (int i = 0; i < AbilityIcon.Length && i < cooldownText.Length; i++)
            {
                if (AbilityIcon[i] != null && AbilityIcon[i].sprite == blank)
                {
                    AbilityIcon[i].sprite = perk.icon;
                    float abilityCooldown = perk.FetchCooldown();
                    cooldownText[i].text = "";
                    abilityKeybinds[i].text = needUI.Keybind;
                    AbilityGreyout[i].fillAmount = 0f;
                    UIPerks[i] = perk;
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
        bool animating = false;
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            int roundedTimer = Mathf.FloorToInt(timer);
            if (cooldownText[index] != null)
            {
                cooldownText[index].text = roundedTimer > 0 ? roundedTimer.ToString() : "";
                AbilityGreyout[index].fillAmount = timer / cooldown;
                AbilityVFX[index].SetBool("AbilityRefreshed", timer < 0.5f);
                if (AbilityVFX[index].GetBool("AbilityRefreshed") && !animating)
                {
                    animating = true;
                    StartCoroutine(ScaleUpThenDown(0.5f, AbilityIcon[index].transform.parent));
                }
            }
            yield return null;
        }
    }

    IEnumerator ScaleUpThenDown(float duration, Transform iconTransform)
    {
        Vector3 startScale = iconTransform.localScale;
        Vector3 bigScale = iconTransform.localScale * 1.15f;
        yield return StartCoroutine(PerkScaling(iconTransform, duration / 2, iconTransform.localScale, bigScale));
        yield return StartCoroutine(PerkScaling(iconTransform, duration / 2, iconTransform.localScale, startScale));
    }

    IEnumerator PerkScaling(Transform iconTransform, float duration, Vector3 startScale, Vector3 endScale)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            iconTransform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / duration);
            yield return null;
        }
        iconTransform.localScale = endScale;
    }

    public void ShowDescription(int index)
    {
        if (index >= 0 && index < 3 && UIPerks[index] != null)
        {
            abilityDescription.SetActive(true);
            abilityDescription.GetComponentInChildren<TextMeshProUGUI>().text = UIPerks[index].description;
        }
    }

    public void HideDescription()
    {
        abilityDescription.SetActive(false);
    }
}