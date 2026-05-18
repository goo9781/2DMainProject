using TMPro;
using UnityEngine;

public class MGDialogueUI : MGUIBase
{
    [SerializeField] private TextMeshProUGUI _descriptionText;

    public void SetDialogue(string dialogueDataId)
    {

        if (_descriptionText != null)
        {
            _descriptionText.text = dialogueDataId;
        }
    }
}
