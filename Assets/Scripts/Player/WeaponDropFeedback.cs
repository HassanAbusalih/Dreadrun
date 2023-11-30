using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponDropFeedback : MonoBehaviour
{
    [SerializeField] TextMeshPro weaponDropText;
    [SerializeField] AnimationClip weaponDropAnimation;

    [SerializeField] Color startColor;
    [SerializeField] Color targetColor;
    [SerializeField] float startCharacterSpacing;
    [SerializeField] float targetCharacterSpacing;


    [Header("Debug Info")]
    [SerializeField] bool isDroppingWeapon;
    [SerializeField] bool weaponDropped;
    [SerializeField] float elapsedTime;

    Player player;
    Animator animator;

    private void OnEnable()
    {
        if(TryGetComponent(out player))
        {
            player.WeaponDropFeedback += WeaponDroppingAnimation;
            animator = GetComponent<Animator>();
        }
    }

    private void OnDisable()
    {
       if(player == null) return;
       player.WeaponDropFeedback -= WeaponDroppingAnimation;
    }

    private void Start()
    {
        if(weaponDropText == null) return;
        startCharacterSpacing = weaponDropText.characterSpacing;
        weaponDropText.color = startColor;
    }

    private void Update()
    {
        if(weaponDropText == null) return;
        if(!isDroppingWeapon && !weaponDropped)
        {
            LerpTextAndColor(startColor, startCharacterSpacing, Time.deltaTime *5f);
            animator.enabled = false;
        }
    }

    void  WeaponDroppingAnimation(float duration, bool isDropping)
    {
        if(weaponDropText == null) return;
        isDroppingWeapon = isDropping;
        if(elapsedTime< duration && isDroppingWeapon && !weaponDropped)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            weaponDropText.text = "DROPPING";
            LerpTextAndColor(targetColor, targetCharacterSpacing, t);
            weaponDropped = false;
            if (elapsedTime >= duration && isDroppingWeapon)
            {
                elapsedTime = 0f;
                weaponDropped = true;
                weaponDropText.text = "DROPPED";
                animator.enabled = true;
                animator.Play(weaponDropAnimation.name,-1,0);
            }
        }
        else elapsedTime = 0f;   
    }

    void LerpTextAndColor(Color targetColor, float targetSpacing, float Tvalue)
    {
        weaponDropText.color = Color.Lerp(weaponDropText.color, targetColor, Tvalue);
        weaponDropText.characterSpacing = Mathf.Lerp(weaponDropText.characterSpacing, targetSpacing,Tvalue);
    }
}
