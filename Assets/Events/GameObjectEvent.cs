using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameObjectEvent : ScriptableObject
{
    private List<GameObjectEventListener> listeners = new List<GameObjectEventListener>();

    public void Raise(Object obj)
    {
        listeners.ForEach(elem =>
        {
            elem.OnEventRaised(obj);
        });

    }

    public void RegisterListener(GameObjectEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(GameObjectEventListener listener)
    {
        listeners.Remove(listener);
    }

}

