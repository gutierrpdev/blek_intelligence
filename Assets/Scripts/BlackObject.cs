using UnityEngine;

public class BlackObject : MonoBehaviour
{
    public GameEvent blackEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<CursorController>() != null)
        {
            blackEvent.Raise();
        }
    }   
}
