using UnityEngine;

public class FollowBehaviour : StateMachineBehaviour
{

    private Transform playerPos;
    public float spd;
    private Enemy enemy;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemy = animator.GetComponent<Enemy>();
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (animator.GetBool("PlayerInSight"))
        {
            Vector2 target = new Vector2(playerPos.position.x, animator.transform.position.y);
            animator.transform.position = Vector2.MoveTowards(animator.transform.position, target, spd * Time.deltaTime);


            if (animator.transform.position.x < playerPos.position.x)
            {
                //player on right
                if (!enemy.facingRight)
                {
                    enemy.facingRight = true;
                    animator.transform.Rotate(0f, 180f, 0f);
                }
            }
            else if (animator.transform.position.x > playerPos.position.x)
            {
                //player on left
                if (enemy.facingRight)
                {
                    enemy.facingRight = false;
                    animator.transform.Rotate(0f, 180f, 0f);
                }

            }

        }
        else
        {
            animator.SetTrigger("Idle");
        }


        

        

    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

}
