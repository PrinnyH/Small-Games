using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{


    [HideInInspector]
    public int health;

    public int maxHealth;


    public float speedValue;
    private float speed;

    public float jumpForce;
    private float moveInput;

    private float dazedTime;
    public float startDazedTime;

    private float inv;
    public float invTime;


    private Rigidbody2D rb;

    private bool facingRight = true;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;


    private int extraJumps;
    public int extraJumpsValue;


    private float mouseX;

    public Animator animPres;
    public Animator animOld;

    public int touchDamage;

    public float deathDelay;

    public HealthBar healthBar;

    [HideInInspector]
    public Text livesText;

    [HideInInspector]
    public int lives;

    public int livesStart;

    [HideInInspector]
    public int coins;

    [HideInInspector]
    public Text coinsText;


    
    
    [HideInInspector]
    public int bulletsShot;
    
    public int bulletsMax;

    private int portalIn;

    public int healerValue = 5;

    [HideInInspector]
    public Vector2 revivePos;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        healthBar.SetMaxHealth(maxHealth);

        health = PlayerPrefs.GetInt("Health", maxHealth);
        lives = PlayerPrefs.GetInt("Lives", livesStart);
        coins = PlayerPrefs.GetInt("Coins", 0);
        livesText.text = "" + lives;
        coinsText.text = "" + coins;
    
    }

    private IEnumerator RevivePlayer(Vector2 resetPos, float delay)
    {
        yield return new WaitForSeconds(delay);
        transform.position = resetPos;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        gameObject.GetComponent<Collider2D>().enabled = true;
        healthBar.SetHealth(health); 
        livesText.text = " " + lives;

    }


    // Update is called once per frame
    void Update()
    {

        if (health <= 0 && lives > 0)
        {
            health = maxHealth;
            lives--;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            gameObject.GetComponent<Collider2D>().enabled = false;
            animPres.Play("Base Layer.death");
            animOld.Play("Base Layer.death");
            StartCoroutine(RevivePlayer(revivePos, deathDelay));    
        } 
        else if (health <=  0 && lives == 0)
        {
            animPres.Play("Base Layer.death");
            animOld.Play("Base Layer.death");
            Destroy(gameObject, deathDelay);
            GameObject.FindObjectOfType<LevelLoader>().GameOver();
        }
        else
        {
            GetInputs();

            if (dazedTime <= 0)
            {
                speed = speedValue;
            }
            else
            {
                speed = 0;
                dazedTime -= Time.deltaTime;
            }
            //countdown for invulnerability
            inv -= Time.deltaTime;

        }

    }

    void FixedUpdate()
    {
        if (health > 0)
        {
            MovePlayer();
        }
    }


    private void GetInputs()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        mouseX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;

        animPres.SetFloat("H", Mathf.Abs(moveInput));
        animOld.SetFloat("V", rb.velocity.y);

        animPres.SetFloat("V", rb.velocity.y);
        animOld.SetFloat("H", Mathf.Abs(moveInput));
        

        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
            animPres.SetBool("DoubleJump", false);
            animOld.SetBool("DoubleJump", false);
        }


        if ( (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
            AudioManager.instance.Play("PlayerJump");
            if (!isGrounded)
            {
                animPres.SetBool("DoubleJump", true);
                animOld.SetBool("DoubleJump", true);
            }
        } 
        else if ( (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && extraJumps == 0 && isGrounded )
        {
            AudioManager.instance.Play("PlayerJump");
            rb.velocity = Vector2.up * jumpForce;
        }


    }


    private void MovePlayer() {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);


        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (facingRight == false && mouseX > transform.position.x)
        {
            Flip();
        }
        
        else if (facingRight == true && mouseX < transform.position.x)
        {
            Flip();
        }

    }


    //flips the sprite
    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    public void TakeDamage(int damage)
    {
        animPres.Play("Base Layer.hurt",0,0);
        animOld.Play("Base Layer.hurt", 0, 0);
        dazedTime = startDazedTime;
        health -= damage;
        healthBar.SetHealth(health);
        inv = invTime;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy" && health > 0 && inv <= 0)
        {
            if (collision.gameObject.GetComponent<Enemy>().canHurtPlayer)
            {
                TakeDamage(touchDamage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Portal")
        {
            AudioManager.instance.ChangePitch("Theme", 0.3f);
            portalIn++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
       
        if (collision.tag == "Portal" && portalIn == 1)
        {
            AudioManager.instance.ChangePitch("Theme", 0.73f);
            portalIn--;

        }else if (collision.tag == "Portal" && portalIn > 1)
        {
            portalIn--;
        }
    }

    public void IncreaseCoins()
    {
        coins++;
        coinsText.text = "" + coins;
        return;
    }
    public void IncreaseHealth()
    {
        if (health + healerValue > maxHealth )
        {
            health = maxHealth;
        }
        else
        {
            health += healerValue;
        }
        healthBar.SetHealth(health);
        return;
    }


    public void IncreaseLife()
    {
        lives++;
        livesText.text = " " + lives;
        return;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }

}
