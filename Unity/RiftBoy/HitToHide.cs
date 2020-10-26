using UnityEngine;

public class HitToHide : MonoBehaviour
{
    public Collider2D cl;
    private float TriggerCount;


    private void Start()
    {
        cl.enabled = true;
    }


    void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.tag == "Portal")
        {
            cl.enabled = false;
            TriggerCount++;
        }
    }


    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Portal" && TriggerCount == 1)
        {
            cl.enabled = true;
            TriggerCount--;
        }
        else if(collider.tag == "Portal")
        {
            TriggerCount--;
        }
    }




}
