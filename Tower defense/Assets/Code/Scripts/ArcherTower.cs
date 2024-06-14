using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArherTower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private SpriteRenderer turretSpriteRenderer;
    [SerializeField] private Sprite[] turretSprites;
    [SerializeField] private SpriteRenderer arherSpriteRenderer;
    [SerializeField] private Sprite[] archerSprites; // Additional sprites for upgrades

    [Header("Attributes")]
    [SerializeField] private float[] targetingRanges;
    [SerializeField] private float[] rotationSpeeds;
    [SerializeField] private float[] bpsLevels; // Bullets Per Second for each level
    [SerializeField] private int[] upgradeCosts;

    

    private float timeUntilFire;
    private Transform target;
    private int level = 0;

    private void Start()
    {
        upgradeButton.onClick.AddListener(Upgrade);
        SetTurretSprites(level); // Initialize sprite on start
    }

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / bpsLevels[level])
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRanges[level], (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRanges[level];
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeeds[level] * Time.deltaTime);
    }

    public void OpenUpgradeUI()
    {
        upgradeUI.SetActive(true);
    }

    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);
    }

    public void Upgrade()
    {
        if (level >= upgradeCosts.Length - 1 || LevelManager.main.currency < upgradeCosts[level]) return;

        LevelManager.main.SpendCurrency(upgradeCosts[level]);
        level++;

        SetTurretSprites(level); // Update turret sprite

        CloseUpgradeUI();
    }

    private void SetTurretSprites(int currentLevel)
    {
        if (currentLevel < turretSprites.Length)
        {
            turretSpriteRenderer.sprite = turretSprites[currentLevel];
        }

        if (currentLevel < archerSprites.Length)
        {
            if (arherSpriteRenderer != null)
            {
                arherSpriteRenderer.sprite = archerSprites[currentLevel];
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (targetingRanges.Length > 0)
        {
            UnityEditor.Handles.color = Color.cyan;
            UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, targetingRanges[level]);
        }
    }
#endif
}
