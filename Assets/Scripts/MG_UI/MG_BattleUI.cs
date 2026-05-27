using UnityEngine;
using UnityEngine.UI;

public class MG_BattleUI : MGUIBase
{
    [SerializeField] private Image Image_HpBar;
    [SerializeField] private Text Text_Hp;
    [SerializeField] private Text Text_Objective;
    [SerializeField] private Button Button_Menu;

    private void OnEnable()
    {
        SetObjectiveText("목적지까지 도달하세요!");
        RefreshHp();

        if (Button_Menu != null)
        {
            Button_Menu.onClick.AddListener(OnClickMenu);
        }
    }

    private void OnDisable()
    {
        if (Button_Menu != null)
        {
            Button_Menu.onClick.RemoveListener(OnClickMenu);
        }
    }

    public void RefreshHp()
    {
        MGPlayerModel playerModel = MGGameManager.Inst.PlayerModel;

        float hpRatio = (float)playerModel.CurrentHp / playerModel.MaxHp;
        hpRatio = Mathf.Clamp01(hpRatio);

        if (Image_HpBar != null)
        {
            Image_HpBar.fillAmount = hpRatio;
        }

        if (Text_Hp != null)
        {
            Text_Hp.text = $"HP : {playerModel.CurrentHp} / {playerModel.MaxHp}";
        }
    }

    public void SetObjectiveText(string objectiveText)
    {
        if (Text_Objective == null)
        {
            return;
        }

        Text_Objective.text = objectiveText;
    }

    private void OnClickMenu()
    {
        if (MGGameManager.Inst == null)
        {
            return;
        }

        MGGameManager.Inst.PauseGame();
    }
}
