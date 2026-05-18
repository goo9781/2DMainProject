using UnityEngine;
using UnityEngine.UI;

public class MG_BattleUI : MGUIBase
{
    [SerializeField] private Image Image_HpBar;
    [SerializeField] private Text Text_Hp;

    private void Start()
    {
        RefreshHp();
    }

    public void RefreshHp()
    {
        MGPlayerModel playerModel = MGGameManager.Inst.PlayerModel;

        float hpRatio = (float)playerModel.CurrentHp / playerModel.MaxHp;

        if (Image_HpBar != null)
        {
            Image_HpBar.fillAmount = hpRatio;
        }

        if (Text_Hp != null)
        {
            Text_Hp.text = $"HP : {playerModel.CurrentHp} / {playerModel.MaxHp}";
        }
    }
}
