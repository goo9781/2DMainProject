using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MG_Monster : MonoBehaviour
{
    [Header("데이터 설정")]
    [SerializeField] private string _monsterDataId;
    
    [Header("몬스터 스탯")]
    [SerializeField] private int _maxHp = 30;
    [SerializeField] private int _currentHp;
    [SerializeField] private int _attackDamage = 1;

    [Header("애니메이터")]
    [SerializeField] private Animator Animator_Monster;

    [Header("이동 설정")]
    [SerializeField] private float _moveSpeed = 2f;

    [Header("공격 설정")]
    [SerializeField] private float _detectRange = 3f;
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private float _attackCoolTime = 1.5f;

    [Header("피격 설정")]
    [SerializeField] private SpriteRenderer SpriteRenderer_Monster;
    [SerializeField] private float _hitFlashTime = 0.1f;

    [Header("피격 넉백 설정")]
    [SerializeField] private float _hitKnockBackPowerX = 3f;
    [SerializeField] private float _hitKnockBackStopTime = 0.15f;

    [Header("순찰 설정")]
    [SerializeField] private float _patrolRange = 2f;
    [SerializeField] private float _patrolSpeed = 1.5f;

    private Rigidbody2D _rigidBody;
    private Transform _player;
    private MGMonsterData _monsterData;

    private float _attackTimer;
    private float _moveDirectionX;
    private float _patrolDirectionX = 1f;
    private float _currentMoveSpeed;

    private Vector3 _originScale;
    private Vector3 _patrolCenterPosition;

    private int _instanceId;

    private bool _isDead;
    private bool _isDamaged;
    private bool _isRegisteredObject;

    private bool IsPlayingState()
    {
        if (MGGameManager.Inst == null)
        {
            return false;
        }

        return MGGameManager.Inst.CurrentState == MGGameState.Playing;
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;

        _originScale = transform.localScale;
        
        _currentHp = _maxHp;
    }

    private void Start()
    {
        _patrolCenterPosition = transform.position;
        _currentMoveSpeed = _patrolSpeed;
    }

    private void Update()
    {
        if (_isDead)
        {
            return;
        }

        if (IsPlayingState() == false)
        {
            StopMove();
            return;
        }

        if (_isDamaged)
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
            Patrol();
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
            Patrol();
        }

    }

    private void FixedUpdate()
    {
        if (_isDead)
        {
            return;
        }

        if (IsPlayingState() == false)
        {
            if (_rigidBody != null)
            {
                _rigidBody.linearVelocity = new Vector2(0f, _rigidBody.linearVelocity.y);
            }

            return;
        }

        Move();
    }

    public void InitMonsterData(string monsterDataId)
    {
        _monsterDataId = monsterDataId;

        if (string.IsNullOrEmpty(_monsterDataId))
        {
            Debug.LogWarning($"{gameObject.name} 몬스터 데이터ID가 비어있습니다.");
            return;
        }

        if (MGGameDataManager.Inst == null)
        {
            Debug.LogError("MGGameDataManager.Inst가 없습니다. 몬스터 데이터를 적용할 수 없습니다.");
            return;
        }

        _monsterData = MGGameDataManager.Inst.GetMonsterData(_monsterDataId);

        if (_monsterData == null)
        {
            Debug.LogError($"몬스터 데이터를 찾을 수 없습니다. DataId : {_monsterDataId}");
            return;
        }

        ApplyMonsterData(_monsterData);
    }

    private void ApplyMonsterData(MGMonsterData monsterData)
    {
        _maxHp = monsterData.MaxHp;
        _currentHp = _maxHp;

        _attackDamage = monsterData.AttackDamage;

        _moveSpeed = monsterData.MoveSpeed;
        _patrolSpeed = monsterData.PatrolSpeed;
        _patrolRange = monsterData.PatrolRange;

        _detectRange = monsterData.DetectRange;
        _attackRange = monsterData.AttackRange;
        _attackCoolTime = monsterData.AttackCoolTime;

        _currentMoveSpeed = _patrolSpeed;

        Debug.Log($"몬스터 데이터 적용 완료 : {_monsterDataId} / {monsterData.Name}");
    }

    public void TakeDamage(int damage)
    {
        TakeDamage(damage, transform.position);
    }


    public void TakeDamage(int damage, Vector2 attackerPosition)
    {
        if (IsPlayingState() == false)
        {
            return;
        }
        
        if (_isDead)
        {
            return;
        }

        _currentHp -= damage;

        if (_currentHp <= 0)
        {
            _currentHp = 0;
        }

        PlayHitFlash();
        PlayHitKnockBack(attackerPosition);

        Debug.Log($"몬스터 피격 : {damage}. 현재 HP : {_currentHp}");

        if (_currentHp <= 0)
        {
            Death();
        }
    }

    private void PlayHitFlash()
    {
        if (SpriteRenderer_Monster == null)
        {
            return;
        }

        SpriteRenderer_Monster.color = new Color(1f, 1f, 1f, 0.4f);

        CancelInvoke(nameof(OffHitFlash));
        Invoke(nameof(OffHitFlash), _hitFlashTime);
    }

    private void OffHitFlash()
    {
        if (SpriteRenderer_Monster == null)
        {
            return;
        }

        SpriteRenderer_Monster.color = new Color(1f, 1f, 1f, 1f);
    }

    private void PlayHitKnockBack(Vector2 attackerPosition)
    {
        _isDamaged = true;
        _moveDirectionX = 0f;
        SetMove(false);

        float directionX = transform.position.x - attackerPosition.x > 0f ? 1f : -1f;

        if (_rigidBody != null)
        {
            _rigidBody.linearVelocity = new Vector2(0f, _rigidBody.linearVelocity.y);
            _rigidBody.AddForce(new Vector2(directionX * _hitKnockBackPowerX, 0f), ForceMode2D.Impulse);
        }

        CancelInvoke(nameof(StopHitKnockBack));
        Invoke(nameof(StopHitKnockBack), _hitKnockBackStopTime);
    }

    private void StopHitKnockBack()
    {
        _isDamaged = false;
        
        if (_rigidBody == null)
        {
            return;
        }

        _rigidBody.linearVelocity = new Vector2(0f, _rigidBody.linearVelocity.y);
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
        
        if (_isRegisteredObject && MGGameObjectManager.Inst != null)
        {
            MGGameObjectManager.Inst.RequestDestroyGameObject(_instanceId);
            return;
        }

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
       if (_isDamaged)
        {
            return;
        }
        
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

        _currentMoveSpeed = _moveSpeed;

        SetMove(true);
        LookAtMoveDirection();
    }

    private void StopMove()
    {
        _moveDirectionX = 0f;
        _currentMoveSpeed = 0f;
        SetMove(false);
    }

    private void Move()
    {
        if(_isDamaged)
        {
            return; 
        }
        
        _rigidBody.linearVelocity = new Vector2(_moveDirectionX * _currentMoveSpeed, _rigidBody.linearVelocity.y);
    }

    private void TryAttack()
    {
        if (_attackTimer > 0f)
        {
            return;
        }

        _attackTimer = _attackCoolTime;

        PlayAttack();
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

        player.TakeDamageFromMonster(_attackDamage, transform.position, this);
    }

    public void AnimationEvent_MonsterAttackHit()
    {
        AttackPlayer();
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

    private void Patrol()
    {
        if (_patrolRange <= 0f)
        {
            StopMove();
            return;
        }

        float leftLimit = _patrolCenterPosition.x - _patrolRange;
        float rightLimit = _patrolCenterPosition.x + _patrolRange;

        if (transform.position.x <= leftLimit)
        {
            _patrolDirectionX = 1f;
        }
        else if (transform.position.x >= rightLimit)
        {
            _patrolDirectionX = -1f;
        }

        _moveDirectionX = _patrolDirectionX;
        _currentMoveSpeed = _patrolSpeed;

        SetMove(true);
        LookAtMoveDirection();
    }

    public void InitMonsterInfo(int instanceId)
    {
        _instanceId = instanceId;
        _isRegisteredObject = true;
    }

    //몬스터의 판정 범위 시각화(플레이어 추격, 공격, 순찰 범위)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);

        Vector3 patrolCenter = Application.isPlaying ? _patrolCenterPosition : transform.position;
        Vector3 leftPoint = patrolCenter + Vector3.left * _patrolRange;
        Vector3 rightPoint = patrolCenter + Vector3.right * _patrolRange;

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(leftPoint, rightPoint);
        Gizmos.DrawWireSphere(leftPoint, 0.1f);
        Gizmos.DrawWireSphere(rightPoint, 0.1f);
    }
}
