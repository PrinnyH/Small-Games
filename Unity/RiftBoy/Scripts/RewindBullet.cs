using System.Collections;
using UnityEngine;

public class RewindBullet : MonoBehaviour
{

    public float speed = 20f;
    public Rigidbody2D rb;
    public CircleCollider2D cC;
    public SpriteMask rewindPortal;
    private bool madeportal = false;


    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
        StartCoroutine(SelfDestruct());

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Ground" && !madeportal)
        {
            madeportal = true;
            Instantiate(rewindPortal, transform.position, Quaternion.identity);
            Destroy(gameObject);
            
        }
        else if (collider.tag == "Player" || collider.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(collider, cC);
        }

    }


    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(20f);
        Destroy(gameObject);
        GameObject.FindObjectOfType<PlayerMovement>().bulletsShot--;
    }


}
