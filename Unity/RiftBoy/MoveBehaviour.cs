using UnityEngine;

public class MoveBehaviour : StateMachineBehaviour
{

    private float timer;
    public float timerValue;
    private Enemy enemy;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Enemy>();
        timer = timerValue;

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (timer <= 0)
        {
            animator.SetTrigger("Idle");
        }
        else
        {
            timer -= Time.deltaTime;
        }


        if (enemy.state == 0)
        {
            animator.transform.position = Vector2.MoveTowards(animator.transform.position, enemy.startPos + new Vector2 (enemy.walkDistance, 0), (enemy.walkDistance / timerValue) * Time.deltaTime);
            
            if (!enemy.facingRight)
            {
                enemy.transform.Rotate(0f, 180f, 0f);
                enemy.facingRight = true;
            }
        }
        else
        {
            animator.transform.position = Vector2.MoveTowards(animator.transform.position, enemy.startPos - new Vector2(enemy.walkDistance, 0), (enemy.walkDistance / timerValue) * Time.deltaTime);
            if (enemy.facingRight)
            {
                enemy.transform.Rotate(0f, 180f, 0f);
                enemy.facingRight = false;
            }
        }
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Move");
        enemy.state = 1 - enemy.state;

    }
}



