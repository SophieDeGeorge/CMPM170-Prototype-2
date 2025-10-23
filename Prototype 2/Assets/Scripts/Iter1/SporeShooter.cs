using UnityEngine;

public class SporeShooter : MonoBehaviour
{
    [SerializeField] private float range = 12f;
    [SerializeField] private LayerMask hittableMask; // set to Enemy + Friend layers in Inspector
    [SerializeField] private float knockbackImpulse = 10f; // strength of push
    [SerializeField] private ParticleSystem shootParticles; // assign in Inspector

    private PlayerAim playerAimScript;

    private void Start()
    {
        playerAimScript = GetComponent<PlayerAim>();
    }

    public void SporeShoot()
    {
        Vector3 firePoint = playerAimScript.MushroomPos;
        Vector2 dir = playerAimScript.AimDir.normalized;

        // Visualize
        Debug.DrawRay(firePoint, dir * range, Color.green, 10f);

        // move and play the particle system
        if (shootParticles != null)
        {
            shootParticles.transform.SetPositionAndRotation(firePoint, Quaternion.LookRotation(dir));
            shootParticles.Emit(1);
        }

        RaycastHit2D hit = Physics2D.Raycast(firePoint, dir, range, hittableMask);
        if (hit.collider != null)
        {
            ApplyKnockback(hit, dir);
            playerAimScript.Animate("shoot");
        }
    }

    void ApplyKnockback(RaycastHit2D hit, Vector2 dir)
    {
        Rigidbody2D enemyRb = hit.rigidbody; // quicker than GetComponent<Rigidbody2D>()
        if (enemyRb == null) return;

        // Push AWAY from the PLAYER (not along the ray)
        enemyRb.AddForce(dir * knockbackImpulse, ForceMode2D.Impulse);
    }
}