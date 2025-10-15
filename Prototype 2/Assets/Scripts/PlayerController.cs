using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private float moveX = 0f, moveY = 0f;   // forces on player in x and y

    const float distToJump = 0.1f;          // max distance from platform player can jump on it
    private Vector2 bottomY;                // adjustment used to get position of bottom of player

    [SerializeField] float moveForce = 5f;  // horizontal movement force
    [SerializeField] float jumpForce = 5f;  // vertical jump force

    private bool facingRight = true;        // to track facing direction

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        bottomY = new(0, boxCollider.size.y / 2 + 0.01f);
    }

    void OnMove(InputValue movementValue)
    {
        moveX = movementValue.Get<float>() * moveForce;

        // Flip player when pressing A or D
        if (moveX > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveX < 0 && facingRight)
        {
            Flip();
        }
    }

    void OnJump(InputValue jumpValue)
    {
        if (IsGrounded())
        {
            moveY = jumpValue.Get<float>() * jumpForce;
            rb.AddForce(new Vector2(0, moveY), ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector2(moveX, 0));  // movement handling
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(rb.position - bottomY, Vector2.down, distToJump);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;       // reverse X scale to flip
        transform.localScale = scale;
    }
}

