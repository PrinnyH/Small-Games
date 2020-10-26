using UnityEngine;

public class Sword : MonoBehaviour
{

    public PlayerMovement player;

    private float timeBtwAttack;
    public float startTimeBtwAttack;


    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRange;

    public int damage;

    public Animator animPres;
    public Animator animOld;

    // Update is called once per frame
    void Update()
    {
        if (player.health > 0)
        {
            if (timeBtwAttack <= 0)
            {
                //attack
                if (Input.GetButtonDown("Fire1"))
                {
                    animPres.Play("Base Layer.attackRight", 0, 0);
                    animOld.Play("Base Layer.attackRight", 0, 0);
                    Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                    for (int i = 0; i < enemiesToDamage.Length; i++)
                    {
                        if (enemiesToDamage[i].tag == "Enemy")
                        {
                            AudioManager.instance.Play("PlayerHitNoise");
                            enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
                        }
                    }
                    timeBtwAttack = startTimeBtwAttack;
                }

            }
            else
            {
                timeBtwAttack -= Time.deltaTime;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }


}
