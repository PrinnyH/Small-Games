using UnityEngine;

public class PlaySFX : StateMachineBehaviour
{
    public string name;
    public bool stopOnEnd;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AudioManager.instance.Play(name);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stopOnEnd)
        {
            AudioManager.instance.Stop(name);
        }
    }


}
