using System.Collections.Generic;
using UnityEngine;

public class WhiteBallSpawner : MonoBehaviour
{
    // Ball that triggers launch when touched.
    public BallObject ballTrigger;
    public GameObject whiteBallPrefab;
    public List<float> whiteBallRotations;
    private List<WhiteBallObject> whiteBallObjects = new List<WhiteBallObject>();
    private List<WhiteBallObject> launchedBallObjects = new List<WhiteBallObject>();

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        // Remove previously launched balls.
        DestroyBalls();
        // only allow spawning if object does not have any balls under it.
        if (whiteBallObjects.Count > 0) return;

        // go over list of ball rotations and instantiate ball prefabs at calculated positions.
        for(int i = 0; i < whiteBallRotations.Count; i++)
        {
            GameObject aux = Instantiate(whiteBallPrefab);

            // calculate position from angle.
            Vector2 dir = Quaternion.Euler(0, 0, whiteBallRotations[i]) * Vector2.right;
            dir *= 0.4f;

            // set ball parameters.
            WhiteBallObject ball = aux.GetComponent<WhiteBallObject>();
            ball.SetActive(false);
            ball.SetPosition(new Vector2(transform.position.x + dir.x, transform.position.y + dir.y));
            whiteBallObjects.Add(ball);

        }
    }

    // Proceed to launch only if Ball triggering the event is the one associated with this spawner.
    public void LaunchIfBallMatch(Object obj)
    {
        BallObject ballObject = (BallObject) obj;
        
        if (ballObject == null) return;
        else if (ballObject == ballTrigger)
        {
            LaunchBalls(ballObject.GetColor());
        }
    }

    public void LaunchBalls(Color color)
    {
        for(int i = 0; i < whiteBallObjects.Count; i++)
        {
            WhiteBallObject aux = whiteBallObjects[i];
            // scaled vector from spawner to current position of white ball's rigid body.
            aux.SetVelocity((aux.GetPosition() - new Vector2(transform.position.x, transform.position.y)).normalized*3);
            // make collider active and detach object from spawner.
            aux.SetActive(true);
            // finally set color to the one provided on call.
            aux.SetColor(color);
        }
        launchedBallObjects = whiteBallObjects;
        whiteBallObjects = new List<WhiteBallObject>();
    }

    // Destroy any previously launched ball.
    public void DestroyBalls()
    {
        for (int i = 0; i < launchedBallObjects.Count; i++)
        {
            if(launchedBallObjects[i] != null)
                Destroy(launchedBallObjects[i].gameObject);
        }
        launchedBallObjects = new List<WhiteBallObject>();
    }
}
