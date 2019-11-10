using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public List<Vector2> cursorTrajectories;
    public Transform tr;
    public Vector2 offset; // difference between first point on current line and original first point
    public Vector2 reference; // first point on current line
    public int loopPosition;
    public bool drawing;
    public bool reflected;
    public bool locallyReflected;

    #region MonoBehaviorAPI
    // Start is called before the first frame update
    void Awake()
    {
        drawing = true;
        reflected = false;
        locallyReflected = false;
        tr = GetComponent<Transform>();
    }

    private void Update()
    {
        if (!drawing) // looping
        {
            if(loopPosition == 0) // update reference
            {
                reference = tr.position;
                offset = reference - cursorTrajectories[0];
                if (locallyReflected) reflected = !reflected;
                locallyReflected = false;
            }

            Vector2 expectedPoint = cursorTrajectories[loopPosition] + offset;
            if (reflected) expectedPoint.x -= 2*(expectedPoint.x - reference.x); // expected next point

            Vector2 cameraPoint = Camera.main.WorldToScreenPoint(expectedPoint);
            if (cameraPoint.x < 0) // out of bounds (left side)
            {
                cameraPoint.x = -cameraPoint.x;
                locallyReflected = true;
            }
            else if(cameraPoint.x > Screen.width) // out of bounds (right side)
            {
                cameraPoint.x -= 2 * (cameraPoint.x - Screen.width);
                locallyReflected = true;
            }

            tr.position = Camera.main.ScreenToWorldPoint(cameraPoint);

            loopPosition = (loopPosition + 1) % cursorTrajectories.Count;
        }
    }

    #endregion

    public void UpdateLine(Vector2 newPos)
    {
        cursorTrajectories.Add(newPos);
        tr.position = newPos;
    }

    // Initialize looping process
    public void BeginLooping()
    {
        loopPosition = 0;
        drawing = false;
        offset = cursorTrajectories[cursorTrajectories.Count - 1] - cursorTrajectories[0];
        reference = cursorTrajectories[cursorTrajectories.Count - 1];
    }

    public void StopAndDestroy()
    {
        enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, GetComponent<TrailRenderer>().time);
    }

    public void SetPosition(Vector2 pos)
    {
        tr.position = pos;
    }
}
