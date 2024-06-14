using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 1f;

    private Transform target;
    private int pathIndex = 0;
    private Vector2 direction;

    private float baseSpeed;

    private void Start()
    {
        baseSpeed = moveSpeed;
        target = LevelManager.main.path[pathIndex];
    }

    private void Update()
    {
        direction = (target.position - transform.position).normalized;

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed", direction.sqrMagnitude);

        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            if (pathIndex >= LevelManager.main.path.Length)
            {
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
            }
            else
            {
                target = LevelManager.main.path[pathIndex];
            }
        }
    }


    private void FixedUpdate()
    {
        rb.velocity = direction * moveSpeed;
    }

    public void UpdateSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    public void ResetSpeed()
    {
        if (TryGetComponent(out Health health))
        {
            float currentHitPoints = health.hitPoints;
            if (currentHitPoints <= 0)
            {
                moveSpeed = 0;
            }
            else
            {
                moveSpeed = baseSpeed;
            }
        }
    }
}