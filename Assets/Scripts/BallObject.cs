using UnityEngine;

public class BallObject : MonoBehaviour
{
    public GameObjectEvent ballTouched;

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

        // and send signal to notify that THIS ball was collided with.
        ballTouched.Raise(this);
    }

    private void CursorCollided()
    {
        // Check whether ball has a spawner.
        // if it doesn't, make it fade.
        /*if (spawner == null)
        {
            GetComponent<Animator>().SetTrigger("Fade");
        }*/
        // if it does, launch all projectiles with ball's own color.
        /* else
         {
             GetComponent<Animator>().SetTrigger("Shrink");
             spawner.LaunchBalls(GetColor());
         }*/

        GetComponent<Animator>().SetTrigger("Fade");
    }

    public void RespawnBall()
    {
        GetComponent<Animator>().ResetTrigger("Shrink");
        GetComponent<Animator>().ResetTrigger("Fade");
        if (GetComponent<Collider2D>().enabled) return;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Animator>().SetTrigger("Instantiate");
    }

    public Color GetColor()
    {
        return GetComponent<SpriteRenderer>().material.color;
    }

}
