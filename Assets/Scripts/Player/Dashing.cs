using System;
using System.Collections;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    Rigidbody rb;
    Player player;

    [Header("Dashing Settings")]
    [SerializeField] float dashDistance;
    [SerializeField] float dashDuration;
    [SerializeField] float staminaCost;
    [SerializeField] bool isDashing;
    [SerializeField] Color dashColor;
    [SerializeField] KeyCode dodge = KeyCode.Space;
    [SerializeField] LayerMask ground;

    bool isInvincible = false;
    Color defaultColor;

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

    bool GetPlayerInvincibility()
    {
        return isInvincible;
    }

    private void DashOnInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
        {
            float currentStamina = canPlayerDash?.Invoke() ?? 0f;
            if (currentStamina <= 0) return;
            Vector3 dashDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
            StartCoroutine(StartDash(dashDirection));
        }
    }

    IEnumerator StartDash(Vector3 dashDirection)
    {
        isDashing = true;
        rb.useGravity = false;
        rb.freezeRotation = true;
        Vector3 endPosition = transform.position + dashDirection * dashDistance;
        float elapsedTime = 0f;
        EnableInvincibility(true);
        onDashing?.Invoke(-staminaCost);

        while (elapsedTime < dashDuration && isDashing)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, endPosition, Time.deltaTime * (dashDistance / dashDuration));
            rb.MovePosition(newPosition);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        StopDash();
    }
    private void StopDash()
    {
        isDashing = false;
        rb.useGravity = true;
        rb.freezeRotation = false;
        EnableInvincibility(false);
    }

    void EnableInvincibility(bool _enabled)
    {
        isInvincible = _enabled;
        GetComponent<Renderer>().material.color = _enabled ? dashColor : defaultColor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDashing)
        {
            if (collision.gameObject.layer != ground)
            {
                StopDash();
            }
        }
    }
}
