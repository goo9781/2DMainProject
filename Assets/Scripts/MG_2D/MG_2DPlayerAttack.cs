using UnityEngine;

public class MG_2DPlayerAttack : MonoBehaviour
{
    [Header("애니메이터")]
    [SerializeField] private Animator Animator_Entity;
    
    [Header("공격 설정")]
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRadius = 0.7f;
    [SerializeField] private int _attackDamage = 10;
    [SerializeField] private LayerMask _monsterLayer;
    [SerializeField] private KeyCode _attackKey = KeyCode.J;
    [SerializeField] private float _attackCoolTime = 0.5f;

    private float _attackTimer;

    private void Update()
    {
       if (_attackTimer > 0f)
        {
            _attackTimer -= Time.deltaTime;
        }
        
        if (Input.GetKeyDown(_attackKey))
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (_attackTimer > 0f)
        {
            return;
        }

        _attackTimer = _attackCoolTime;

        PlayAttackAnimation();
    }

    private void PlayAttackAnimation()
    {
        if (Animator_Entity == null)
        {
            return;
        }

        Animator_Entity.SetTrigger("Attack");
    }

        private void OnDrawGizmos()
    {
        if (_attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRadius);
    }

    public void AnimationEvent_AttackHit()
    {
        Collider2D hitCollider = Physics2D.OverlapCircle(
            _attackPoint.position,
            _attackRadius,
            _monsterLayer
            );

        if (hitCollider == null)
        {
            return;
        }

        MG_Monster monster = hitCollider.GetComponent<MG_Monster>();

        if (monster == null)
        {
            return;
        }

        Debug.Log("플레이어 공격 성공");

        monster.TakeDamage(_attackDamage, transform.position);
    }
}

