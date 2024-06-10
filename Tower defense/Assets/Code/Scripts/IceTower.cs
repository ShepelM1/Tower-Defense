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

    private void Update()
    {
        timeUntilFire += Time.deltaTime;

        if (timeUntilFire >= 1f / aps)
        {
            FreezeEnemies();
            timeUntilFire = 0f;
        }
    }

    private void FreezeEnemies()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyMask);

        if (hits.Length > 0)
        {
            foreach (Collider2D hit in hits)
            {
                EnemyMovement em = hit.GetComponent<EnemyMovement>();
                if (em != null)
                {
                    em.UpdateSpeed(0.5f);
                    StartCoroutine(ResetEnemySpeed(em));
                }
            }
        }
    }

    private IEnumerator ResetEnemySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(freezeTime);
        em.ResetSpeed();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
#endif
}