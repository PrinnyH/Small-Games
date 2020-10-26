using UnityEngine;

public class HitToSee : MonoBehaviour
{
    public Collider2D cl;
    private float TriggerCount;


    private void Start()
    {
        cl.enabled = false;
    }


    void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.tag == "Portal")
        {
            cl.enabled = true;
            this.gameObject.tag = "Ground";
            this.gameObject.layer = 8;
            TriggerCount++;
        }
    }


    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Portal" && TriggerCount == 1)
        {
            cl.enabled = false;
            this.gameObject.tag =  "OldGround";
            this.gameObject.layer = 11;
            TriggerCount--;
        }
        else if(collider.tag == "Portal")
        {
            TriggerCount--;
        }
    }




}
