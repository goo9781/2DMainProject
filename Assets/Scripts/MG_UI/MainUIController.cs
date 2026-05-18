using UnityEngine;

public class MainUIController : MonoBehaviour
{
    public GameObject playerPopup;
    public GameObject inventoryPopup;
    
    public void OnClickCharacter()
    {
        playerPopup.SetActive(true);
    }

    public void OnClickSkill()
    {
        Debug.Log("스킬 버튼 클릭");
    }

    public void OnClickInventory()
    {
        inventoryPopup.SetActive(true);
    }

    public void OnClickMonsterSpawn()
    {
        Debug.Log("몬스터 소환 버튼 클릭");
    }

    public void OnClickOpenSimplePopup()
    {
        MGUIManager.Instance.OpenSimplePopup("테스트 팝업");
    }
}
