using UnityEngine;

public class MG_FinishPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MG_2DPlayer player = collision.GetComponent<MG_2DPlayer>();

        if(player == null)
        {
            return;
        }

        Debug.Log("플레이어가 목적지에 도달했습니다.");

        MGUIManager.Instance.OpenGameResultUI(true);
    }
}
