using UnityEngine;

public class MG_DeadZone : MonoBehaviour
{
    private bool _isFailed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isFailed)
        {
            return;
        }

        MG_2DPlayer player = collision.GetComponent<MG_2DPlayer>();

        if (player == null)
        {
            return;
        }

        _isFailed = true;

        MGGameManager.Inst.FailGame();
    }
}
