using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public int maxHealth;

    public float inv;
    public float invValue;

    public float speed;
    public Vector2 startPos;
    public float walkDistance;

    public Animator anim;

    public float deathDelay;

    public HealthBar healthBar;

    public int state = 0;

    public bool playerInView = false;

    public bool canHurtPlayer = true;

    public bool facingRight;

    void Start()
    {
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        startPos = transform.position;
        inv = invValue;
    }


    // Update is called once per frame
    void Update()
    {
        
        if(health <= 0)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            Destroy(GetComponent<Collider2D>());
            anim.Play("Base Layer.death");
            Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length + deathDelay);
        }

        else if (health > 0)
        {
            anim.SetBool("PlayerInSight", playerInView);
            inv -= Time.deltaTime;

            canHurtPlayerCheck();
        }



    }

    virtual public void TakeDamage(int damage)
    {
        if (inv <= 0)
        {
            health -= damage;
            anim.Play("Base Layer.hurt");
            healthBar.SetHealth(health);
            inv = invValue;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && playerInView == false)
        {
            playerInView = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInView = false;

        }
    }



    virtual public void canHurtPlayerCheck()
    {

    }

}
