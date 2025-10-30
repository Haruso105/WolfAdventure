using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    Animator animator;
    public GameObject healflower_particle;
    Collider2D flowerCollider;

    public int healthRestore = 50;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        flowerCollider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if(damageable)
        {
          bool wasHealed = damageable.Heal(healthRestore);
          
          if(wasHealed)
          {
            animator.SetBool(AnimationStrings.isAlive, false);
            Destroy(healflower_particle);
            flowerCollider.enabled = false;
          }
        }
    }
}
