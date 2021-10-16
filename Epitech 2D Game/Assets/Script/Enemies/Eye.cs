using UnityEngine;

public class Eye : MonoBehaviour
{
    public float speed;
    public float turnDelay;
    private float turnCooldown = 0;
    public float attackDelay;
    private float attackCooldown = 0;
    public float basicAttackRange;
    private Vector3 dashDirection;
    private Side side = Side.RIGHT;
    private Pattern pattern = Pattern.SEARCH;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite;
    private GameObject fieldOfView;
    private GameObject player;

    public AudioSource audioSource;

    public AudioClip attackSound;


    void Start()
    {
        if (side == Side.LEFT)
            gameObject.transform.eulerAngles = new Vector3(0,180,0);

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
                break;
            case Pattern.CHASE:
                EyeMove();
                if (attackCooldown <= 0 && CheckPlayerIsInRange())
                    SetBasicAttack();
                break;
            case Pattern.ATTACK:
                break;
            default:
                break;
        }

        PassAttackCooldown();
    }

    void TurnPattern()
    {
        turnCooldown += Time.deltaTime;

        if (turnCooldown > turnDelay) {
            Flip();
            turnCooldown = 0;
        }
    }

    void EyeMove()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        FacePlayer();
    }

    bool CheckPlayerIsInRange()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 eyePos = transform.position;

        return (Vector3.Distance(playerPos, eyePos) <= basicAttackRange);
    }

    void SetBasicAttack()
    {
        animator.SetTrigger("basicAttack");
        SwitchPattern(Pattern.ATTACK);
        dashDirection = (player.transform.position - transform.position).normalized;
        attackCooldown = attackDelay;

        FacePlayer();
    }

    void EyeBasicAttack()
    {
        audioSource.PlayOneShot(attackSound);
        rb.AddForce(new Vector2(dashDirection.x * 400, dashDirection.y * 400));   
    }

    void PassAttackCooldown()
    {
        if (attackCooldown > 0)
            attackCooldown -= Time.deltaTime;
    }


    void FacePlayer()
    {
        if ((side == Side.LEFT && transform.position.x < player.transform.position.x) ||
            (side == Side.RIGHT && transform.position.x > player.transform.position.x))
            Flip();
    }

    public void Die(bool impulseRight)
    {
        animator.SetTrigger("death");
        SwitchPattern(Pattern.DEAD);
        gameObject.layer = 10;
        foreach (Transform child in transform)
         {
                 child.gameObject.layer = 10;
         }
    }

    void DestroyWithDeath()
    {
        Destroy(gameObject);
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

    void StopRigidbody()
    {
        rb.velocity = Vector2.zero;
    }

    
    void SwitchPattern(Pattern newPattern)
    {
        if (newPattern == pattern)
            return;

        switch (pattern) {
            case Pattern.SEARCH:
                fieldOfView.SetActive(false);
                break;
            case Pattern.ATTACK:
                FacePlayer();
                break;
            default:
                break;
        }

        pattern = newPattern;

        switch (pattern) {
            case Pattern.SEARCH:
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
        Gizmos.DrawWireSphere(transform.position, basicAttackRange);
    }
}
