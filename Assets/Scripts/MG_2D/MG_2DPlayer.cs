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
    [SerializeField] private MG_BattleUI BattleUI;

    [Header("피격 설정")]
    [SerializeField] private SpriteRenderer SpriteRenderer_Player;
    [SerializeField] private float _invincibleTime = 1f;
    [SerializeField] private float _knockBackPowerX = 5f;
    [SerializeField] private float _knockBackPowerY = 3f;
    [SerializeField] private float _knockBackStopTime = 0.2f;

    [Header("공격 설정")]
    [SerializeField] private KeyCode _attackKey = KeyCode.J;
    [SerializeField] private float _attackCoolTime = 0.5f;

    private Rigidbody2D _rigidBody;
    private bool _isGrounded;
    private float _horizontalInput;
    private float _attackTimer;
    private bool _lookRight = true;
    private bool _isDead;
    private bool _isInvincible;
    private bool _isDamaged;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();

        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        if(_isDead)
        {
            return;
        }        

        if (_attackTimer > 0f)
        {
            _attackTimer -= Time.deltaTime;
        }

        _horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(_attackKey))
        {
            Attack();
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

        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _checkRadius, _groundLayer);

        Move();
    }

    void Move()
    {
        if (_isDamaged)
        {
            return;
        }
        
        _rigidBody.linearVelocity = new Vector2(_horizontalInput * _moveSpeed, _rigidBody.linearVelocity.y);
    }

    void Jump()
    {
        _rigidBody.linearVelocity = new Vector2(_rigidBody.linearVelocity.x, _jumpForce);
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
    public void TakeDamage(int damage, Vector2 attackerPosition)
    {
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

        if (BattleUI != null)
        {
            BattleUI.RefreshHp();
        }

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

    private void Death()
    {
        _isDead = true;

        _rigidBody.linearVelocity = Vector2.zero;

        MGGameManager.Inst.GameOver();
    }
}
