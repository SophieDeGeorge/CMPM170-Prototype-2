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

    private Vector2 aimDir = Vector2.zero;
    private Vector3 mushroomPos = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
        playerAimScript = GetComponent<PlayerAim>();
    }
    public void UseJumpPad()
    {
        aimDir = playerAimScript.AimDir.normalized;
        mushroomPos = playerAimScript.MushroomPos;
        Debug.DrawRay(mushroomPos, distToJumpPad * aimDir, Color.red, 1000, true);
        if (CanUseJumpPad() && !controller.IsGrounded())
        {
            rb.velocity = new(0, 0);
            rb.AddForce(jumpPadForce * -aimDir, ForceMode2D.Impulse);
        }
    }
    bool CanUseJumpPad()
    {
        return Physics2D.Raycast(mushroomPos, aimDir, distToJumpPad);
    }
}
