using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MG_Monster : MonoBehaviour
{
    [Header("몬스터 상태")]
    [SerializeField] private int _maxHp = 30;
    [SerializeField] private int _currentHp;

    [Header("애니메이터")]
    [SerializeField] private Animator Animator_Monster;

    [Header("이동 설정")]
    [SerializeField] private float _moveSpeed = 2f;

    [Header("공격 설정")]
    [SerializeField] private int _attackDamage = 10;
    [SerializeField] private float _detectRange = 3f;
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private float _attackCoolTime = 1.5f;

    private Rigidbody2D _rigidBody;
    private Transform _player;
    private float _attackTimer;
    private float _moveDirectionX;
    private Vector3 _originScale;

    private bool _isDead;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;

        _originScale = transform.localScale;
        
        _currentHp = _maxHp;
    }

    private void Update()
    {
        if (_isDead)
        {
            return;
        }

        if (_attackTimer > 0f)
        {
            _attackTimer -= Time.deltaTime;
        }

        FindPlayer();

        if (_player == null)
        {
            StopMove();
            return;
        }

        float distance = Vector2.Distance(transform.position, _player.position);

        if (distance <= _attackRange)
        {
            StopMove();
            TryAttack();
        }

        else if (distance <= _detectRange)
        {
            ChasePlayer();
        }

        else
        {
            StopMove();
        }

    }

    private void FixedUpdate()
    {
        if (_isDead)
        {
            return;
        }

        Move();
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

    private void ChasePlayer()
    {
        float directionX = _player.position.x - transform.position.x;

        if (directionX > 0f)
        {
            _moveDirectionX = 1f;
        }

        else if (directionX < 0f)
        {
            _moveDirectionX = -1f;
        }

        else
        {
            _moveDirectionX = 0f;
        }

        SetMove(true);
        LookAtMoveDirection();
    }

    private void StopMove()
    {
        _moveDirectionX = 0f;
        SetMove(false);
    }

    private void Move()
    {
        _rigidBody.linearVelocity = new Vector2(_moveDirectionX * _moveSpeed, _rigidBody.linearVelocity.y);
    }

    private void TryAttack()
    {
        if (_attackTimer > 0f)
        {
            return;
        }

        _attackTimer = _attackCoolTime;

        PlayAttack();
        AttackPlayer();
    }

    private void AttackPlayer()
    {
        if (_player == null)
        {
            return;
        }

        MG_2DPlayer player = _player.GetComponent<MG_2DPlayer>();

        if (player == null)
        {
            return;
        }

        float distance = Vector2.Distance(transform.position, _player.position);

        if (distance > _attackRange)
        {
            return;
        }

        player.TakeDamage(_attackDamage);
    }

    private void LookAtMoveDirection()
    {
        if (_moveDirectionX > 0f)
        {
            transform.localScale = new Vector3(Mathf.Abs(_originScale.x), _originScale.y, _originScale.z);
        }

        else if (_moveDirectionX < 0f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(_originScale.x), _originScale.y, _originScale.z);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
