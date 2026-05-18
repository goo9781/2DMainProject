using System;
using System.Collections.Generic;
using UnityEngine;

public class MGUIManager : MonoBehaviour
{
    [SerializeField] private Canvas Canvas_BgRoot;
    [SerializeField] private Canvas Canvas_MainRoot;
    [SerializeField] private Canvas Canvas_ContentRoot;
    [SerializeField] private Canvas Canvas_PopupRoot;
    [SerializeField] private Canvas Canvas_VeryFrontRoot;

    public static MGUIManager Instance { get; private set; }

    private Dictionary<MGUIType, MGUIBase> _createdUIDic
        = new Dictionary<MGUIType, MGUIBase>();

    private HashSet<MGUIType> _openedUISet
        = new HashSet<MGUIType>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
    }

    public MGUIBase OpenUI(MGUIRootType uiRootType, MGUIType uiType, bool isInitialHide = false)
    {
        MGUIBase openedUI = GetCreatedUI(uiRootType, uiType);

        if(openedUI == null)
        {
            return null;
        }

        bool isSetActiveOnOpen = isInitialHide == false;

        if (_openedUISet.Contains(uiType) == false)
        {
            openedUI.gameObject.SetActive(isSetActiveOnOpen);
            _openedUISet.Add(uiType);
        }

        return openedUI;
    }

    public void CloseUI(MGUIRootType uiRootType, MGUIType uiType)
    {
        if(_openedUISet.Contains(uiType))
        {
            MGUIBase openedUI = _createdUIDic[uiType];
            openedUI.gameObject.SetActive(false);
            _openedUISet.Remove(uiType);
        }
    }

    public MGUIBase OpenPopupUI(MGUIType uiType)
    {
        return OpenUI(MGUIRootType.PopupUI, uiType);
    }

    public void ClosePopupUI(MGUIType uiType)
    {
        CloseUI(MGUIRootType.PopupUI, uiType);
    }

    private MGUIBase GetCreatedUI(MGUIRootType uiRootType, MGUIType uiType)
    {
        if(_createdUIDic.ContainsKey(uiType)==false)
        {
            CreateUI(uiRootType, uiType);
        }

        if(_createdUIDic.ContainsKey(uiType)==false)
        {
            return null;
        }

        return _createdUIDic[uiType];
    }

    private void CreateUI(MGUIRootType uiRootType, MGUIType uiType)
    {
        if(_createdUIDic.ContainsKey(uiType))
        {
            return;
        }

        string path = this.GetUIPath(uiRootType, uiType);

        GameObject loadedObj = Resources.Load<GameObject>(path);

        if(loadedObj == null)
        {
            return;
        }

        Transform root = GetRootTransform(uiRootType);

        if(root == null)
        {
            return;
        }

        GameObject gameObj = Instantiate(loadedObj, root);

        MGUIBase uiBase = gameObj.GetComponent<MGUIBase>();

        if(uiBase == null)
        {
            return;
        }

        _createdUIDic.Add(uiType, uiBase);
    }

    private Transform GetRootTransform(MGUIRootType uiRootType)
    {
        switch(uiRootType)
        {
            case MGUIRootType.BackgroundUI:
                return Canvas_BgRoot.transform;

                case MGUIRootType.MainUI:
                return Canvas_MainRoot.transform;

                case MGUIRootType.ContentUI:
                return Canvas_ContentRoot.transform;

                case MGUIRootType.PopupUI:
                return Canvas_PopupRoot.transform;

                case MGUIRootType.VeryFrontUI:
                return Canvas_VeryFrontRoot.transform;
        }
        return null;
    }

    internal void OpenDialogueUI(string dialogueDataId)
    {

        MGUIBase uiBase = OpenUI(MGUIRootType.PopupUI, MGUIType.MGDialogueUI);

        if(uiBase == null)
        {
            return;
        }


        MGDialogueUI dialogueUI = uiBase as MGDialogueUI;

        if(dialogueUI == null)
        {
            return; 
        }


        dialogueUI.SetDialogue(dialogueDataId);
    }
}
