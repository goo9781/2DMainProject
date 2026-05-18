using TMPro;
using UnityEngine;

public class MG_SimplePopup : MGUIBase
{
    [SerializeField] private TMP_Text text_Message;

    public void SetUI(string msg)
    {
        text_Message.text = msg;
    }

    public void OnClickClose()
    {
        MGUIManager.Instance.CloseSimplePopup();
    }
}
