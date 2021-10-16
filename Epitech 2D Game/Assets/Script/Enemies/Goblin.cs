using UnityEngine;

public class Goblin : MonoBehaviour
{
    public float speed;
    public float turnDelay;
    private float turnCooldown = 0;
    private float walkDuration = 0;
    private Side side;
    private Pattern pattern = Pattern.SEARCH;
    public float basicAttackRange;
    public Transform attackPoint;
    public Transform raycastPoint;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite;
    private GameObject fieldOfView;

    private GameObject player;
    private Vector3 velocity = Vector3.zero;

    public AudioSource audioSource;

    public AudioClip attackSound;


    void Start()
    {
        if (gameObject.transform.eulerAngles.y == 0)
            side = Side.RIGHT;
        else
            side = Side.LEFT;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        fieldOfView = transform.Find("FieldOfView").gameObject;
        player = GameObject.Find("Player");
    }
    void FixedUpdate()
    {
        switch (pattern) {
            case Pattern.SEARCH:
                TurnPattern();
                RandomMovePattern();
                break;
            case Pattern.CHASE:
                if(CheckPlayerIsInRange())
                    SetBasicAttack();
                else if (!ThereIsGroundForward())
                    SwitchPattern(Pattern.SEARCH);
                else
                    GoblinMove();
                break;
            case Pattern.ATTACK:
                break;
            default:
                break;
        }
        
        animator.SetFloat("speed", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    void TurnPattern()
    {
        turnCooldown += Time.deltaTime;

        if (turnCooldown > turnDelay) {
            Flip();
            turnCooldown = 0;
            walkDuration = Random.Range(0, 3);
        }
    }

    void RandomMovePattern()
    {
        if (!ThereIsGroundForward())
            walkDuration = 0;

        if (walkDuration > 0)
        {
            walkDuration -= Time.deltaTime;
            GoblinMove();
        }
    }

    bool ThereIsGroundForward()
    {
        RaycastHit2D hit = Physics2D.Raycast(raycastPoint.position, Vector3.down, 0.3f, LayerMask.GetMask("Platforms"));
        Debug.DrawRay(raycastPoint.position, Vector3.down, Color.green, 0.1f);

        return(hit.collider != null);
    }

    void GoblinMove()
    {
        float horizontalMovement = (side == Side.LEFT ? -1 : 1) * speed * Time.deltaTime;
        Vector3 targetVelocity = new Vector2(horizontalMovement, rb.velocity.y);

        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);
    }

    bool CheckPlayerIsInRange()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 goblinPos = transform.position;

        return (Mathf.Abs(playerPos.x - goblinPos.x) <= 1.6 && Mathf.Abs(playerPos.y - goblinPos.y) <= 1);
    }

    void SetBasicAttack()
    {
        audioSource.PlayOneShot(attackSound);
        animator.SetTrigger("basicAttack");
        StopRigidbody();
        SwitchPattern(Pattern.ATTACK);
    }

    void GoblinBasicAttack()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(new Vector2(attackPoint.position.x, attackPoint.position.y), basicAttackRange, LayerMask.GetMask("Player"));

        foreach (Collider2D collider in hitPlayer) {
            collider.SendMessage("TakeDamage", transform.position.x < collider.transform.position.x);
        }
    }

    void Flip()
    {
        if (side == Side.LEFT) {
            gameObject.transform.eulerAngles = Vector3.zero;
            side = Side.RIGHT;
        } else {
            gameObject.transform.eulerAngles = new Vector3(0,180,0);
            side = Side.LEFT;
        }
    }

    public void Die(bool impulseRight)
    {
        animator.SetTrigger("death");
        SwitchPattern(Pattern.DEAD);
        gameObject.layer = 10;
        rb.velocity = Vector2.zero;

        GetComponent<SpriteRenderer>().flipX = !impulseRight;

        float force = 80;
        rb.AddForce(new Vector2(impulseRight ? force : -force, force), ForceMode2D.Impulse);
    }

    void DestroyWithDeath()
    {
        Destroy(gameObject);
    }

    void StopRigidbody()
    {
        rb.velocity = Vector2.zero;
    }

    void SwitchPattern(Pattern newPattern)
    {
        switch (pattern) {
            case Pattern.SEARCH:
                fieldOfView.SetActive(false);
                break;
            default:
                break;
        }

        pattern = newPattern;

        switch (pattern) {
            case Pattern.SEARCH:
                walkDuration = 0;
                StopRigidbody();
                fieldOfView.SetActive(true);
                break;
            case Pattern.ATTACK:
                StopRigidbody();
                break;
            default:
                break;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, basicAttackRange);
    }
}
