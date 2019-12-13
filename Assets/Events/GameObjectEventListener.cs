using UnityEngine;
using UnityEngine.Events;

public class GameObjectEventListener : MonoBehaviour
{

    public GameObjectEvent Event;
    public GameObjectEventResponse Response;

    public void OnEnable()
    {
        Event.RegisterListener(this);
    }

    public void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(Object obj)
    {
        Response.Invoke(obj);
    }
}
