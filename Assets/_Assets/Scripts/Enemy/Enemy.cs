using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour

{
    [SerializeField] private int HP = 100;
    private NavMeshAgent navAgent;
    private Animator animator;

    public bool isDead;

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
            
            isDead = true;
            //dead ssss
            SoundManager.Instance.ZombieChannel2.PlayOneShot(SoundManager.Instance.zombieDeath);
        }
        else
        {
            animator.SetTrigger("DAMAGE");

            //hurt SSS
            SoundManager.Instance.ZombieChannel2.PlayOneShot(SoundManager.Instance.zombieHurt);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 18f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 21f);
    }
}
