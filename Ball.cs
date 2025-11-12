using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Ball : MonoBehaviour
{
    public int damage = 1;
    public float moneyPerHit = 1f;

    [Header("Spawn safety")]
    public float spawnIgnoreTime = 0.12f;

    [HideInInspector] public GameManager gameManager;
    private Rigidbody2D rb;
    private float spawnTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(GameManager gm, int initialDamage, float initialSpeed, float initialMoneyPerHit)
    {
        gameManager = gm;
        damage = initialDamage;
        moneyPerHit = initialMoneyPerHit;

        spawnTime = Time.time;

        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.velocity = Random.insideUnitCircle.normalized * initialSpeed;
    }

    public void SetSpeed(float newSpeed)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (rb.velocity != Vector2.zero)
            rb.velocity = rb.velocity.normalized * newSpeed;
        else
            rb.velocity = Random.insideUnitCircle.normalized * newSpeed;
    }

    public void SetDamage(int newDamage) { damage = newDamage; }
    public void SetMoneyPerHit(float m) { moneyPerHit = m; }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (Time.time - spawnTime < spawnIgnoreTime) return;

        Enemy e = collision.collider.GetComponent<Enemy>();
        if (e != null)
        {
            e.TakeDamage(damage);

            if (gameManager != null)
            {
                gameManager.AddDamage(damage);    
                gameManager.AddMoney(moneyPerHit);
            }
        }
    }

    void OnDestroy()
    {
        if (gameManager != null) gameManager.RemoveBall(this);
    }
}
