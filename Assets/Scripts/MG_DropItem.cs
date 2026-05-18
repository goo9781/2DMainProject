using UnityEngine;

public class MG_DropItem : MonoBehaviour
{
    [SerializeField] private int _addCount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MG_2DPlayer player = collision.GetComponent<MG_2DPlayer>();

        if (player == null)
        {
            return;
        }

        player.AddDropItemCount(_addCount);

        Destroy(gameObject);
    }
}
