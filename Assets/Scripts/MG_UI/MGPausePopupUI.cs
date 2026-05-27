using UnityEngine;
using UnityEngine.UI;

public class MGPausePopupUI : MGUIBase
{
    [Header("버튼")]
    [SerializeField] private Button Button_Resume;
    [SerializeField] private Button Button_GoMain;
    [SerializeField] private Button Button_ExitGame;

    private void OnEnable()
    {
        if (Button_Resume != null)
        {
            Button_Resume.onClick.AddListener(OnClickResume);
        }

        if (Button_GoMain != null)
        {
            Button_GoMain.onClick.AddListener(OnClickGoMain);
        }

        if (Button_ExitGame != null)
        {
            Button_ExitGame.onClick.AddListener(OnClickExitGame);
        }
    }

    private void OnDisable()
    {
        if (Button_Resume != null)
        {
            Button_Resume.onClick.RemoveListener(OnClickResume);
        }

        if (Button_GoMain != null)
        {
            Button_GoMain.onClick.RemoveListener(OnClickGoMain);
        }

        if (Button_ExitGame != null)
        {
            Button_ExitGame.onClick.RemoveListener(OnClickExitGame);
        }
    }

    private void OnClickResume()
    {
        if (MGGameManager.Inst == null)
        {
            return;
        }

        MGGameManager.Inst.ResumeGame();
    }

    private void OnClickGoMain()
    {
        if (MGGameManager.Inst == null)
        {
            return;
        }

        MGGameManager.Inst.GoMain();
    }

    private void OnClickExitGame()
    {
        if (MGGameManager.Inst == null)
        {
            return;
        }

        MGGameManager.Inst.ExitGame();
    }
}
