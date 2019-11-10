using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region general_data

    public GameObject cursorPrefab;
    public CursorController currentCursor;
    public GameEvent restart;

    #endregion

    #region MonoBehaviorAPI

    // Update is called once per frame
    void Update()
    {
        // case 1: player decides to start drawing the line
        if (Input.GetMouseButtonDown(0))
        {
            if(currentCursor != null)
                ResetLevel();
            CreateLine();
        }
        // case 2: player is currently drawing the line
        else if (Input.GetMouseButton(0) && currentCursor != null && currentCursor.drawing)
        {
            Vector2 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentCursor.UpdateLine(newPos);       
        }
        // case 3: player finishes drawing the line
        else if (Input.GetMouseButtonUp(0) && currentCursor != null && currentCursor.drawing)
        {
            currentCursor.BeginLooping();
        }
    }
    #endregion


    // Initialize line with original point
    void CreateLine()
    { 
        // initialize drawing
        Vector2 newPos = Input.mousePosition;
        Vector3 newPosWorld = Camera.main.ScreenToWorldPoint(newPos);
        newPosWorld.z = 0;

        GameObject go = Instantiate(cursorPrefab, newPosWorld, Quaternion.identity);
        currentCursor = go.GetComponent<CursorController>();
    }

    // destroys current cursor
    private void ResetCursor()
    {
        if(currentCursor != null)
        {
            currentCursor.StopAndDestroy();
            currentCursor = null;
        }
    }

    // resets cursor and triggers event to respawn all balls
    public void ResetLevel()
    {
        ResetCursor();
        restart.Raise();
    }

    public void StopDrawing()
    {
        if (currentCursor != null)
        {
            currentCursor.drawing = false;
        }
    }
}
