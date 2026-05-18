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
        StartGame();

        //메인 UI 추가후 변경
        //ChangeState(MGGameState.Main);
    }
    
    public void ChangeState(MGGameState state)
    {
        _prevState = _currentState;
        _currentState = state;
    }

    public void StartGame()
    {
        ChangeState(MGGameState.Playing);

        Time.timeScale = 1f;
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

        MGUIManager.Instance.OpenGameResultUI(true);
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

        MGUIManager.Instance.OpenGameResultUI(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        ChangeState(MGGameState.Playing);

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void GoMain()
    {
        Time.timeScale = 1f;

        ChangeState(MGGameState.Main);

        SceneManager.LoadScene("MainScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
