using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedRotation : MonoBehaviour
{
    [Serializable]
    public struct Rotation
    {
        public float angle;
        public float seconds;
    };

    private bool rotationDone;
    private int currentRotation;
    private IEnumerator currentCoroutine;

    // Each rotation in script is defined by a number of degrees to rotate in a step and the amount of time
    // (in frames) required to perform such a rotation.
    public List<Rotation> rotations; 

    void Start()
    {
        rotationDone = true;
        currentRotation = -1;
    }

    void Update()
    {
        if (rotationDone && rotations.Count > 0)
        {
            currentRotation = (currentRotation + 1) % rotations.Count;
            currentCoroutine =
                RotateDegreesInSeconds(rotations[currentRotation].angle, rotations[currentRotation].seconds);
            StartCoroutine(currentCoroutine);
        }
    }

    public void ResetRotation()
    {
        StopCoroutine(currentCoroutine);
        currentCoroutine = null;
        rotationDone = true;
        currentRotation = 0;
    }

    private IEnumerator RotateDegreesInSeconds(float degrees, float seconds)
    {
        rotationDone = false;

        // number of frames needed to complete rotation.
        float frames = seconds / Time.deltaTime;
        // number of degrees to rotate each frame.
        float degPerFrame = frames == 0 ? 0 : degrees / frames;
        float accRotation = 0;

        while(frames > 0){
            // rotate degPerFrame degrees each frame.
            transform.RotateAround(transform.position, new Vector3(0, 0, 1), degPerFrame);
            accRotation += degPerFrame;

            // recalculate needed frames and degrees per frame.
            //seconds -= Time.deltaTime;
            //degrees -= degPerFrame;
            //frames = seconds / Time.deltaTime;
            //degPerFrame = degrees / frames;
            frames--;

            // wait for next frame.
            yield return null;
        }

        transform.RotateAround(transform.position, new Vector3(0, 0, 1), degrees - accRotation);

        rotationDone = true;
        // make sure ending point matches expected ending point.
        // float fixAngle = degrees - frames * degPerFrame;
        // transform.RotateAround(transform.position, new Vector3(0, 0, 1), fixAngle);
    }
}
