using UnityEngine;

public class MG_2DPlayerAttack : MonoBehaviour
{
    [Header("애니메이터")]
    [SerializeField] private Animator Animator_Entity;

    [Header("방어 설정")]
    [SerializeField] private KeyCode _guardKey = KeyCode.J;
    [SerializeField] private bool _isMoveLockOnGuard = true;
    [SerializeField] private int _guardReflectDamage = 999;

    [Header("방어 성공 이펙트")]
    [SerializeField] private GameObject Prefab_GuardSuccessEffect;
    [SerializeField] private Transform Transform_GuardEffectPosition;
    [SerializeField] private float _guardEffectDestroyTime = 1f;

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

        SetGuardAnimation(true);
    }

    private void EndGuard()
    {
        if (_isGuarding == false)
        {
            return;
        }

        _isGuarding = false;

        SetGuardAnimation(false);
    }

    private void SetGuardAnimation(bool isGuard)
    {
        if (Animator_Entity == null)
        {
            return;
        }

        Animator_Entity.SetBool("IsGuard", isGuard);
    }

    private void PlayGuardSuccessEffect()
    {
        if (Prefab_GuardSuccessEffect == null)
        {
            return;
        }

        Vector3 effectPosition = transform.position;

        if (Transform_GuardEffectPosition != null)
        {
            effectPosition = Transform_GuardEffectPosition.position;
        }

        GameObject effectObject = Instantiate(Prefab_GuardSuccessEffect, effectPosition, Quaternion.identity);

        if (_guardEffectDestroyTime > 0f)
        {
            Destroy(effectObject, _guardEffectDestroyTime);
        }
    }

    public bool TryReflectMonsterAttack(MG_Monster attackerMonster)
    {
        if (_isGuarding == false)
        {
            return false;
        }

        if (attackerMonster == null)
        {
            return false;
        }

        PlayGuardSuccessEffect();

        attackerMonster.TakeDamage(_guardReflectDamage, transform.position);

        return true;
    }
}

