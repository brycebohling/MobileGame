using UnityEngine;

public class AIAlert : AIBase
{
    [SerializeField] float alertRadius;
    [SerializeField] LayerMask enemyLayer;

    bool _wasAlerted;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        _healthScript.OnDamaged += OnDamaged;
    }

    protected override void OnDisable()
    {
        _healthScript.OnDamaged -= OnDamaged;
    }

    private void OnDamaged(float dmg, float currentHealth, float knockBackForce, Vector2 senderPos)
    {
        if (!_wasAlerted)
        {
            AlertNearbyEnemies();
        }

        _wasAlerted = false;
    }

    public void OnAggro()
    {
        if (!_wasAlerted)
        {
            AlertNearbyEnemies();
        }
    }

    public void AlertedByAlly()
    {
        _wasAlerted = true;

        AIChase chase = GetComponent<AIChase>();
        if (chase != null)
        {
            chase.Damaged();
        }
    }

    private void AlertNearbyEnemies()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, alertRadius, enemyLayer);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].gameObject == gameObject) continue;

            AIAlert otherAlert = hits[i].GetComponent<AIAlert>();
            if (otherAlert != null)
            {
                otherAlert.AlertedByAlly();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, alertRadius);
    }
}
