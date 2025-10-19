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
    private float moveX = 0f;   // force on player in x
    private Vector2 bottomY;    // adjustment used to get position of bottom of player

    [Header("Jump Validation Distances")]
    [SerializeField] float distToJump = 0.1f;          // max distance from platform player can jump on it
    [SerializeField] float distToJumpPad = 0.5f;       // max distance from platform player can use jump pad

    [Header("Player Forces")]
    [SerializeField] float moveForce = 5f;      // horizontal movement force
    [SerializeField] float jumpForce = 5f;      // vertical jump force
    [SerializeField] float jumpPadForce = 7f;   // vertical jump pad force
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        bottomY = new(0, boxCollider.size.y * transform.localScale.y / 2 + 0.01f);
    }

    void OnMove(InputValue input)
    {
        moveX = input.Get<float>() * moveForce;
    }
    void OnJump(InputValue input) {
        //Debug.DrawRay(rb.position - bottomY, distToJump * Vector2.down, Color.red, 1000, true);   // uncomment to visualize jump raycast
        if (IsGrounded())
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);    // jump handling
        }
    }
    void OnAction(InputValue input)
    {
        //Debug.DrawRay(rb.position - bottomY, distToJumpPad * Vector2.down, Color.red, 1000, true);
        if (CanUseJumpPad() && !IsGrounded())
        {
            rb.velocity = new(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpPadForce), ForceMode2D.Impulse);
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

    bool CanUseJumpPad()
    {
        return Physics2D.Raycast(rb.position - bottomY, Vector2.down, distToJumpPad);
    }
}
