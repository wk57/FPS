using UnityEngine;
using UnityEngine.AI;

public class ZombieChaseState : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform player;

    public float chaseSpeed = 6f;
    public float stopChasingDistance = 21f;
    public float attackingDistance = 2.5f;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = chaseSpeed;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (SoundManager.Instance.ZombieChannel.isPlaying == false)
        {
            SoundManager.Instance.ZombieChannel.PlayOneShot(SoundManager.Instance.zombieChase);

        }

        agent.SetDestination(player.position);
        animator.transform.LookAt(player);

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        // stop chase
        if (distanceFromPlayer > stopChasingDistance)
        {
            animator.SetBool("ISCHASING", false);
        }

        //--- atacar

        if (distanceFromPlayer < attackingDistance)
        {
            animator.SetBool("ISATTACKING", true);
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);



        SoundManager.Instance.ZombieChannel.Stop();

        
    }
}
