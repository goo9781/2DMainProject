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

    private Rigidbody2D _rigidBody;
    private bool _isGrounded;
    private float _horizontalInput;
    private bool _lookRight = true;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();

        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");

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
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _checkRadius, _groundLayer);

        Move();
    }

    void Move()
    {
        _rigidBody.linearVelocity = new Vector2(_horizontalInput * _moveSpeed, _rigidBody.linearVelocity.y);
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
        MGPlayerModel playerModel = MGGameManager.Inst.PlayerModel;

        playerModel.CurrentHp -= damage;

        if (playerModel.CurrentHp <= 0)
        {
            playerModel.CurrentHp = 0;

            Death();
        }
    }

    private void Death()
    {
        MGGameManager.Inst.GameOver();
    }
}
