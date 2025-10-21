using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] float jumpPadForce = 7f;   // vertical jump pad force
    [SerializeField] float distToJumpPad = 0.5f;       // max distance from platform player can use jump pad

    private Rigidbody2D rb;
    private PlayerController controller;
    private PlayerAim playerAimScript;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
        playerAimScript = GetComponent<PlayerAim>();
    }
    public void UseJumpPad()
    {
        Debug.DrawRay(rb.position - controller.BottomY, distToJumpPad * Vector2.down, Color.red, 1000, true);
        if (CanUseJumpPad() && !controller.IsGrounded())
        {
            rb.velocity = new(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpPadForce), ForceMode2D.Impulse);
        }
    }
    bool CanUseJumpPad()
    {
        return Physics2D.Raycast(rb.position - controller.BottomY, Vector2.down, distToJumpPad);
    }
}
