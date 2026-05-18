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
    MGSimplePopup,
    MGLoadingUI,
    MGDialogueUI,
    MGGameResultUI,
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
}