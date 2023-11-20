using System;
using System.Collections;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    Rigidbody rb;
    Player player;
    Color defaultColor;

    [Header("Dashing Settings")]
    [SerializeField] float dashForce;
    [SerializeField] float dashDuration;
    [SerializeField] AnimationCurve dashCurve;
    [SerializeField] float staminaCost;

    [SerializeField] Color dashColor;
    [SerializeField] KeyCode dodge = KeyCode.Space;

    [SerializeField] SoundSO dashSFX;
    AudioSource audioSource;

    [Header("Debug Info")]
    [SerializeField] bool isInvincible = false;
    [SerializeField] bool isDashing;

    public Action<float> onDashing;
    public delegate float canDash();
    public canDash canPlayerDash;


    private void OnEnable()
    {
        player = GetComponent<Player>();
        if (player == null) return;
        player.canPLayerTakeDamage += GetPlayerInvincibility;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        defaultColor = GetComponent<Renderer>().material.color;
    }

    private void Update()
    {
        DashOnInput();
    }

    private void OnDisable()
    {
        if (player == null) return;
        player.canPLayerTakeDamage -= GetPlayerInvincibility;
    }

    private void DashOnInput()
    {
        if ((Input.GetKeyDown(dodge) || Input.GetKeyDown(KeyCode.JoystickButton0))  && !isDashing)
        {
            float currentStamina = canPlayerDash?.Invoke() ?? 0f;
            if (currentStamina <= 0) return;
            Vector3 dashDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
            StartCoroutine(StartDash(dashDirection));
        }
    }

    IEnumerator StartDash(Vector3 dashDirection)
    {
        DashMode(true);
        float elapsedTime = 0f;
        onDashing?.Invoke(-staminaCost);
        dashSFX.PlaySound(0, AudioSourceType.Player);
        while (elapsedTime < dashDuration && isDashing)
        {
            float dashProgress = elapsedTime / dashDuration;
            Vector3 _dashForce = Vector3.Lerp(Vector3.zero, dashDirection * dashForce, dashCurve.Evaluate(dashProgress));
            rb.AddForce(_dashForce, ForceMode.Impulse);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        DashMode(false);
    }
    private void DashMode(bool _enabled)
    {
        isDashing = _enabled;
        rb.freezeRotation = _enabled;
        EnableInvincibility(_enabled);
        
    }

    void EnableInvincibility(bool _enabled)
    {
        isInvincible = _enabled;
        GetComponent<Renderer>().material.color = _enabled ? dashColor : defaultColor;
    }

    bool GetPlayerInvincibility()
    {
        return isInvincible;
    }
}
