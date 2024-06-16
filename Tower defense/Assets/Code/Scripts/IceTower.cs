using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class IceTower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float aps = 4f;
    [SerializeField] private float freezeTime = 1f;

    private float timeUntilFire;
    private List<EnemyMovement> enemiesInRange = new List<EnemyMovement>();

    private void Update()
    {
        timeUntilFire += Time.deltaTime;

        if (timeUntilFire >= 1f / aps)
        {
            UpdateEnemiesInRange();
            FreezeEnemies();
            timeUntilFire = 0f;
        }
    }

    private void UpdateEnemiesInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyMask);
        List<EnemyMovement> currentEnemies = new List<EnemyMovement>();

        foreach (Collider2D hit in hits)
        {
            EnemyMovement em = hit.GetComponent<EnemyMovement>();
            if (em != null)
            {
                currentEnemies.Add(em);
                if (!enemiesInRange.Contains(em))
                {
                    em.UpdateSpeed(0.5f);
                    enemiesInRange.Add(em);
                }
            }
        }

        foreach (EnemyMovement em in enemiesInRange.ToArray())
        {
            if (!currentEnemies.Contains(em))
            {
                em.ResetSpeed();
                enemiesInRange.Remove(em);
            }
        }
    }

    private void FreezeEnemies()
    {
        foreach (EnemyMovement em in enemiesInRange)
        {
            em.UpdateSpeed(0.5f);
            StartCoroutine(ResetEnemySpeed(em));
        }
    }

    private IEnumerator ResetEnemySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(freezeTime);
        if (enemiesInRange.Contains(em))
        {
            em.UpdateSpeed(0.5f);
        }
        else
        {
            em.ResetSpeed();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
#endif
}
