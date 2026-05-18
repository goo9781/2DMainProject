using System.Runtime.CompilerServices;
using UnityEngine;

public class MG_2DPlayerAttack : MonoBehaviour
{
    [Header("공격 설정")]
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRadius = 0.7f;
    [SerializeField] private int _attackDamage = 10;
    [SerializeField] private LayerMask _monsterLayer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
    }

    private void Attack()
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

        monster.TakeDamage(_attackDamage);
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
}

