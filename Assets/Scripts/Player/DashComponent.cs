using System.Collections;
using UnityEngine;

public class DashComponent : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerRenderer;

    [SerializeField] private float dashForce;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    [SerializeField] private Color dashColor = new Color(1f, 1f, 1f, 0.5f);

    private Rigidbody2D rb;
    private PlayerInputHandler inputHandler;
    private Health playerHealth;

    private bool isDashing;
    private float nextDashTime;
    private Color playerOriginalColor;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputHandler = GetComponent<PlayerInputHandler>();
        playerHealth = GetComponent<Health>();
        if(playerRenderer) playerOriginalColor = playerRenderer.color;
    }

    private void Update()
    {
        if (inputHandler.isDashing)
        {
            inputHandler.ResetDashTrigger();
            if(Time.time > nextDashTime && !isDashing)
            {
                StartCoroutine(PerformDash());
            }
        }
    }

    private IEnumerator PerformDash()
    {
        isDashing = true;
        nextDashTime = Time.time + dashCooldown;

        AudioManager.Instance.PlaySFX("PlayerDash");

        Vector2 dashDirection = inputHandler.moveDirection.normalized;
        if (dashDirection == Vector2.zero) dashDirection = transform.right;

        playerHealth.SetInvincible(true);

        SetColor(dashColor);
        
        rb.linearVelocity = dashDirection * dashForce;
        yield return new WaitForSeconds(dashDuration);

        playerHealth.SetInvincible(false);
        isDashing = false;
        SetColor(playerOriginalColor);
    }

    private void SetColor(Color color)
    {
        if(playerRenderer) playerRenderer.color = color;
    }
}
