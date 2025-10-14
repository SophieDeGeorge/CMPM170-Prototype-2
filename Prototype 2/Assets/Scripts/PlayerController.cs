using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
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
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        bottomY = new(0, boxCollider.size.y / 2 + 0.01f);
    }

    void OnMove(InputValue movementValue)
    {
        moveX = movementValue.Get<float>() * moveForce;
    }
    void OnJump(InputValue jumpValue) {
        //Debug.DrawRay(rb.position - bottomY, distToJump * Vector2.down, Color.red, 1000, true);   // uncomment to visualize jump raycast
        if (IsGrounded())
        {
            moveY = jumpValue.Get<float>() * jumpForce;
            rb.AddForce(new Vector2(0, moveY), ForceMode2D.Impulse);    // jump handling
        }
    }
    private void FixedUpdate()
    {
        rb.AddForce(new Vector2(moveX,0));  // movement handling
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(rb.position - bottomY, Vector2.down, distToJump);
    }
}
