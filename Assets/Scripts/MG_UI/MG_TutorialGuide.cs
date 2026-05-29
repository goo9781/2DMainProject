using System.Collections;
using UnityEngine;

public class MG_TutorialGuide : MonoBehaviour
{
    [SerializeField] private float _firstDelayTime = 0.5f;
    [SerializeField] private float _messageShowTime = 2f;

    private MG_BattleUI _battleUI;

    private void Start()
    {
        StartCoroutine(Co_PlayTutorial());
    }

    private IEnumerator Co_PlayTutorial()
    {
        yield return new WaitForSeconds(_firstDelayTime);

        while (_battleUI == null)
        {
            _battleUI = FindFirstObjectByType<MG_BattleUI>();
            yield return null;
        }

        SetTutorialMessage("A/D 키를 눌러 이동하세요.");
        yield return new WaitForSeconds(_messageShowTime);

        SetTutorialMessage("Space 키로 점프하세요.");
        yield return new WaitForSeconds(_messageShowTime);

        SetTutorialMessage("J 키를 누르고 있으면 적의 공격을 방어합니다.");
        yield return new WaitForSeconds(_messageShowTime);

        SetTutorialMessage("몬스터의 공격을 방어하면 반격합니다.");
        yield return new WaitForSeconds(_messageShowTime);

        SetTutorialMessage("목적지까지 도달하세요!");

    }

    private void SetTutorialMessage(string message)
    {
        if (_battleUI == null)
        {
            return;
        }

        _battleUI.SetObjectiveText(message);
    }
}
