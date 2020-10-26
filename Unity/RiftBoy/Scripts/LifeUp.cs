using UnityEngine;

public class LifeUp : MonoBehaviour
{
    public Animator anim;
    public float delay = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerMovement>().IncreaseLife();
            anim.SetTrigger("PickedUp");
            Destroy(gameObject, delay);

        }
    }
}
