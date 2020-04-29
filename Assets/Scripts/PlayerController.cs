using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region general_data
    public GameObject cursorPrefab;
    private CursorController currentCursor;
    #endregion

    #region state_management
    // State definitions and current state reference
    enum State
    {
        AWAIT_INPUT, FIRST_TOUCH, DRAW_LINE, LOOP_LINE
    }

    private State currentState;
    private Vector2 firstTouchPos;
    private BallObject firstTouchBall;

    // Flags
    private bool blackEventTriggered = false;
    private bool ballTouchedEventTriggered = false;
    private bool levelCompleted = false;

    private void Start()
    {
        EventManager.BallTouched += TriggerBallTouchedEvent;
        EventManager.BlackTouched += TriggerBlackEvent;
        EventManager.LevelCompleted += TriggerLevelCompletedEvent;
    }

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
        Debug.Log(currentState);
        //cheat 
        if(Input.touches.Length >= 3)
        {
            EventManager.CallLevelCompleted();
            return;
        }
        switch (currentState)
        {
            case State.AWAIT_INPUT: HandleAwait(); break;
            case State.FIRST_TOUCH: HandleFirstTouch(); break;
            case State.DRAW_LINE: HandleDrawing(); break;
            case State.LOOP_LINE: HandleLooping(); break;
        }
    }

    private void HandleAwait()
    {
        // screen touched at valid point.
        if (Input.GetMouseButtonDown(0) && ValidPoint())
        {
            // register first touch position
            firstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // change to first touch to determine whether user keeps drawing.
            currentState = State.FIRST_TOUCH;
            EventManager.CallFirstTouch();
        }
    }

    private void HandleFirstTouch()
    {
        // user keeps finger down: two cases
        // CASE 1: User is keeping finger at exact same position -> maintain state
        // CASE 2: User decides to move finger -> start drawing
        if (Input.GetMouseButton(0))
        {
            if (!firstTouchPos.Equals(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
            {
                // Create line at given point.
                CreateLine();
                // Initialize line and begin drawing process.
                currentState = State.DRAW_LINE;
                // Notify about beginning of drawing phase.
                EventManager.CallDrawing();
            } 
        }
        // user lifts finger -> go back to AWAIT state.
        else
        {
            currentState = State.AWAIT_INPUT;
        }
    }

    private void HandleDrawing()
    {
        // screen released OR ballTouched event triggers.
        if (Input.GetMouseButtonUp(0) || ballTouchedEventTriggered)
        {
            // adapt curve to end at center of touched ball.
            if (ballTouchedEventTriggered)
            {
                InterpolateLine();
            }
            ballTouchedEventTriggered = false;
            firstTouchBall = null;
            currentCursor.BeginLooping();
            EventManager.CallLooping();
            currentState = State.LOOP_LINE;
        }
        // black event triggers.
        else if (blackEventTriggered)
        {
            blackEventTriggered = false;
            currentCursor.DestroyCursor();
            EventManager.CallLevelRestart();
            currentState = State.AWAIT_INPUT;
        }
    }

    private void HandleLooping()
    {
        if (levelCompleted)
        {
            levelCompleted = false;
            currentCursor.DestroyCursor();
            currentState = State.AWAIT_INPUT;
        }
        // black event triggered or cursor out of bounds.
        else if (blackEventTriggered || currentCursor.IsOutOfBounds())
        {
            blackEventTriggered = false;
            currentCursor.DestroyCursor();
            EventManager.CallLevelRestart();
            currentState = State.AWAIT_INPUT;
        }
        // screen touched while looping.
        else if (Input.GetMouseButtonDown(0))
        {
            EventManager.CallLevelRestart();
            currentCursor.DestroyCursor();
            EventManager.CallFirstTouch();
            // register first touch position
            firstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentState = State.FIRST_TOUCH;
        }
    }

    // Perform an action dependent on current player state. 
    private void PerformAction()
    {
        switch (currentState)
        {
            // add screen positions to current line.
            case State.DRAW_LINE:
                Vector2 newPos;
                newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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

    private void InterpolateLine()
    {
        Vector2 firstPos;
        firstPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lastPos = firstTouchBall.transform.position;
        Vector2 lineVect = lastPos - firstPos;

        for(int i = 0; i < 25; i++)
        {
            currentCursor.UpdateLine(firstPos + lineVect * (i/25f));
        }
    }

    private bool ValidPoint()
    {
        // TODO: ensure that line does not start on top of any item on the screen.

        return true;
    }

    // Sets a flag to trigger black event when checking FSM.
    private void TriggerBlackEvent(){
        blackEventTriggered = true;
    }

    // Sets a flag to trigger ballTouched event when checking FSM.
    private void TriggerBallTouchedEvent(BallObject ball){
        if (currentState != State.LOOP_LINE)
        {
            ballTouchedEventTriggered = true;
            firstTouchBall = ball;
        }
    }

    // Sets a flag to trigger levelCompleted event when checking FSM.
    private void TriggerLevelCompletedEvent(){
        levelCompleted = true;
    }
    #endregion
}
