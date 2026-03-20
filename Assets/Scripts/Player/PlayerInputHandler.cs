using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 moveDirection { get; private set; }
    public bool isfiring { get; private set; }
    public bool isDashing { get; private set; }
    public bool isReloading { get; private set; }

    public void Move(InputAction.CallbackContext context)
    {
        moveDirection =  context.ReadValue<Vector2>();

    }

    public void OnFire(InputAction.CallbackContext context)
    {
        isfiring = context.performed;
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isReloading = true;
        }
    }

    public void ResetReloadTrigger()
    {
        isReloading = false;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isDashing = true;
        }
    }

    public void ResetDashTrigger()
    {
        isDashing = false;
    }
}
