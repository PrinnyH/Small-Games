using UnityEngine;

public class HealthUp : MonoBehaviour
{
    
    public Animator anim;
    public float delay = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            if (collision.GetComponent<PlayerMovement>().health < collision.GetComponent<PlayerMovement>().maxHealth)
            {
                collision.GetComponent<PlayerMovement>().IncreaseHealth();
                anim.SetTrigger("PickedUp");
                Destroy(gameObject, delay);

            }

        }
    }
}

