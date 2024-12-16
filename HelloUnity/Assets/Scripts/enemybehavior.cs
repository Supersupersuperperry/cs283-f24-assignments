using UnityEngine;
using TMPro;
using System.Collections;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public float attackDistance = 2f;
    public int health = 15;
    public int damage = 1;
    public float attackCooldown = 1f;

    private Animator animator;
    private float lastAttackTime = 0f;
    private TextMeshProUGUI healthText;
    private Transform mainCamera;
    private GameObject canvasObject;
    public Transform sword; // Reference to the enemy's sword
    private bool isAttacking = false;

    public delegate void EnemyDefeated();
    public event EnemyDefeated onEnemyDefeated;

    void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main?.transform;

        canvasObject = new GameObject("EnemyCanvas");
        canvasObject.transform.SetParent(transform);
        canvasObject.transform.localPosition = Vector3.zero;

        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;

        GameObject textObject = new GameObject("HealthText");
        textObject.transform.SetParent(canvasObject.transform);
        textObject.transform.localPosition = new Vector3(0, 150f, 0);

        healthText = textObject.AddComponent<TextMeshProUGUI>();
        healthText.text = $"HP: {health}";
        healthText.fontSize = 20;
        healthText.color = Color.red;
        healthText.alignment = TextAlignmentOptions.Center;
    }

    void Update()
    {
        if (healthText != null && mainCamera != null)
        {
            healthText.transform.LookAt(mainCamera);
            healthText.transform.Rotate(0, 180, 0);
        }

        if (player == null || isAttacking) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > attackDistance)
        {
            MoveTowardsPlayer();
        }
        else
        {
            AttemptAttack();
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        if (animator)
        {
            animator.SetFloat("Speed", speed);
        }
    }

    void AttemptAttack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            PerformAttack();
            lastAttackTime = Time.time;
        }
    }

    void PerformAttack()
    {
        isAttacking = true;

        if (animator)
        {
            animator.SetTrigger("Attack");
        }

        if (sword)
        {
            StartCoroutine(RotateSword());
        }

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        Invoke(nameof(ResetAttack), 1f); // Adjust based on attack animation length
    }

    IEnumerator RotateSword()
    {
        float rotationTime = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < rotationTime)
        {
            sword.Rotate(0, 720 * Time.deltaTime, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        sword.localRotation = Quaternion.identity;
    }

    void ResetAttack()
    {
        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (healthText != null)
        {
            healthText.text = $"HP: {health}";
        }

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        onEnemyDefeated?.Invoke();

        if (canvasObject != null)
        {
            Destroy(canvasObject);
        }

        Destroy(gameObject);
    }
}
