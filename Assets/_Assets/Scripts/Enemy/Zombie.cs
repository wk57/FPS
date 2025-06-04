using UnityEngine;

public class Zombie : MonoBehaviour
{
   public ZombieHand hand;
  
   public int zombieDamage;

    private void Start()
    {
        hand.damage = zombieDamage;
    }
}
