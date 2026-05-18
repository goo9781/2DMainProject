using UnityEngine;

public class MG_Monster : MonoBehaviour
{
    [Header("몬스터 상태")]
    [SerializeField] private int _maxHp = 30;
    [SerializeField] private int _currentHp;

    private bool _isDead;

    private void Awake()
    {
        _currentHp = _maxHp;
    }

    public void TakeDamage(int damage)
    {
        if (_isDead)
        {
            return;
        }

        _currentHp -= damage;

        if (_currentHp <= 0)
        {
            _currentHp = 0;
        }

        Debug.Log($"몬스터 피격 : {damage}. 현재 HP : {_currentHp}");

        if (_currentHp <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        _isDead = true;
        
        Destroy(gameObject);
    }
}
