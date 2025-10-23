using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private SpriteRenderer mushroomSprite; // assign Mushroom's SpriteRenderer
    [SerializeField] private float orbitDistance = 1.0f;
    private float aimAngle = 0;
    private Vector3 aimDir = Vector3.zero;
    private Vector3 mushroomPos = Vector3.zero;
    private bool flipped = false;

    void OnAim(InputValue input)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(input.Get<Vector2>());
        mousePos.z = 0f;
        aimDir = mousePos - transform.position;
        aimAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;

        
        mushroomSprite.transform.SetPositionAndRotation(mushroomPos, Quaternion.Euler(0, 0, aimAngle+90));
        CheckFlip(mousePos);
    }

    public Vector2 AimDir => aimDir;
    public Vector3 MushroomPos => mushroomPos;

    private void Update()
    {
        mushroomPos = transform.position + aimDir.normalized * orbitDistance;
    }
    void CheckFlip(Vector3 mousePos)
    {
        if (mousePos.x < transform.position.x != flipped)
        {
            FlipPlayer();
        }
            
    }

    void FlipPlayer()
    {
        flipped = !flipped;

        Vector3 flippedScale = transform.localScale;
        flippedScale.x *= -1;
        transform.localScale = flippedScale;

        Vector3 weaponScale = mushroomSprite.transform.localScale;
        weaponScale.x *= -1;
        mushroomSprite.transform.localScale = weaponScale;
    }

    public void FlipMushroom()
    {
        Vector3 weaponScale = mushroomSprite.transform.localScale;
        weaponScale.y *= -1;
        mushroomSprite.transform.localScale = weaponScale;
    }

    public void Animate(string name)
    {
        Animator animator = mushroomSprite.GetComponent<Animator>();
        animator.SetTrigger("shoot");
    }
}