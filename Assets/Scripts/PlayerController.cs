using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float groundDist;

    public LayerMask terrainLayer;
    public Rigidbody rb;
    public SpriteRenderer sr;
    public Animator anim;
    public AudioSource audioSrc; // Make sure to assign the AudioSource in the Inspector

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
            }
        }

        // Player Movement
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 moveDir = new Vector3(x, 0, y).normalized;
        rb.velocity = moveDir * speed;

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

    private void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        // Ensure that only one component is active at a time to prevent diagonal movement
        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            y = 0f;
        }
        else
        {
            x = 0f;
        }

        Vector3 moveDir = new Vector3(x, 0, y);
        rb.velocity = moveDir.normalized * speed;
    }

    private void StopMoving()
    {
        rb.velocity = Vector3.zero;
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

}
