using UnityEngine;

public class BlackObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<CursorController>() != null)
        {
            EventManager.CallBlackTouched();
        }
    }   
}
