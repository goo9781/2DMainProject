using UnityEngine;

public class MG_Trap : MonoBehaviour
{
    [SerializeField] private int _damage = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MG_2DPlayer player = collision.GetComponent<MG_2DPlayer>();

        if (player == null)
        {
            return;
        }

        player.TakeDamage(_damage);
    }
}
