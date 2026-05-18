using UnityEngine;

public class InventoryPopupUI : MonoBehaviour
{
    [Header("참조")]
    public GameObject slotPrefab;
    public Transform content;

    [Header("설정")]
    public int createCount = 1;

    public void AddSlot()
    {
        for (int i = 0; i < createCount; i++)
        {
            Instantiate(slotPrefab, content);
        }

        Debug.Log("슬롯 생성!");

    }
    public void ClosePopup()
    {
        gameObject.SetActive(false);
    }
}
