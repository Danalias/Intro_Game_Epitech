using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField]
    private bool combatEnabled;
    [SerializeField]
    private float inputTimer, attack1Radius, attack1Damage;
    [SerializeField]
    private Transform attack1HitBoxPos;
    [SerializeField]
    private LayerMask WhatIsDamageable;
    [SerializeField]
    private bool gotInput, isAttacking;

    private float lastInputTime = Mathf.NegativeInfinity;

    private Animator anim;
    private PlayerController controller;

    public AudioSource audioSource;
    public AudioClip sound;

    private void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
        anim.SetBool("canAttack", combatEnabled);
    }

    private void Update()
    {
        CheckCombatInput();
        CheckAttack();
    }

    private void CheckCombatInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (combatEnabled)
            {
                gotInput = true;
                lastInputTime = Time.time;
            }
        }
    }

    private void CheckAttack()
    {
        if (gotInput)
        {
            if (!isAttacking)
            {
                gotInput = false;
                isAttacking = true;
                anim.SetBool("isAttacking", isAttacking);
                ChooseAttack();
            }
        }

        if (Time.time >= lastInputTime + inputTimer)
        {
            gotInput = false;
        }
    }

    private void ChooseAttack()
    {
        if (!controller.isGrounded) {
            audioSource.PlayOneShot(sound);
            anim.SetBool("airAttack", true);
        } else if (controller.isWalking) {
            audioSource.PlayOneShot(sound);
            anim.SetBool("runAttack", true);
        } else {
            audioSource.PlayOneShot(sound);
            anim.SetBool("idleAttack", true);
        }
    }

    private void CheckAttackBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitBoxPos.position, attack1Radius, WhatIsDamageable);

        foreach (Collider2D collider in detectedObjects)
        {
            if(!collider.isTrigger)
                collider.gameObject.SendMessage("Die", transform.position.x < collider.transform.position.x);
        }
    }

    private void FinishAttack()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);

        anim.SetBool("airAttack", false);
        anim.SetBool("runAttack", false);
        anim.SetBool("idleAttack", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);
    }
}
