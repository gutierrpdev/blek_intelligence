using System.Collections.Generic;
using UnityEngine;

/* Provides Methods to manage and control a screen cursor to be used solely by player.
 * This can be considered the main object to be controlled by the player class in a given level.
 * For this reason, no update method is included so as to force cursor to be updated only through player calls.
 */
public class CursorController : MonoBehaviour
{
    private List<Vector2> cursorTrajectories; // List containing all points in current trace.
    private Transform tr;
    private Vector2 offset; // difference between first point on current line and original first point
    private Vector2 reference; // first point on current line
    private int loopPosition;
    private bool reflected;
    private bool locallyReflected;
    private bool outOfBounds; // out of vertical bounds.

    // Initialize data related to reflection and set transform reference.
    void Awake()
    {
        cursorTrajectories = new List<Vector2>();
        reflected = false;
        locallyReflected = false;
        outOfBounds = false;
        tr = GetComponent<Transform>();
    }

    // Moves cursor following given trajectory and reflects it when needed.
    public void UpdateLoop()
    {
        if (loopPosition == 0) // update reference
        {
            reference = tr.position;
            offset = reference - cursorTrajectories[0];
            if (locallyReflected) reflected = !reflected;
            locallyReflected = false;
        }

        Vector2 expectedPoint = cursorTrajectories[loopPosition] + offset;
        if (reflected) expectedPoint.x -= 2 * (expectedPoint.x - reference.x); // expected next point

        Vector2 cameraPoint = Camera.main.WorldToScreenPoint(expectedPoint);
        if (cameraPoint.x < 0) // out of bounds (left side)
        {
            cameraPoint.x = -cameraPoint.x;
            locallyReflected = true;
        }
        else if (cameraPoint.x > Screen.width) // out of bounds (right side)
        {
            cameraPoint.x -= 2 * (cameraPoint.x - Screen.width);
            locallyReflected = true;
        }
        // out of bounds (vertically) and last point in trajectory.
        else if((cameraPoint.y > Screen.height || cameraPoint.y < 0) && loopPosition == cursorTrajectories.Count - 1)
        {
            outOfBounds = true;
        }

        tr.position = Camera.main.ScreenToWorldPoint(cameraPoint);

        // move position in trajectory.
        loopPosition = (loopPosition + 1) % cursorTrajectories.Count;
    }

    // Adds a new point to line if distance with last point is sufficient.
    public void UpdateLine(Vector2 newPos)
    {

        cursorTrajectories.Add(newPos);
        tr.position = newPos;
        /*if(cursorTrajectories.Count == 0 || 
                Vector2.Distance(cursorTrajectories[cursorTrajectories.Count - 1], newPos) > 0.005f)
        {
            cursorTrajectories.Add(newPos);
            tr.position = newPos;
        }*/
    }

    // Initialize looping process
    public void BeginLooping()
    {
        //FrechetDistance.Test();
        //GameAnalytics.AddTrace(cursorTrajectories);
        loopPosition = 0;
        offset = cursorTrajectories[cursorTrajectories.Count - 1] - cursorTrajectories[0];
        reference = cursorTrajectories[cursorTrajectories.Count - 1];
    }

    // Destroy cursor ensuring that trail is maintained on screen for a sufficiently long time.
    public void DestroyCursor()
    {
        enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, GetComponent<TrailRenderer>().time);
    }

    // Returns current number on points on line.
    public int PointCount()
    {
        return cursorTrajectories == null ? 0 : cursorTrajectories.Count;
    }

    // Checks if cursor is out of vertical bounds.
    public bool IsOutOfBounds()
    {
        return outOfBounds;
    }
}
