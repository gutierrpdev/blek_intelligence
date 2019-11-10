using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region general_data
    public GameObject cursorPrefab;
    private CursorController currentCursor;
    public GameEvent restart;
    public GameEvent drawing;
    public GameEvent drawingStopped;
    #endregion

    #region state_management
    enum State
    {
        AWAIT_INPUT, DRAW_LINE, LOOP_LINE
    }

    private State currentState;
    private bool blackEventTriggered = false;
    private bool ballTouchedEventTriggered = false;

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
        if (currentState == State.AWAIT_INPUT)
        {
            // screen touched at valid point.
            if (Input.GetMouseButtonDown(0) && ValidPoint())
            {
                // Create line at given point.
                CreateLine();
                // Initialize line and begin drawing process.
                currentState = State.DRAW_LINE;
                // Notify about beginning of drawing phase.
                drawing.Raise();
            }
            
        }
        // input is discontinued while drawing.
        else if (currentState == State.DRAW_LINE)
        {
            // screen released and line long enough OR ballTouched event triggers.
            if ((Input.GetMouseButtonUp(0) && currentCursor.PointCount() > 1) || ballTouchedEventTriggered)
            {
                ballTouchedEventTriggered = false;
                currentCursor.BeginLooping();
                drawingStopped.Raise();
                currentState = State.LOOP_LINE;
            }
            // screen released and line too short OR black event triggers.
            else if ((Input.GetMouseButtonUp(0) && currentCursor.PointCount() <= 1) || blackEventTriggered)
            {
                blackEventTriggered = false;
                currentCursor.DestroyCursor();
                restart.Raise();
                drawingStopped.Raise();
                currentState = State.AWAIT_INPUT;
            }
        }
        // input is provided while looping.
        else if (currentState == State.LOOP_LINE)
        {
            // screen touched at invalid point OR black event triggered.
            if (Input.GetMouseButtonDown(0) && !ValidPoint() || blackEventTriggered)
            {
                blackEventTriggered = false;
                currentCursor.DestroyCursor();
                restart.Raise();
                currentState = State.AWAIT_INPUT;
            }
            // screen touched at valid point.
            else if (Input.GetMouseButtonDown(0) && ValidPoint())
            {
                restart.Raise();
                currentCursor.DestroyCursor();
                // Create Line at given point.
                CreateLine();
                // Notify about beginning of drawing phase.
                drawing.Raise();
                currentState = State.DRAW_LINE;
            }
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

    #region utils
    // Initialize line with original point.
    // Create line at current screen touch point.
    private void CreateLine()
    { 
        // initialize drawing
        Vector2 newPos = Input.mousePosition;
        Vector3 newPosWorld = Camera.main.ScreenToWorldPoint(newPos);
        newPosWorld.z = 0;

        GameObject go = Instantiate(cursorPrefab, newPosWorld, Quaternion.identity);
        currentCursor = go.GetComponent<CursorController>();
    }

    private bool ValidPoint()
    {
        // TODO: ensure that line does not start on top of any item on the screen.

        return true;
    }

    // Sets a flag to trigger black event when checking FSM.
    public void TriggerBlackEvent(){
        blackEventTriggered = true;
    }

    // Sets a flag to trigger ballTouched event when checking FSM.
    public void TriggerBallTouchedEvent()
    {
        ballTouchedEventTriggered = true;
    }
    #endregion
}
