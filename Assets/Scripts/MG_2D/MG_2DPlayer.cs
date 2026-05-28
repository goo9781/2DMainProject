using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class MG_2DPlayer : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float _moveSpeed = 8f;
    [SerializeField] private float _jumpForce = 12f;

    [Header("지면 체크 설정")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _checkRadius = 0.5f;
    [SerializeField] private LayerMask _groundLayer;

    [Header("애니메이터")]
    [SerializeField] private Animator Animator_Entity;

    [Header("플레이어 상태")]
    [SerializeField] private int _dropItemCount;

    [Header("피격 설정")]
    [SerializeField] private SpriteRenderer SpriteRenderer_Player;
    [SerializeField] private float _invincibleTime = 1f;
    [SerializeField] private float _knockBackPowerX = 5f;
    [SerializeField] private float _knockBackPowerY = 3f;
    [SerializeField] private float _knockBackStopTime = 0.2f;

    private Rigidbody2D _rigidBody;
    private float _horizontalInput;

    private bool _isGrounded;
    private bool _lookRight = true;
    private bool _isDead;
    private bool _isInvincible;
    private bool _isDamaged;

    private MG_2DPlayerAttack _playerAttack;

    private bool IsPlayingState()
    {
        if (MGGameManager.Inst == null)
        {
            return false;
        }

        return MGGameManager.Inst.CurrentState == MGGameState.Playing;
    }

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _playerAttack = GetComponent<MG_2DPlayerAttack>();

        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        if(_isDead)
        {
            return;
        }     
        
        if (IsPlayingState() == false)
        {
            _horizontalInput = 0f;
            UpdatePlayerAnimState();
            return;
        }

        _horizontalInput = Input.GetAxisRaw("Horizontal");

        if (IsGuardMoveLocked())
        {
            _horizontalInput = 0f;
            UpdatePlayerAnimState();
            return;
        }

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            Jump();
        }

        if (_horizontalInput > 0 && !_lookRight)
        {
            Flip();
        }
        else if (_horizontalInput < 0 && _lookRight)
        {
            Flip();
        }

        UpdatePlayerAnimState();

    }

    void FixedUpdate()
    {
        if(_isDead)
        {
            _rigidBody.linearVelocity = Vector2.zero;
            return;
        }

        if (IsPlayingState() == false)
        {
            _rigidBody.linearVelocity = new Vector2(0f, _rigidBody.linearVelocity.y);
            return;
        }

        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _checkRadius, _groundLayer);

        Move();
    }

    void Move()
    {
        if (IsGuardMoveLocked())
        {
            _rigidBody.linearVelocity = new Vector2(0f, _rigidBody.linearVelocity.y);
            return;
        }
        
        if (_isDamaged)
        {
            return;
        }
        
        _rigidBody.linearVelocity = new Vector2(_horizontalInput * _moveSpeed, _rigidBody.linearVelocity.y);
    }

    private bool IsGuardMoveLocked()
    {
        if (_playerAttack == null)
        {
            return false;
        }

        if (_playerAttack.IsGuarding == false)
        {
            return false;
        }

        return _playerAttack.IsMoveLockOnGuard;
    }

    void Jump()
    {
        _rigidBody.linearVelocity = new Vector2(_rigidBody.linearVelocity.x, _jumpForce);
    }

    void Flip()
    {
        _lookRight = !_lookRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void OnDrawGizmos()
    {
        if(_groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_groundCheck.position, _checkRadius);
        }
    }

    private void UpdatePlayerAnimState()
    {
        if(Animator_Entity == null)
        {
            return;
        }

        bool isJump = _isGrounded == false;
        bool isMove = _horizontalInput != 0 && _isGrounded;

        Animator_Entity.SetBool("IsMove", isMove);
        Animator_Entity.SetBool("IsJump", isJump);
    }

    public void AddDropItemCount(int count)
    {
        _dropItemCount += count;

        Debug.Log($"플레이어 상태 변경 - 드랍 아이템 개수: {_dropItemCount}");
    }

    public void TakeDamage(int damage)
    {
        TakeDamage(damage, transform.position);
    }

    public void TakeDamageFromMonster(int damage, Vector2 attackerPosition, MG_Monster attackerMonster)
    {
        if (IsPlayingState() == false)
        {
            return;
        }

        if (_isDead)
        {
            return;
        }

        if (_playerAttack != null)
        {
            bool isReflectSuccess = _playerAttack.TryReflectMonsterAttack(attackerMonster);

            if (isReflectSuccess)
            {
                return;
            }
        }

        TakeDamage(damage, attackerPosition);
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

        if (_isInvincible)
        {
            return;
        }

        MGPlayerModel playerModel = MGGameManager.Inst.PlayerModel;

        playerModel.CurrentHp -= damage;

        if (playerModel.CurrentHp <= 0)
        {
            playerModel.CurrentHp = 0;
        }

        OnDamaged(attackerPosition);

        RefreshBattleUI();

        if (playerModel.CurrentHp <= 0)
        {
            Death();
        }

    }

    private void OnDamaged(Vector2 attackerPosition)
    {
        _isInvincible = true;
        _isDamaged = true;

        if (SpriteRenderer_Player != null)
        {
            SpriteRenderer_Player.color = new Color(1f, 1f, 1f, 0.4f);
        }

        float directionX = transform.position.x - attackerPosition.x > 0f ? 1f : -1f;

        if (_rigidBody != null)
        {
            _rigidBody.linearVelocity = Vector2.zero;
            _rigidBody.AddForce(new Vector2(directionX * _knockBackPowerX, _knockBackPowerY), ForceMode2D.Impulse);
        }

        CancelInvoke(nameof(StopKnockBack));
        CancelInvoke(nameof(OffDamaged));

        Invoke(nameof(StopKnockBack), _knockBackStopTime);
        Invoke(nameof(OffDamaged), _invincibleTime);
    }
    
    private void StopKnockBack()
    {
        _isDamaged = false;

        if (_rigidBody != null)
        {
            _rigidBody.linearVelocity = new Vector2(0f, _rigidBody.linearVelocity.y);
        }
    }

    private void OffDamaged()
    {
        _isInvincible = false;

        if (SpriteRenderer_Player != null)
        {
            SpriteRenderer_Player.color = new Color(1f, 1f, 1f, 1f);
        }
    }

    private void RefreshBattleUI()
    {
        MG_BattleUI battleUI = FindFirstObjectByType<MG_BattleUI>();

        if (battleUI == null)
        {
            return;
        }

        battleUI.RefreshHp();
    }

    private void Death()
    {
        _isDead = true;

        _rigidBody.linearVelocity = Vector2.zero;

        MGGameManager.Inst.GameOver();
    }
}
