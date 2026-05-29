using UnityEngine;

public class MG_MonsterProjectile : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 5f;

    private Vector2 _moveDirection;
    private float _moveSpeed;
    private int _damage;
    private MG_Monster _ownerMonster;

    private bool _isInit;

    public void InitProjectile(Vector2 moveDirection, float moveSpeed, int damage, MG_Monster ownerMonster)
    {
        _moveDirection = moveDirection.normalized;
        _moveSpeed = moveSpeed;
        _damage = damage;
        _ownerMonster = ownerMonster;

        _isInit = true;

        Destroy(gameObject, _lifeTime);
    }

    private void Update()
    {
        if (_isInit == false)
        {
            return;
        }

        MoveProjectile();
    }

    private void MoveProjectile()
    {
        transform.position += (Vector3)(_moveDirection * _moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MG_2DPlayer player = collision.GetComponent<MG_2DPlayer>();

        if (player != null)
        {
            player.TakeDamageFromMonster(_damage, transform.position, _ownerMonster);

            Destroy(gameObject);
            return;
        }

        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
            return;
        }
    }


}
