using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour

{
    [SerializeField] private int HP = 100;
    private NavMeshAgent navAgent;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;

        if (HP <= 0)
        {
            int randomValue = Random.Range(0, 2);
            
            if (randomValue == 0)
            {
                animator.SetTrigger("DIE1");
            }
            else
            {
                animator.SetTrigger("DIE2");
            }     
            

        }
        else
        {
            animator.SetTrigger("DAMAGE");
        }
    }

    private void Update()
    {
        if (navAgent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("ISWALKING", true);
        }
        else
        {
            animator.SetBool("ISWALKING", false);
        }
    }
}
