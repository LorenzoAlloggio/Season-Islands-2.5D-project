using System.Collections;
using UnityEngine;

public class MainPlayerAttack : MonoBehaviour
{
    private GameObject attackArea = default;
    private bool attacking = false;
    private Animator animator;
    private float timeToAttack = 0.25f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        Transform parentTransform = transform.parent;
        if (parentTransform != null && parentTransform.childCount > 1)
        {
            attackArea = parentTransform.GetChild(1).gameObject;
        }
    }

    void Update()
    {
        bool isMoving = animator.GetBool("isMoving");

        if (Input.GetMouseButtonDown(0) && (!isMoving))
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
            // Attack duration is now handled by coroutine
        }
    }

    private void Attack()
    {
        StartCoroutine(PerformAttack());

        // Check the direction and mirror animation if facing left
        bool isFacingLeft = animator.GetBool("isFacingLeft");
        if (isFacingLeft)
        {
            MirrorAnimation("SwordAttackRight");
        }
    }

    private IEnumerator PerformAttack()
    {
        attacking = true;
        attackArea.SetActive(attacking);

        yield return new WaitForSeconds(timeToAttack);

        attacking = false;
        attackArea.SetActive(attacking);
    }

    private void MirrorAnimation(string spriteName)
    {
        Transform playerSprite = transform.Find("GFX");

        if (playerSprite != null)
        {
            Transform specificSprite = playerSprite.Find(spriteName);

            if (specificSprite != null)
            {
                Vector3 localScale = specificSprite.localScale;
                localScale.x *= -1f;
                specificSprite.localScale = localScale;
            }
        }
    }
}
