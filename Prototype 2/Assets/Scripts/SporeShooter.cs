using UnityEngine;

public class SporeShooter2D : MonoBehaviour
{
    [SerializeField] private Transform firePoint;   // where the spore ray starts (e.g., gun muzzle)
    [SerializeField] private float range = 12f;
    [SerializeField] private float cooldown = 0.2f;
    [SerializeField] private LayerMask hittableMask; // set to Enemy layer in Inspector
    [SerializeField] private float knockbackImpulse = 10f; // strength of push

    private float nextShootTime;

    void Update()
    {
        if (Time.time < nextShootTime) return;

        if (Input.GetMouseButtonDown(0)) // Left mouse to shoot
        {
            ShootTowardMouse();          // or use ShootForward() below
            nextShootTime = Time.time + cooldown;
        }
    }

    void ShootTowardMouse()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mouse - firePoint.position);
        dir.Normalize();

        // Visualize
        Debug.DrawRay(firePoint.position, dir * range, Color.green, 0.25f);

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, dir, range, hittableMask);
        if (hit.collider != null)
        {
            ApplyKnockback2D(hit);
        }
    }

    // If you want to shoot along the player's forward in 2D (usually the local +X):
    void ShootForward()
    {
        Vector2 dir = firePoint.right; // assumes firePoint points in shooting direction
        Debug.DrawRay(firePoint.position, dir * range, Color.cyan, 0.25f);

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, dir, range, hittableMask);
        if (hit.collider != null)
        {
            ApplyKnockback2D(hit);
        }
    }

    void ApplyKnockback2D(RaycastHit2D hit)
    {
        Rigidbody2D enemyRb = hit.rigidbody; // quicker than GetComponent<Rigidbody2D>()
        if (enemyRb == null) return;

        // Push AWAY from the PLAYER (not along the ray)
        Vector2 awayFromPlayer = ((Vector2)hit.transform.position - (Vector2)transform.position).normalized;
        enemyRb.AddForce(awayFromPlayer * knockbackImpulse, ForceMode2D.Impulse);
    }
}
