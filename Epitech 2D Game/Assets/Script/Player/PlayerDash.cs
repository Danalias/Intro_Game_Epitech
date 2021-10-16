using UnityEngine;
using System.Collections.Generic;

public class PlayerDash : MonoBehaviour
{
    private float dashLenght;
    public float lastImageXpos;

    private int facingDirection = 1;


    public bool isDashing = false;
    public bool canDash = true;

    
    private Vector3 startPoint = Vector3.zero;
    private Vector3 dir;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite;
    private Player player;

    private Queue<GameObject> enemyToKill = new Queue<GameObject>();

    public float dashSpeed;
    public float dashLenghtMax;
    public float slowMotionFactor;
    public float slowMotionDuration;
    public float slowMotionCooldown = 0;
    public float distanceBetweenImages;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        player = GetComponent<Player>();
    }

    void OnEnable()
    {
        rb.gravityScale = 0.0f;
        rb.velocity = Vector2.zero;
        slowMotionCooldown = 0;
        Time.timeScale = slowMotionFactor;
        Time.fixedDeltaTime = slowMotionFactor * 0.02f;
        transform.eulerAngles = Vector3.zero;
        StartDash();
        AnalysePath();
    }

    void Update()
    {
        if (!PauseMenu.isGamePaused) {
            CheckDash();
            CheckInput();
        }
    }

    void CheckInput()
    {
        if (canDash && !isDashing && Input.GetMouseButtonDown(0)) {
            StartDash();
            AnalysePath();
            ResetCooldown();
        }
    }

    void StartDash()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector3(mousePos.x, mousePos.y, 0);
        dir = (mousePos - transform.position).normalized;

        startPoint = transform.position;
        FLip(transform.position.x < mousePos.x ? 1 : -1);

        //animator.SetTrigger("special");
        int animationIndex = Random.Range(1, 4);
        while (animationIndex == animator.GetInteger("specialIndex"))
            animationIndex = Random.Range(1, 4);
        animator.SetInteger("specialIndex", animationIndex);

        isDashing = true;
        canDash = false;
        dashLenght = dashLenghtMax;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;

        //Debug.Log(mousePos + " / " + startPoint + " -> " + (mousePos - transform.position).normalized);
    }

    void AnalysePath()
    {
        RaycastHit2D hit = Physics2D.Raycast(startPoint, dir, dashLenghtMax + 1f, LayerMask.GetMask("Platforms"));

        //Collider2D[] hitCollider = Physics2D.OverlapCircleAll(startPoint, dashLenghtMax + 1f, LayerMask.GetMask("Platforms", "Enemy"));
        if (hit.collider != null) {
            dashLenght = Vector3.Distance(hit.point, startPoint) - 1f;
            if (dashLenght < 1.5f)
                StopDash();
        }

        RaycastHit2D[] enemyHitted = Physics2D.RaycastAll(startPoint, dir, dashLenghtMax + 1f, LayerMask.GetMask("Enemy"));

        foreach(RaycastHit2D enemyCollider in enemyHitted)
        {
            enemyToKill.Enqueue(enemyCollider.collider.gameObject);
            canDash = true;
        }
    }

    void CheckDash()
    {
        if (isDashing)
            Dash();
            
        slowMotionCooldown += Time.unscaledDeltaTime;

        if (slowMotionCooldown >= slowMotionDuration)
        {
            player.StopDash();
            while (enemyToKill.Count >= 1)
                enemyToKill.Dequeue().SendMessage("Die", Random.Range(0, 2) == 0 ? false : true);
        }
    }

    void Dash()
    {
        transform.Translate(new Vector2(dir.x * dashSpeed * facingDirection, dir.y * dashSpeed));

        if (Vector3.Distance(startPoint, transform.position) >= dashLenght) {
            StopDash();
            Debug.DrawRay(startPoint, transform.position - startPoint, Color.green, dashLenght);
        }

        if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
        {
            PlayerAfterImagePool.Instance.GetFromPool();
            lastImageXpos = transform.position.x;
        }

        //Debug.Log(startPoint + " / " + transform.position + " / " + dir + " = " + Vector3.Distance(startPoint, transform.position));
    }

    void StopDash()
    {
        isDashing = false;
        ResetCooldown();
    }

    void FLip(int direction)
    {
        facingDirection = direction;
        transform.eulerAngles = facingDirection == -1 ? new Vector3(0,180,0) : Vector3.zero;
    }

    void ResetCooldown() {slowMotionCooldown = 0;}

    void OnDisable()
    {
        rb.gravityScale = 4f;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        animator.SetInteger("specialIndex", 0);
    }
}
