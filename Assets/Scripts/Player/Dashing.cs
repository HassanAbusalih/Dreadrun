using System;
using System.Collections;
using System.Diagnostics.Contracts;
using UnityEngine;
using Cinemachine;

public class Dashing : MonoBehaviour
{
    Rigidbody rb;
    Player player;
    Color defaultColor;
    [SerializeField] MeshRenderer meshRenderer;
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

    [Header("Cinamachine Cameras")]
    [SerializeField] CinemachineVirtualCamera playerCamera;
    [SerializeField] CinemachineVirtualCamera dashCamera;

    public Action<float> onDashing;
    public delegate float canDash();
    public canDash canPlayerDash;

    Transform levelTransform;
    bool useCustomDirection;


    private void OnEnable()
    {
        player = GetComponent<Player>();
        if (player == null) return;
        player.canPLayerTakeDamage += GetPlayerInvincibility;
        LevelRotate.GiveLevelDirectionToPlayer += UpdateMovementInputDirection;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (meshRenderer != null)
            defaultColor = meshRenderer.material.color;
    }

    private void Update()
    {
        DashOnInput();
    }

    private void OnDisable()
    {
        if (player == null) return;
        player.canPLayerTakeDamage -= GetPlayerInvincibility;
        LevelRotate.GiveLevelDirectionToPlayer -= UpdateMovementInputDirection;
    }

    private void DashOnInput()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        Vector3 moveInput = Vector3.zero;

        if (!useCustomDirection)
        moveInput = cameraForward * Input.GetAxisRaw("Vertical") + cameraRight * Input.GetAxisRaw("Horizontal");
        else moveInput = levelTransform.forward * Input.GetAxisRaw("Vertical") + levelTransform.right * Input.GetAxisRaw("Horizontal");

        if (moveInput == Vector3.zero) return;

        if ((Input.GetKeyDown(dodge) || Input.GetKeyDown(KeyCode.JoystickButton0)) && !isDashing)
        {
            float currentStamina = canPlayerDash?.Invoke() ?? 0f;
            if (currentStamina <= 0) return;
            Vector3 dashDirection = (moveInput).normalized;
            StartCoroutine(StartDash(dashDirection));
        }
    }

    void UpdateMovementInputDirection(Transform customTransform, bool customDirectionEnabled)
    {
        useCustomDirection = customDirectionEnabled;
        levelTransform = customTransform;
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
            rb.AddForce(Vector3.up * 1.2f, ForceMode.Impulse);


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
        SwitchCameras(_enabled);
    }

    void EnableInvincibility(bool _enabled)
    {
        isInvincible = _enabled;
        if (meshRenderer != null)
            meshRenderer.material.color = _enabled ? dashColor : defaultColor;
    }

    void SwitchCameras(bool _enabled)
    {
        if (playerCamera == null || dashCamera == null) return;
        int playerCamePriority = _enabled ? 0 : 1;
        int dashCamPriority = _enabled ? 1 : 0;
        playerCamera.Priority = playerCamePriority;
        dashCamera.Priority = dashCamPriority;
    }

    bool GetPlayerInvincibility()
    {
        return isInvincible;
    }
}
