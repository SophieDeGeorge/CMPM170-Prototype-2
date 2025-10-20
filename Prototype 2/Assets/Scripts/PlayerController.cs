using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private bool facingRight = true;        // to track facing direction

    // NEW: flip only the visual body (e.g., Circle), not the root, so gun (Mushroom) can rotate freely
    [Header("Visual flip target (assign your Circle here)")]
    [SerializeField] private Transform body;
    [SerializeField] private Transform mushroom;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        bottomY = new(0, boxCollider.size.y * transform.localScale.y / 2 + 0.01f);
    }

    void OnMove(InputValue input)
    {
        moveX = input.Get<float>() * moveForce;
        
        // Flip player when pressing A or D
        // (Original intent kept as a comment; facing is now driven by mouse in Update()
        //  so we don't flip here to avoid fighting with mouse-facing.)
        // if (moveX > 0 && !facingRight) { Flip(); }
        // else if (moveX < 0 && facingRight) { Flip(); }
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
     void Update()
    {
        // NEW: Flip player body toward mouse side of the screen (auto face the cursor)
        FlipTowardMouse();
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(rb.position - bottomY, Vector2.down, distToJump);
    }

    // NEW: Face the mouse horizontally (right if mouse.x > player.x, else left)
    void FlipTowardMouse()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mouseWorld.x > transform.position.x && !facingRight)
        {
            Flip();
        }
        else if (mouseWorld.x < transform.position.x && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        // Flip only the visual "body"
        Vector3 bodyScale = body.localScale;
        bodyScale.x *= -1;
        body.localScale = bodyScale;

        // Compensate for mirrored parent so weapon rotation stays correct
        Vector3 weaponScale = mushroom.localScale;
        weaponScale.x *= -1;
        mushroom.localScale = weaponScale;
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


