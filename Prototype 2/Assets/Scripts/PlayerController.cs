using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private float moveX = 0f;               // force on player in x
    private Vector2 bottomY;                // adjustment used to get position of bottom of player

    private string action = "jump";
    private JumpPad jumpPadScript;
    private SporeShooter sporeShooterScript;
    private PlayerAim playerAimScript;

    [Header("Jump Validation Distance")]
    [SerializeField] float distToJump = 0.1f;          // max distance from platform player can jump on it

    [Header("Player Forces")]
    [SerializeField] float moveForce = 5f;      // horizontal movement force
    [SerializeField] float jumpForce = 5f;      // vertical jump force

    [SerializeField] private MushroomMeter mushroomMeter;
    [SerializeField] private float actionCost = 20f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        bottomY = new(0, boxCollider.size.y * transform.localScale.y / 2 + 0.01f);
        jumpPadScript = GetComponent<JumpPad>();
        sporeShooterScript = GetComponent<SporeShooter>();
        playerAimScript = GetComponent<PlayerAim>();
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
        Debug.Log("action fired");
        switch (action) {
            case "shoot":
                Debug.Log("shoot");
                if (mushroomMeter == null || mushroomMeter.TryConsume(actionCost))
                {
                    sporeShooterScript.SporeShoot();
                }
                else
                {
                    Debug.Log("not enough mp to shoot");
                }
                 break;
            case "jump":
                Debug.Log("jump");
                if (mushroomMeter == null || mushroomMeter.TryConsume(actionCost))
                {
                    jumpPadScript.UseJumpPad();
                }
                {
                    Debug.Log("not enough mp to use jump pad");
                }
                break;
            default:
                Debug.Log("unknown action");
                break;
        }
    }

    void OnSwitchAction(InputValue input)
    {
        playerAimScript.FlipMushroom();
        if (action == "shoot")
        {
            action = "jump";
        } else
        {
            action = "shoot";
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector2(moveX,0));  // movement handling
    }

    public Vector2 BottomY => bottomY;  // getter for bottom y (needed for other scripts to raycast from bottom)

    public bool IsGrounded() 
    {
        return Physics2D.Raycast(rb.position - bottomY, Vector2.down, distToJump);
    }
}


