using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [Header("Dashing Settings")]
    [SerializeField] float dashDistance;
    [SerializeField] float dashDuration;
    [SerializeField] bool isDashing;
    [SerializeField] bool isInvincible;
    [SerializeField] LayerMask ground;
    Color defaultColor;
    [SerializeField] Color dashColor;

    [SerializeField] KeyCode dodge = KeyCode.Space;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        defaultColor = GetComponent<Renderer>().material.color;
    }

    private void Update()
    {
        DashOnInput();
    }

    private void DashOnInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
        {
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
