using UnityEngine;

public class PlayerAim2D : MonoBehaviour
{
    [SerializeField] private SpriteRenderer MushroomSprite; // assign Mushroom's SpriteRenderer

    void Update()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mouse - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Keep sprite looking correct when aiming left
        bool pointingLeft = angle > 90f || angle < -90f;
        if (MushroomSprite) MushroomSprite.flipY = pointingLeft;
    }
}