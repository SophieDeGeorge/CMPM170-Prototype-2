using UnityEngine;

public class SporeShooterv2 : MonoBehaviour
{
    [SerializeField] private float range = 12f;
    [SerializeField] private LayerMask hittableMask; // set to Enemy + Friend layers in Inspector
    [SerializeField] private ParticleSystem shootParticles; // assign in Inspector

    [Header("Mask Regen Stats")]
    [SerializeField] private InvertedMeter mushroomMeter;
    [SerializeField] private int friendRegen = 0;
    [SerializeField] private int environmentRegen = 0;

    [Header("Graveyard")]
    [SerializeField] private LayerMask moveTo;

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

        // animate mushroom
        playerAimScript.Animate("shoot");

        // move and play the particle system
        if (shootParticles != null)
        {
            shootParticles.transform.SetPositionAndRotation(firePoint, Quaternion.LookRotation(dir));
            shootParticles.Emit(1);
        }

        RaycastHit2D hit = Physics2D.Raycast(firePoint, dir, range, hittableMask);
        if (hit.collider != null)
        {
            GameObject hitGO = hit.collider.gameObject;

            // If the object hit is in the "Friend" layer, stop its animation using the triggerDeath parameter
            if (hitGO.layer == LayerMask.NameToLayer("Friend"))
            {
                Animator hitAnimator = hit.collider.GetComponentInParent<Animator>();
                if (hitAnimator != null) hitAnimator.enabled = false;
                hit.collider.GetComponentInParent<SpriteRenderer>().color = Color.black;
                mushroomMeter.Regen(friendRegen);
            } else
            {
                hit.collider.GetComponentInParent<SpriteRenderer>().color = Color.grey;
                mushroomMeter.Regen(environmentRegen);
            }
            hitGO.layer = moveTo;
        }
    }
}