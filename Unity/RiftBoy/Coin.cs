
using UnityEngine;

public class Coin : MonoBehaviour
{
    public Animator anim;
    public float delay = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerMovement>().IncreaseCoins();
            anim.SetTrigger("PickedUp");
            Destroy(gameObject, delay);
        }
    }
}
