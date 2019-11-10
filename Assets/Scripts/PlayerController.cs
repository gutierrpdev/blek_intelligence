using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region general_data
    public GameObject cursorPrefab;
    private CursorController currentCursor;
    public GameEvent restart;
    #endregion

    #region state_management
    enum State
    {
        AWAIT_INPUT, DRAW_LINE, LOOP_LINE
    }

    private State currentState;

    private void Update()
    {
        // Take input and change state accordingly.
        HandleInput();
        // Perform action depending on current state.
        PerformAction();
    }

    // FSM for player states
    private void HandleInput()
    {
        // input is provided while waiting for player touch.
        if (currentState == State.AWAIT_INPUT && Input.GetMouseButtonDown(0))
        {
            // Initialize line and begin drawing process.
            CreateLine();
            currentState = State.DRAW_LINE;
        }
        // input is discontinued while drawing.
        else if (currentState == State.DRAW_LINE && Input.GetMouseButtonUp(0))
        {
            // Begin looping process if line has enough points to work with.
            if (currentCursor.PointCount() > 1)
            {
                currentCursor.BeginLooping();
                currentState = State.LOOP_LINE;
            }
            // If line is not long enough, cancel drawing and wait again.
            else
            {
                currentCursor.DestroyCursor();
                currentState = State.AWAIT_INPUT;
            }
        }
        // input is provided while looping.
        else if (currentState == State.LOOP_LINE && Input.GetMouseButtonDown(0))
        {
            currentCursor.DestroyCursor();
            CreateLine();
            currentState = State.DRAW_LINE;
        }
    }

    // Perform an action dependent on current player state. 
    private void PerformAction()
    {
        switch (currentState)
        {
            // wait for input to be provided.
            case State.AWAIT_INPUT: break;
            // add screen positions to current line.
            case State.DRAW_LINE:
                Vector2 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                currentCursor.UpdateLine(newPos);
                break;
            // loop until death or new line
            case State.LOOP_LINE:
                currentCursor.UpdateLoop();
                break;
        }
    }
    #endregion

    // Initialize line with original point.
    void CreateLine()
    { 
        // initialize drawing
        Vector2 newPos = Input.mousePosition;
        Vector3 newPosWorld = Camera.main.ScreenToWorldPoint(newPos);
        newPosWorld.z = 0;

        GameObject go = Instantiate(cursorPrefab, newPosWorld, Quaternion.identity);
        currentCursor = go.GetComponent<CursorController>();
    }
}
