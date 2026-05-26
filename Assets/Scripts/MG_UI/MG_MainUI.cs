using UnityEngine;
using UnityEngine.UI;

public class MG_MainUI : MGUIBase
{
    [Header("버튼")]
    [SerializeField] private Button Button_StartGame;
    [SerializeField] private Button Button_OpenBook;
    [SerializeField] private Button Button_ExitGame;

    private void OnEnable()
    {
        if (Button_StartGame != null)
        {
            Button_StartGame.onClick.AddListener(OnClickStartGame);
        }

        if (Button_OpenBook != null)
        {
            Button_OpenBook.onClick.AddListener(OnClickOpenBook);
        }

        if (Button_ExitGame != null)
        {
            Button_ExitGame.onClick.AddListener(OnClickExitGame);
        }
    }

    private void OnDisable()
    {
        if (Button_StartGame != null)
        {
            Button_StartGame.onClick.RemoveListener(OnClickStartGame);
        }

        if (Button_OpenBook != null)
        {
            Button_OpenBook.onClick.RemoveListener(OnClickOpenBook);
        }

        if (Button_ExitGame != null)
        {
            Button_ExitGame.onClick.RemoveListener(OnClickExitGame);
        }
    }

    private void OnClickStartGame()
    {
        MGGameManager.Inst.StartGame();
    }

    private void OnClickOpenBook()
    {
        //추후에 도감 UI 및 스크립트 추가 후 수정
        Debug.Log("도감 열기");
    }

    private void OnClickExitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }
}
