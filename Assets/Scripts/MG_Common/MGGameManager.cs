using UnityEngine;
using UnityEngine.SceneManagement;

public class MGGameManager : MonoBehaviour
{
    public static MGGameManager Inst {get; private set;}

    private MGPlayerModel _playerModel = new MGPlayerModel();

    private bool _isPlaying;

    public MGPlayerModel PlayerModel
    {
        get { return _playerModel; }
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
    }
    
    public void StartGame()
    {
        _isPlaying = true;

        Debug.Log("게임시작");
    }

    public void SuccessGame()
    {
        if (_isPlaying == false)
        {
            return;
        }

        _isPlaying = false;

        _playerModel.ClearStageCount++;

        MGUIManager.Instance.OpenGameResultUI(true);
    }

    public void FailGame()
    {
        if (_isPlaying == false)
        {
            return;
        }

        _isPlaying = false;

        _playerModel.DeathCount++;

        MGUIManager.Instance.OpenGameResultUI(false);
    }

    public void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

        StartGame();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
