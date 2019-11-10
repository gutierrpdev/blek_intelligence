using UnityEngine;

public class WhiteBallObject : MonoBehaviour
{

    public Vector2 velocity = new Vector2(0, 0);

    private void Update()
    {
        float newX = transform.position.x + velocity.x * Time.deltaTime;
        float newY = transform.position.y + velocity.y * Time.deltaTime;
        transform.position = new Vector3(newX, newY, transform.position.z);    
    }

    private void Start()
    {
        SetActive(false);
    }

    public void SetVelocity(Vector2 velocity)
    {
        this.velocity = velocity;
    }

    public void SetActive(bool active)
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = active;
        }
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }

    public Vector2 GetPosition()
    {
        return new Vector2(transform.position.x, transform.position.y);
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

}
