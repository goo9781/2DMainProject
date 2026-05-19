using UnityEngine;

public class MG_Monster : MonoBehaviour
{
    [Header("몬스터 상태")]
    [SerializeField] private int _maxHp = 30;
    [SerializeField] private int _currentHp;

    [Header("애니메이터")]
    [SerializeField] private Animator Animator_Monster;

    [Header("공격 설정")]
    [SerializeField] private float _detectRange = 3f;
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private float _attackCoolTime = 1.5f;

    private Transform _player;
    private float _attackTimer;

    private bool _isDead;

    private void Awake()
    {
        _currentHp = _maxHp;
    }

    private void Update()
    {
        if (_isDead)
        {
            return;
        }

        FindPlayer();

        if (_player == null)
        {
            SetMove(false);
            return;
        }

        float distance = Vector2.Distance(transform.position, _player.position);

        if (distance <= _attackRange)
        {
            SetMove(false);
            TryAttack();
        }

        else if (distance <= _detectRange)
        {
            SetMove(true);
        }

        else
        {
            SetMove(false);
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

    private void FindPlayer()
    {
        if (_player != null)
        {
            return;
        }

        MG_2DPlayer player = FindFirstObjectByType<MG_2DPlayer>();

        if (player == null)
        {
            return;
        }

        _player = player.transform;
   
    }

    private void TryAttack()
    {
        _attackTimer -= Time.deltaTime;

        if (_attackTimer > 0f)
        {
            return;
        }

        _attackTimer = _attackCoolTime;

        PlayAttack();
    }
}
