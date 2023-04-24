using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 20f;
    private float jumpForce = 12f;
    private bool isGrounded = false;
    private bool shouldJump = false;
    private Rigidbody rb;

    // Initialize components
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Do these all the time
    void Update()
    {
        ConstrainPlayer();
        CheckForJump();
    }

    // Do these all the time, but for physics
    void FixedUpdate()
    {
        HandleMovement();
        
        if (shouldJump && isGrounded)
            Jump();
    }

    void OnCollisionEnter(Collision other)
    {
        // Do different things depending on what the player collides with
        switch (other.collider.tag)
        {
            case "ground":
                isGrounded = true;
                break;
            
            case "power_up":
                transform.localScale = new Vector3(2f, 2f, 2f);
                Destroy(other.gameObject);
                Invoke("ResetScale", 4f);
                break;

            case "enemy":
                Destroy(gameObject);
                break;

            default:
                break;
        }
    }

    void OnCollisionExit(Collision other)
    {
        // Do different things when the collison exits with the player and another object
        switch (other.collider.tag)
        {
            case "ground":
                isGrounded = false;
                shouldJump = false;
                break;

            default:
                break;
        }
    }

    void HandleMovement()
    {
        // These will return a value of either -1, 0, or 1
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Add a force to the rigidbody based on the input
        Vector3 force = new Vector3(horizontalInput * speed * Time.deltaTime, 0f, verticalInput * speed * Time.deltaTime);
        rb.AddForce(force, ForceMode.Impulse);
    }

    void CheckForJump()
    {
        // If the space bar is pressed, the player can jump
        if (Input.GetButtonDown("Jump") && isGrounded)
            shouldJump = true;
    }

    void Jump()
    {
        // Add a force to the y position of the player
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void ConstrainPlayer()
    {
        // Don't let the player fall off the stage
        if (transform.position.y < -5f)
        {
            transform.position = new Vector3(0f, 1f, 0f);
            rb.velocity = Vector3.zero;
        }
    }

    void ResetScale()
    {
        // Resets the scale to <1, 1, 1>
        transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
