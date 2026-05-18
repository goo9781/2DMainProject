using System.Collections;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    public GameObject popup;

    public void OpenPopup()
    {
        popup.SetActive(true);

        StartCoroutine(ClosePopupAfterDelay());
    }

    private IEnumerator ClosePopupAfterDelay()
    {
        yield return new WaitForSeconds(3f);

        popup.SetActive(false);
    }
}
