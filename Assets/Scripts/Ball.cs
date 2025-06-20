using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float respawnDelay = 1f;

    [Header("Основные настройки")]
    public float startSpeed = 8f;
    public float minSpeed = 5f;
    public float maxSpeed = 20f;
    [Range(0.1f, 1f)] public float minVerticalFactor = 0.3f;

    [Header("Ускорение")]
    public float speedIncreasePerBounce = 0.5f;
    public float maxSpeedIncrease = 10f;
    public float angleChangeAfterMaxSpeed = 15f;

    private Rigidbody2D rb;
    private Vector2 lastFrameVelocity;
    private int bounceCount = 0;
    private float currentMaxSpeed;

    public GameManager manager;

    private int score1;
    private int score2;

    private int countupdate = 0;

    void Start()
    {
        score1 = 0; score2 = 0;
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        currentMaxSpeed = startSpeed + maxSpeedIncrease;
        LaunchBall();
    }

    void Update()
    {
        lastFrameVelocity = rb.linearVelocity;

        if (transform.position.x < -10f || transform.position.x > 10f)
        {
            if (transform.position.x < -10f)
            {
                if(countupdate == 0)
                {
                    score2++;
                    manager.UpdateScore(score1, score2);
                    StartCoroutine(RespawnWithDelay());
                    countupdate++;
                }
            }
            else if(transform.position.x > 10f)
            {
                if (countupdate == 0)
                {
                    score1++;
                    manager.UpdateScore(score1, score2);
                    StartCoroutine(RespawnWithDelay());
                    countupdate++;
                }
            }
        }

        if (Mathf.Abs(rb.linearVelocity.y) < startSpeed * minVerticalFactor)
        {
            PreventHorizontalTrajectory();
        }
    }

    void LaunchBall()
    {
        
        float angle = Random.Range(-45f, 45f);
        Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.left;
        rb.linearVelocity = direction.normalized * startSpeed;
    }

    void ResetBall()
    {
        transform.position = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
        bounceCount = 0;
        LaunchBall();
    }

    void PreventHorizontalTrajectory()
    {
        Vector2 newVelocity = rb.linearVelocity;
        float sign = Random.value > 0.5f ? 1f : -1f;
        newVelocity.y = sign * startSpeed * minVerticalFactor;
        rb.linearVelocity = newVelocity.normalized * Mathf.Clamp(lastFrameVelocity.magnitude, minSpeed, currentMaxSpeed);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.contacts[0];
        Vector2 normal = contact.normal;
        Vector2 incomingDirection = lastFrameVelocity.normalized;

        // Основное отражение
        Vector2 reflectedDirection = Vector2.Reflect(incomingDirection, normal);

        // Обработка BoxCollider
        if (collision.collider is BoxCollider2D)
        {
            reflectedDirection = HandleBoxColliderBounce(incomingDirection, normal, contact.point, collision.transform);
        }

        // Коррекция угла
        if (Mathf.Abs(reflectedDirection.y) < minVerticalFactor)
        {
            float sign = reflectedDirection.y >= 0 ? 1 : -1;
            reflectedDirection.y = sign * minVerticalFactor;
            reflectedDirection = reflectedDirection.normalized;
        }

        // Ускорение после каждого отскока
        bounceCount++;
        float speedMultiplier = 1f + (speedIncreasePerBounce * bounceCount * 0.1f);
        float newSpeed = Mathf.Clamp(lastFrameVelocity.magnitude * speedMultiplier, minSpeed, currentMaxSpeed);

        // Дополнительная рандомизация угла при достижении maxSpeed
        if (newSpeed >= currentMaxSpeed * 0.9f)
        {
            float randomAngle = Random.Range(-angleChangeAfterMaxSpeed, angleChangeAfterMaxSpeed);
            reflectedDirection = Quaternion.Euler(0, 0, randomAngle) * reflectedDirection;
        }

        rb.linearVelocity = reflectedDirection * newSpeed;
    }

    Vector2 HandleBoxColliderBounce(Vector2 incoming, Vector2 normal, Vector2 contactPoint, Transform boxTransform)
    {
        Vector2 localContact = boxTransform.InverseTransformPoint(contactPoint);
        float boxWidth = boxTransform.GetComponent<BoxCollider2D>().size.x * 0.5f;
        float boxHeight = boxTransform.GetComponent<BoxCollider2D>().size.y * 0.5f;

        if (Mathf.Abs(localContact.x) > boxWidth * 0.8f &&
            Mathf.Abs(localContact.y) > boxHeight * 0.8f)
        {
            Vector2 diagonalNormal = new Vector2(normal.x * 1.5f, normal.y * 1.5f).normalized;
            return Vector2.Reflect(incoming, diagonalNormal);
        }

        return Vector2.Reflect(incoming, normal);
    }
    IEnumerator RespawnWithDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        ResetBall();
        countupdate = 0;
    }
}
