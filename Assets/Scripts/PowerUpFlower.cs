using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpFlower : MonoBehaviour
{
    Collider2D flowerCollider;
    private PlayerStates playerStates;

    public GameObject particle;
    Animator animator;
    public AudioSource powerupaudio;

    public int addForce = 5;

    void Start()
    {
        flowerCollider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerStates = GameObject.FindWithTag("Player").GetComponent<PlayerStates>();
        if(collision.tag == "Player")
        {
            powerupaudio.Play();
            playerStates.ReinforcedDamage += addForce;
            CharacterEvents.characterPowerUp(gameObject, addForce);   //ダメージアップを表示
            animator.SetBool(AnimationStrings.isAlive, false);  //枯れるアニメーション
            flowerCollider.enabled = false;
            Destroy(particle);
        }
    }
}
