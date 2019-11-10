using UnityEngine;

public class BallObject : MonoBehaviour
{
    public GameEvent ballTouched;
    public WhiteBallSpawner spawner;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Destroy colliding ball if it is a projectile.
        if (other.gameObject.GetComponent<WhiteBallObject>() != null)
        {
            GetComponent<Animator>().SetTrigger("Fade");
            Destroy(other.gameObject);
        }
        else if (other.gameObject.GetComponent<CursorController>() != null)
        {
            CursorCollided();
        }
        else return;

        // disable collider so that gameobjects can no longer interact with ball
        GetComponent<Collider2D>().enabled = false;

        // and send signal
        ballTouched.Raise();
    }

    private void CursorCollided()
    {
        // if it doesn't, make it fade.
        if (spawner == null)
        {
            GetComponent<Animator>().SetTrigger("Fade");
        }
        // if it does, launch all projectiles with ball's own color.
        else
        {
            GetComponent<Animator>().SetTrigger("Shrink");
            Color whiteColor = GetComponent<SpriteRenderer>().material.color;
            spawner.LaunchBalls(whiteColor);
        }
    }

    public void RespawnBall()
    {
        GetComponent<Animator>().ResetTrigger("Shrink");
        GetComponent<Animator>().ResetTrigger("Fade");
        if (GetComponent<Collider2D>().enabled) return;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Animator>().SetTrigger("Instantiate");
    }

}
