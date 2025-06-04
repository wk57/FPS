using UnityEngine;

public class ZombieIdleState : StateMachineBehaviour
{

    float timer;
    public float idleTime = 0f;

    Transform player;

    public float detectionalAreaRadious = 18f;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

   // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { //----- pasa a patrullar---------------
        timer += Time.deltaTime;
        if (timer > idleTime)
        {
            animator.SetBool("ISPATROLLING", true);
        }

        //-------- pasa a perseguir -------------

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionalAreaRadious)
        {
            animator.SetBool("ISCHASING", true);
        }

    }

   // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }


}
