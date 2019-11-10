using System.Collections.Generic;
using UnityEngine;

public class WhiteBallSpawner : MonoBehaviour
{
    public GameObject whiteBallPrefab;
    public List<float> whiteBallRotations;
    public List<WhiteBallObject> whiteBallObjects = new List<WhiteBallObject>();
    public List<WhiteBallObject> launchedBallObjects = new List<WhiteBallObject>();

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        DestroyBalls();
        // only allow spawning if object does not have any balls under it.
        if (whiteBallObjects.Count > 0) return;

        // go over list of ball rotations and instantiate ball prefabs at calculated positions.
        for(int i = 0; i < whiteBallRotations.Count; i++)
        {
            GameObject aux = Instantiate(whiteBallPrefab);
            Debug.Log(aux.transform.position);
            Debug.Log(aux.transform.localPosition);

            // calculate position from angle.
            Vector2 dir = Quaternion.Euler(0, 0, whiteBallRotations[i]) * Vector2.right;
            dir *= 0.4f;
            /*Debug.Log(i + ">>" + dir);
            Debug.Log(transform.position);
            Debug.Log(new Vector2(transform.position.x + dir.x, transform.position.y + dir.y));*/

            // set ball parameters.
            WhiteBallObject ball = aux.GetComponent<WhiteBallObject>();
            ball.SetActive(false);
            ball.SetPosition(new Vector2(transform.position.x + dir.x, transform.position.y + dir.y));
            whiteBallObjects.Add(ball);

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
