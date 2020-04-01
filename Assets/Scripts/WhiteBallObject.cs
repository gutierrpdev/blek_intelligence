using UnityEngine;

public class WhiteBallObject : MonoBehaviour
{

    private Vector2 projectileDirection;
    private float speed;

    private void Update()
    {
        transform.Translate(projectileDirection * Time.deltaTime * speed, Space.World);
    }

    private void Start()
    {
        EventManager.LevelRestart += DestroyProjectile;
        SetActive(false);
        SetProjectileDirection(new Vector2(0, 0));
    }

    public void SetProjectileDirection(Vector2 direction)
    {
        projectileDirection = direction;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetActive(bool active)
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = active;
        }
    }

    public void SetColor(Color color)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.material.color = color;
    }

    public void SetParent(Transform t)
    {
        transform.SetParent(t);
    }

    private void DestroyProjectile()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col.enabled)
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        EventManager.LevelRestart -= DestroyProjectile;
    }

}
