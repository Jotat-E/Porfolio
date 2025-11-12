using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float dashSpeed = 12f;
    public float dashDuration = 0.15f;


    [Header("Dash+")]
    public bool hasDashPlus = false; 
    public float dashPlusDistance = 6f; 
    public float dashPlusInvulTime = 0.12f;
    public float dashPlusCooldown = 2.0f;


    [Header("Combat")]
    public Transform attackPoint;
    public float attackRange = 1.0f;
    public int baseAttackDamage = 10;
    public LayerMask enemyLayer;
    public float comboWindow = 0.5f;


    Rigidbody2D rb;
    Animator animator;
    Vector2 input;


    bool isDashing = false;
    float dashTimer = 0f;


    bool isAttacking = false;
    int comboStep = 0;
    float comboTimer = 0f;


    float dashPlusTimer = 0f;
    float dashPlusCooldownTimer = 0f;


    CharacterStats stats;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        stats = GetComponent<CharacterStats>();
        if (stats == null) Debug.LogWarning("Player missing CharacterStats");
    }


    void Update()
    {
        HandleInput();
        UpdateTimers();
        UpdateAnimator();
    }


    void FixedUpdate()
    {
        if (!isAttacking && !isDashing)
            Move();
    }


    void HandleInput()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;


        if (Input.GetButtonDown("Fire1"))
        {
            TryAttack();
        }


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            TryDash();
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            TryDashPlus();
        }
    }


    void Move()
    {
        Vector2 vel = input * moveSpeed * (stats != null ? stats.moveSpeedMultiplier : 1f);
        rb.velocity = vel;
    }


    void TryAttack()
    {
        if (isAttacking) 
        {
            if (comboTimer > 0f)
            {
                comboStep = Mathf.Clamp(comboStep + 1, 1, 3);
                comboTimer = comboWindow;
            }
            return;
        }


        StartCoroutine(DoAttack());
    }


    IEnumerator DoAttack()
    {
        isAttacking = true;
        comboStep = 1;
        comboTimer = comboWindow;
        animator.SetTrigger("Attack1");


        while (comboTimer > 0f || comboStep > 1)
        {
            if (comboStep >= 1)
            {
                HitEnemies(baseAttackDamage + (stats != null ? stats.GetAttackBonus() : 0));
            }


            yield return null;
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0f) break;
        }


        yield return new WaitForSeconds(0.05f);
        isAttacking = false;
        comboStep = 0;
    }

    void HitEnemies(int damage)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (var c in hits)
        {
            var enemy = c.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, transform);
            }
        }
    }


    void TryDash()
    {
        if (isDashing) return;
        StartCoroutine(DoDash());
    }


    IEnumerator DoDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        float start = Time.time;
        Vector2 dir = input.sqrMagnitude > 0.01f ? input : new Vector2(transform.localScale.x > 0 ? 1 : -1, 0);
        while (dashTimer > 0f)
        {
            rb.velocity = dir * dashSpeed;
            dashTimer -= Time.deltaTime;
            yield return null;
        }
        isDashing = false;
    }


    void TryDashPlus()
    {
        if (!hasDashPlus) return;
        if (dashPlusCooldownTimer > 0f) return;
        StartCoroutine(DoDashPlus());
    }

    IEnumerator DoDashPlus()
    {
        dashPlusCooldownTimer = dashPlusCooldown;

        var invul = GetComponent<Invulnerability>();
        if (invul != null) invul.SetInvulnerable(dashPlusInvulTime);


        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = (mouseWorld - transform.position);
        dir.z = 0;
        if (dir.magnitude > dashPlusDistance) dir = dir.normalized * dashPlusDistance;

        transform.position = (Vector3)transform.position + dir;


        yield return new WaitForSeconds(0.02f);
    }


    void UpdateTimers()
    {
        if (comboTimer > 0f) comboTimer -= Time.deltaTime;
        if (dashPlusCooldownTimer > 0f) dashPlusCooldownTimer -= Time.deltaTime;
    }

    void UpdateAnimator()
    {
        if (animator == null) return;
        animator.SetFloat("Speed", rb.velocity.magnitude);
        animator.SetBool("Dashing", isDashing);
    }


    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
