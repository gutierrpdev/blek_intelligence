using System.Collections.Generic;
using UnityEngine;

public class BallWithProjectiles : BallObject
{
    // prefab to use for instantiating projectiles.
    public GameObject whiteBallPrefab;
    // list of positions where projectiles should be placed around ball center.
    public List<float> whiteBallRotations;

    private Quaternion initialPosition;
    private Vector3 spawnPosition;

    private void Start()
    {
        float halfHorizontalSize = GetComponent<SpriteRenderer>().bounds.extents.x;
        spawnPosition = transform.position + new Vector3(halfHorizontalSize * 13 / 20, 0, 0);
        initialPosition = transform.rotation;
        Spawn();

        EventManager.LevelRestart += RespawnBall;
    }

    private void Spawn()
    {
        // go over list of ball rotations and instantiate ball prefabs at calculated positions.
        for (int i = 0; i < whiteBallRotations.Count; i++)
        {
            WhiteBallObject projectile = 
                Instantiate(whiteBallPrefab, spawnPosition, Quaternion.identity).GetComponent<WhiteBallObject>();

            projectile.transform.RotateAround(transform.position, new Vector3(0, 0, 1), whiteBallRotations[i]);

            // set active to prevent whiteball projectile from colliding with this object.
            projectile.SetActive(false);

            projectile.SetParent(transform);
        }
    }

    private void LaunchBalls()
    {
        WhiteBallObject[] whiteBallObjects = gameObject.GetComponentsInChildren<WhiteBallObject>();
        for (int i = 0; i < whiteBallObjects.Length; i++)
        {
            WhiteBallObject aux = whiteBallObjects[i];
            // make collider active.
            aux.SetActive(true);
            // set color to be the same as this ball's.
            aux.SetColor(GetComponent<SpriteRenderer>().material.color);
            // detach from parent
            aux.SetParent(null);
            // set direction and speed parameters
            aux.SetProjectileDirection(aux.transform.position - transform.position);
            aux.SetSpeed(15f);
        }
    }

    private void DestroyBalls()
    {
        WhiteBallObject[] whiteBallObjects = gameObject.GetComponentsInChildren<WhiteBallObject>();
        for (int i = 0; i < whiteBallObjects.Length; i++)
        {
            WhiteBallObject aux = whiteBallObjects[i];
            Destroy(aux.gameObject);
        }
        whiteBallObjects = null;
    }

    protected override void AfterCollision()
    {
        // do the same thing here as for regular basic balls, but this time launch all spawned projectiles.
        base.AfterCollision();
        // launch all projectiles.
        LaunchBalls();
    }

    protected override void RespawnBall()
    {
        // resets all triggers to prevent animation mishaps.
        GetComponent<Animator>().ResetTrigger("Shrink");
        GetComponent<Animator>().ResetTrigger("Fade");

        DestroyBalls();


        // if collider is enabled, ball hasn't been collided with yet, and so we don't need to spawn it again.
        // if (GetComponent<Collider2D>().enabled) return;

        // enable collider and play instantiation clip.
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Animator>().SetTrigger("Instantiate");
        transform.rotation = Quaternion.Slerp(transform.rotation, initialPosition, 0);

        // If there is a scripted rotation attached to this ball, make sure it gets reseted.
        ScriptedRotation rot = GetComponent<ScriptedRotation>();
        if (rot != null)
        {
            rot.ResetRotation();
        }
        Spawn();
    }

    private void OnDisable()
    {
        EventManager.LevelRestart -= RespawnBall;
    }
}