using UnityEngine;
using UnityEngine.UI;

public class MGGameResultUI : MGUIBase
{
    [SerializeField] private Text Text_Result;
    [SerializeField] private Button Button_Confirm;
    [SerializeField] private Button Button_Exit;

    private void OnEnable()
    {
        if (Button_Confirm != null)
        {
            Button_Confirm.onClick.AddListener(OnClickConfirm);
        }

        if (Button_Exit != null)
        {
            Button_Exit.onClick.AddListener(OnClickExit);
        }
    }

    private void OnDisable()
    {
        if (Button_Confirm != null)
        {
            Button_Confirm.onClick.RemoveListener(OnClickConfirm);
        }

        if (Button_Exit != null)
        {
            Button_Exit.onClick.RemoveListener(OnClickExit);
        }
    }

    public void SetUI(bool isSuccess)
    {
        if (Text_Result != null)
        {
            Text_Result.text = isSuccess ? "게임 성공!" : "게임 실패!";
        }

        Time.timeScale = 0f;
    }

    private void OnClickConfirm()
    {
        if (MGGameManager.Inst == null)
        {
            return;
        }

        MGGameManager.Inst.RestartGame();
    }

    private void OnClickExit()
    {
        if (MGGameManager.Inst == null)
        {
            return;
        }
        
        MGGameManager.Inst.GoMain();
    }
}
