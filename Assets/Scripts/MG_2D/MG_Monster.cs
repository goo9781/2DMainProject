using UnityEngine;

public class MG_Monster : MonoBehaviour
{
    [Header("몬스터 상태")]
    [SerializeField] private int _maxHp = 30;
    [SerializeField] private int _currentHp;

    [Header("애니메이터")]
    [SerializeField] private Animator Animator_Monster;

    private bool _isDead;

    private void Awake()
    {
        _currentHp = _maxHp;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetMove(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetMove(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayAttack();
        }
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

    public void SetMove(bool isMove)
    {
        if (_isDead)
        {
            return;
        }

        if (Animator_Monster !=null)
        {
            Animator_Monster.SetBool("IsMove", isMove);
        }
    }

    public void PlayAttack()
    {
        if (_isDead)
        {
            return;
        }

        if (Animator_Monster != null)
        {
            Animator_Monster.SetTrigger("Attack");
        }
    }

    public void Death()
    {
        if (_isDead)
        {
            return;
        }
        
        _isDead = true;
        
        Destroy(gameObject);
    }
}
