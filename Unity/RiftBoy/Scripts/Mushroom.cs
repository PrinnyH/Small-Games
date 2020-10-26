
public class Mushroom : Enemy
{

    override public void TakeDamage(int damage)
    {
            if (inv <= 0 && !playerInView)
            {
                health -= damage;
                anim.Play("Base Layer.hurt");
                healthBar.SetHealth(health);
                inv = invValue;
            }
    }

    public override void canHurtPlayerCheck()
    {
        if (playerInView)
        {
            canHurtPlayer = false;
        }else
        {
            canHurtPlayer = true;
        }
    }
    

}
