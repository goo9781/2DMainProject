using UnityEngine;
using UnityEngine.SceneManagement;

public enum MGGameState
{
    None = 0,
    Main,
    Playing,
    Pause,
    StageClear,
    GameOver
}

public class MGGameManager : MonoBehaviour
{
    public static MGGameManager Inst {get; private set;}

    private MGPlayerModel _playerModel = new MGPlayerModel();

    private MGGameState _currentState = MGGameState.None;
    private MGGameState _prevState = MGGameState.None;

    public MGPlayerModel PlayerModel
    {
        get { return _playerModel; }
    }

    public MGGameState CurrentState
    {
        get { return _currentState; }
    }

    private void Awake()
    {
        if (Inst != null)
        {
            Destroy(gameObject);
            return;
        }

        Inst = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartMain();
    }
    
    public void ChangeState(MGGameState state)
    {
        _prevState = _currentState;
        _currentState = state;
    }

    public void StartGame()
    {
        Time.timeScale = 1f;

        ChangeState(MGGameState.Playing);

        if (MGUIManager.Instance != null)
        {
            MGUIManager.Instance.CloseMainUI();
            MGUIManager.Instance.OpenBattleUI();
        }
    }

    public void StartMain()
    {
        Time.timeScale = 1f;

        ChangeState(MGGameState.Main);

        if (MGUIManager.Instance != null)
        {
            MGUIManager.Instance.CloseBattleUI();
            MGUIManager.Instance.OpenMainUI();
        }

        ResetPlayerForRestart();
    }

    public void PauseGame()
    {
        if (_currentState != MGGameState.Playing)
        {
            return;
        }

        ChangeState(MGGameState.Pause);

        Time.timeScale = 0f;

        //일시정지 팝업 만들면 연결
        //MGUIManager.Instance.OpenPausePopup();
    }

    public void ResumeGame()
    {
        if (_currentState != MGGameState.Pause)
        {
            return;
        }

        ChangeState(MGGameState.Playing);

        Time.timeScale = 1f;

        //일시정지 팝업 닫기에 연결
        //MGUIManager.Instance.ClosePausePopup();
    }

    public void StageClearGame()
    {
        if (_currentState != MGGameState.Playing)
        {
            return;
        }

        ChangeState(MGGameState.StageClear);

        _playerModel.ClearStageCount++;

        Time.timeScale = 0f;

        if (MGUIManager.Instance != null)
        {
            MGUIManager.Instance.OpenGameResultUI(true);
        }
    }

    public void GameOver()
    {
        if (_currentState != MGGameState.Playing)
        {
            return;
        }

        ChangeState(MGGameState.GameOver);

        _playerModel.DeathCount++;

        Time.timeScale = 0f;

        if (MGUIManager.Instance != null)
        {
            MGUIManager.Instance.OpenGameResultUI(false);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        ChangeState(MGGameState.Playing);

        ResetPlayerForRestart();

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void ResetPlayerForRestart()
    {
        if (_playerModel == null)
        {
            return;
        }

        _playerModel.CurrentHp = _playerModel.MaxHp;
    }

    public void GoMain()
    {
        Time.timeScale = 1f;

        ChangeState(MGGameState.Main);

        if (MGUIManager.Instance != null)
        {
            MGUIManager.Instance.CloseBattleUI();
            MGUIManager.Instance.ClosePopupUI(MGUIType.MGGameResultUI);
            MGUIManager.Instance.OpenMainUI();
        }

        ResetPlayerForRestart();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
