using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float groundDist;
    public LayerMask terrainLayer;
    public Rigidbody rb;
    public SpriteRenderer sr;
    public Animator anim;
    public AudioSource audioSrc; // Make sure to assign the AudioSource in the Inspector

    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Ground Position Adjustment
        RaycastHit hit;
        Vector3 castPos = transform.position;
        castPos.y += 1;
        if (Physics.Raycast(castPos, -transform.up, out hit, Mathf.Infinity, terrainLayer))
        {
            if (hit.collider != null)
            {
                Vector3 movePos = transform.position;
                movePos.y = hit.point.y + groundDist;
                transform.position = movePos;
                isGrounded = true; // Update grounded state
            }
        }
        else
        {
            isGrounded = false; // Update grounded state
        }

        // Player Movement
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 moveDir = (transform.forward * y + transform.right * x).normalized;

        rb.velocity = new Vector3(moveDir.x * speed, rb.velocity.y, moveDir.z * speed); // Preserve vertical velocity

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // Update grounded state
        }

        // Animation Control
        if (x != 0 || y != 0)
        {
            anim.SetFloat("X", x);
            anim.SetFloat("Y", y);
            if (!anim.GetBool("isMoving"))
            {
                anim.SetBool("isMoving", true);
                PlayMovingSound(); // Call the method to play the sound
            }
        }
        else
        {
            if (anim.GetBool("isMoving"))
            {
                anim.SetBool("isMoving", false);
                StopMoving();
                StopMovingSound(); // Call the method to stop the sound
            }
        }
    }

    private void StopMoving()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0); // Stop horizontal movement
    }

    private void PlayMovingSound()
    {
        if (audioSrc != null && !audioSrc.isPlaying)
        {
            audioSrc.volume = 1f; // Set the volume to unmute
            audioSrc.enabled = true; // Enable the audio source
            audioSrc.Play(); // Play the audio
        }
    }

    private void StopMovingSound()
    {
        if (audioSrc != null && audioSrc.isPlaying)
        {
            audioSrc.Stop(); // Stop the audio
            audioSrc.enabled = false; // Disable the audio source
        }
    }

    public void RotateSpritesToCam()
    {
        Vector3 targetVector = Camera.main.transform.position - transform.position;

        float newYAngle = Mathf.Atan2(targetVector.z, targetVector.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, -1 * newYAngle, 0);
    }
}
