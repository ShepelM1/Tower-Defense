using UnityEngine;
using UnityEngine.UI;

public class BallistaTowerLv2 : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject upgradePrefabLv3;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Animator animator;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float bps = 1f;
    [SerializeField] private int upgradeCost = 100;

    private Transform target;
    private float timeUntilFire;

    private void Start()
    {
        upgradeButton.onClick.AddListener(Upgrade);
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
            animator.SetBool("IsAiming", false); 
        }
        else
        {
            animator.SetBool("IsAiming", true); 

            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / bps)
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

    public void OpenUpgradeUI()
    {
        upgradeUI.SetActive(true);
    }

    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
            animator.SetBool("IsAiming", true);
        }
        else
        {
            animator.SetBool("IsAiming", false);
        }
    }

    private bool CheckTargetIsInRange()
    {
        return target != null && Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void RotateTowardsTarget()
    {
        if (target == null) return;

        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        turretRotationPoint.rotation = Quaternion.Slerp(turretRotationPoint.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    public void Upgrade()
    {
        if (LevelManager.main.currency < upgradeCost) return;

        LevelManager.main.SpendCurrency(upgradeCost);
        GameObject upgradedTower = Instantiate(upgradePrefabLv3, transform.position, transform.rotation);

        Destroy(gameObject);
        CloseUpgradeUI();
    }
}
