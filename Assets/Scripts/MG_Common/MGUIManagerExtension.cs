using UnityEngine;

public enum MGUIRootType
    {
        None = 0,
        BackgroundUI,
        MainUI,
        ContentUI,
        PopupUI,
        VeryFrontUI
    }

public enum MGUIType
{
    MGMainUI,
    MGSimplePopup,
    MGLoadingUI,
    MGDialogueUI,
    MGGameResultUI,
    MGBattleUI,
}

public static class MGUIManagerExtension
{
    public static string GetUIPath(this MGUIManager uiManager, MGUIRootType uiRootType, MGUIType uiType)
    {
        return $"Prefabs/UI/{uiRootType}/{uiType}";
    }

    public static void OpenSimplePopup(this MGUIManager uiManager, string msg)
    {
        MGUIBase uiBase = uiManager.OpenPopupUI(MGUIType.MGSimplePopup);

        if(uiBase == null)
        {
            return;
        }
        
        if (uiBase is MG_SimplePopup simplePopup)
        {
            simplePopup.SetUI(msg);
        }
    }

    public static void CloseSimplePopup(this MGUIManager uiManager)
    {
        uiManager.ClosePopupUI(MGUIType.MGSimplePopup);
    }

    public static void OpenLoadingUI(this MGUIManager uiManager)
    {
        MGUIBase uiBase = uiManager.OpenUI(MGUIRootType.VeryFrontUI, MGUIType.MGLoadingUI);


        if (uiBase == null)
        {
            Debug.LogWarning("로딩 UI가 생성되지 않았습니다.");
            return;
        }

    }

    public static void CloseLoadingUI(this MGUIManager uiManager)
    {
        uiManager.CloseUI(MGUIRootType.VeryFrontUI, MGUIType.MGLoadingUI);
    }

    public static void OpenGameResultUI(this MGUIManager uiManager, bool isSuccess)
    {
        MGUIBase uiBase = uiManager.OpenPopupUI(MGUIType.MGGameResultUI);

        if(uiBase == null)
        {
            return;
        }

        if (uiBase is MGGameResultUI gameResultUI)
        {
            gameResultUI.SetUI(isSuccess);
        }
    }

    public static void OpenMainUI(this MGUIManager uiManager)
    {
        uiManager.OpenUI(MGUIRootType.MainUI, MGUIType.MGMainUI);
    }

    public static void CloseMainUI(this MGUIManager uiManager)
    {
        uiManager.CloseUI(MGUIRootType.MainUI, MGUIType.MGMainUI);
    }

    public static void OpenBattleUI(this MGUIManager uiManager)
    {
        uiManager.OpenUI(MGUIRootType.MainUI, MGUIType.MGBattleUI);
    }

    public static void CloseBattleUI(this MGUIManager uiManager)
    {
        uiManager.CloseUI(MGUIRootType.MainUI, MGUIType.MGBattleUI);
    }

}