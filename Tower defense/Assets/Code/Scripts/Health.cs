using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Attribute")]
    [SerializeField] public int hitPoints = 2;
    [SerializeField] private int currencyWorth = 50;

    [Header("References")]
    [SerializeField] private Animator animator;

    private AudioManager audioManager;

    private bool isDestroyed = false;
    private float deathAnimationTime;
    private bool isDeathAnimationStarted = false;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void Update()
    {
        if (isDeathAnimationStarted)
        {
            deathAnimationTime -= Time.deltaTime;
            if (deathAnimationTime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void TakeDamage(int dmg)
    {
        hitPoints -= dmg;

        if (hitPoints <= 0 && !isDestroyed)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            LevelManager.main.IncreaseCurrency(currencyWorth);
            isDestroyed = true;
            GetComponent<EnemyMovement>().ResetSpeed();
            audioManager.PlaySFX(audioManager.death);
            animator.SetBool("isDead", true);
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            deathAnimationTime = stateInfo.length;
            isDeathAnimationStarted = true;
        }
    }
}