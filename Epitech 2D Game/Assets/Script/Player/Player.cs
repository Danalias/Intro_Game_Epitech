using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform attackPoint;

    public float recoveryDuration;
    private float recoveryCooldown = 0;
    private float lastBlink;

    public int nbLife;
    public int nbSpecial;
    public float attackRange;
    public bool allowDash;
    private bool dashActive = false;
    private bool recoveryActive = false;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sp;
    private PlayerController playerController;
    private PlayerDash playerDash;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
        playerDash = GetComponent<PlayerDash>();
    }

    void Update()
    {
        CheckInput();
        CheckRecovery();

        if (animator.GetBool("isDead"))
            animator.SetFloat("yVelocity", rb.velocity.y);
    }

    void CheckInput()
    {
        //Debug.Log(animator.GetBool("isDead"));

       if (!PauseMenu.isGamePaused) {
            if (allowDash && nbSpecial > 0 && !dashActive && Input.GetMouseButtonDown(0) && !animator.GetBool("isDead"))
                StartDash();
        }
    }

    void CheckRecovery()
    {
        if (recoveryCooldown > 0)
        {
            recoveryCooldown -= Time.deltaTime;
            if (Time.time >= lastBlink + 0.1) {
                sp.enabled = !sp.enabled;
                lastBlink = Time.time;
            }
        }
        else if (recoveryActive)
            StopRecovery();
    }

    void StartDash()
    {
        dashActive = true;
        playerDash.enabled = true;
        playerController.enabled = false;
        nbSpecial--;
    }

    public void StopDash()
    {
        dashActive = false;
        playerDash.enabled = false;
        playerController.enabled = true;
    }

    public void StartRecovery()
    {
        recoveryActive = true;
        recoveryCooldown = recoveryDuration;
        lastBlink = Time.time;
    }

    public void StopRecovery()
    {
        recoveryActive = false;
        sp.enabled = true;
    }

    public void TakeDamage(bool impulseRight)
    {
        if (dashActive || recoveryActive)
            return;

        nbLife--;

        if (nbLife > 0)
            StartRecovery();
        else
            Die(impulseRight);
    }
    public void Die(bool impulseRight)
    {
        animator.SetTrigger("death");
        animator.SetBool("isDead", true);
        gameObject.layer = 10;
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(impulseRight ? 10 : -10, 10), ForceMode2D.Impulse);
        playerController.enabled = false;
        rb.sharedMaterial = null;
    }

    void LoseWithDeath()
    {
        animator.SetBool("isDead", false);
        GameObject.Find("GameCanva").SendMessage("ActivateDeathMenu");
    }

    void AddHeart() { if (nbLife < 3) nbLife +=1; }

    void AddSword() { if (nbSpecial < 3) nbSpecial +=1; }

    void StopBool(string boolName)
    {
        animator.SetBool(boolName, false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
