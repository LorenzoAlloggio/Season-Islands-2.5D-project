using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainPlayerAttack : MonoBehaviour
{
    private GameObject attackArea = default;

    private bool attacking = false;

    private Animator animator;

    private float timeToAttack = 0.25f;
    private float timer = 0f;

    // Start is called before the first frame update

    private void Start()
    {
        animator = GetComponent<Animator>();
        attackArea = transform.parent.GetChild(1).gameObject;
    }

    void Update()
    {
        bool isMoving = animator.GetBool("isMoving");
        if (Input.GetKeyDown(KeyCode.Mouse0) && (!isMoving))
        {
            Attack();
            animator.SetBool("Attack", true);
        }
        else
        {
            // Reset the "Attack" parameter to false when the mouse button is not clicked
            animator.SetBool("Attack", false);
        }

        if (attacking)
        {
            timer += Time.deltaTime;
            if (timer > timeToAttack)
            {
                timer = 0;
                attacking = false;
                attackArea.SetActive(attacking);
            }
        }
    }

    private void Attack()
    {
        attacking = true;
        attackArea.SetActive(attacking);

        // Check the direction and mirror animation if facing left
        bool isFacingLeft = animator.GetBool("isFacingLeft");
        if (isFacingLeft)
        {
            MirrorAnimation();
        }
    }

    // Mirror the attack animation
    private void MirrorAnimation()
    {
        // Assuming the player's sprite is a child of the main player object
        Transform playerSprite = transform.Find("GFX");
        if (playerSprite != null)
        {
            Vector3 localScale = playerSprite.localScale;
            localScale.x *= -1f;
            playerSprite.localScale = localScale;
        }
    }
}
