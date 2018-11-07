using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSensor : MonoBehaviour {
   
    public bool isInside;
    public GameObject colidedObject;
    public string myTrackedTag;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == myTrackedTag)
        {
            isInside = true;
            colidedObject = collision.gameObject;
        }                
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == myTrackedTag)
        {
            isInside = false;
        }
    }
}
