using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce; 
    public float moveSpeed = 5f; 
    private Rigidbody2D rb;
    public bool isGrounded; 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Horizontal movement
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Ground collision
        if(other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        if (other.gameObject.CompareTag("Hazard"))
        {
            Debug.Log("Owie");
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        // Ground collision
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
