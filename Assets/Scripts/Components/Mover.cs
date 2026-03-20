using UnityEngine;

public class MoverComponent : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb;
    private PlayerInputHandler playerInputHandler;


  
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInputHandler = GetComponent<PlayerInputHandler>();
    }


    void FixedUpdate()
    {
        Vector2 direction = playerInputHandler.moveDirection;
        rb.linearVelocity = direction.normalized * moveSpeed;
    }
}
