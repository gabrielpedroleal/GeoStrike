using UnityEngine;
using System.Collections;
public enum EnemyType {Melee, Ranged}
public enum BulletEffect { None, Fire, Electric }
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private EnemyType type;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float stopDistance;
    [SerializeField] private float attackRange;

    [SerializeField] private float separationRadius = 0.8f;
    [SerializeField] private float separationWeight = 1.5f;
    [SerializeField] private LayerMask enemyLayer;

    private Transform player;
    private Rigidbody2D rb;
    private WeaponComponent weapon;
    private bool isParalyzed = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        weapon = GetComponentInChildren<WeaponComponent>();
    }

    private void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    private void Update()
    {
        if (player == null || isParalyzed) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (type == EnemyType.Ranged && distanceToPlayer <= attackRange)
        {
            if (weapon != null) weapon.TryEnemyShoot();
        }
    }

    private void FixedUpdate()
    {
        if (player == null || isParalyzed)
        {
            if (isParalyzed) rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 playerPos = player.position;
        Vector2 currentPos = transform.position;

        Vector2 directionToPlayer = (playerPos - currentPos).normalized;
        float distanceToPlayer = Vector2.Distance(currentPos, playerPos);

        Vector2 separation = CalculateSeparation();

        Vector2 moveDirection = Vector2.zero;

        if (distanceToPlayer > stopDistance)
        {
            moveDirection = (directionToPlayer + (separation * separationWeight)).normalized;
            rb.linearVelocity = moveDirection * moveSpeed;
        }
        else
        {
            rb.linearVelocity = separation * moveSpeed;
        }

        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }
    private Vector2 CalculateSeparation()
    {
        Vector2 separationForce = Vector2.zero;
        Collider2D[] neighbors = Physics2D.OverlapCircleAll(transform.position, separationRadius, enemyLayer);
        int neighborCount = 0;

        foreach(var neighbor in neighbors)
        {
            if (neighbor.gameObject == gameObject) continue;

            Vector2 diff = (Vector2)transform.position - (Vector2)neighbor.transform.position;
            float distance = diff.magnitude;

            if(distance < separationRadius && distance > 0)
            {
                separationForce += diff.normalized / distance;
                neighborCount ++;
            }
        }
        return separationForce;
    }

    public void SetParalyzed(bool state)
    {
        isParalyzed = state;
        if (isParalyzed && rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
