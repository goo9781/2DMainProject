using UnityEngine;

public class MG_2DPlayerAttack : MonoBehaviour
{
    [Header("애니메이터")]
    [SerializeField] private Animator Animator_Entity;

    [Header("방어 설정")]
    [SerializeField] private KeyCode _guardKey = KeyCode.J;
    [SerializeField] private bool _isMoveLockOnGuard = true;

    private bool _isGuarding;

    public bool IsGuarding
    {
        get { return _isGuarding; }
    }

    public bool IsMoveLockOnGuard
    {
        get { return _isMoveLockOnGuard; }
    }

    private void Update()
    {
        if (IsPlayingState() == false)
        {
            EndGuard();
            return;
        }

        if (Input.GetKeyDown(_guardKey))
        {
            StartGuard();
        }

        if (Input.GetKeyUp(_guardKey))
        {
            EndGuard();
        }
    }

    private bool IsPlayingState()
    {
        if (MGGameManager.Inst == null)
        {
            return false;
        }

        return MGGameManager.Inst.CurrentState == MGGameState.Playing;
    }

    private void StartGuard()
    {
        if (_isGuarding)
        {
            return;
        }

        _isGuarding = true;

        Debug.Log("방어 시작");
    }

    private void EndGuard()
    {
        if (_isGuarding == false)
        {
            return;
        }

        _isGuarding = false;

        Debug.Log("방어 종료");
    }
}

