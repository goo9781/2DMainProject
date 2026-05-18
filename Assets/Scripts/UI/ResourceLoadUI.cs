using UnityEngine;
using UnityEngine.UI;

public class ResourceLoadUI : MonoBehaviour
{
    [SerializeField] private Image Image_LoadSampleTest;

    private void OnEnable()
    {
        Sprite loadedSprite = Resources.Load<Sprite>("Icon/SkillIcon_Block");

        if(loadedSprite != null)
        {
            Image_LoadSampleTest.sprite = loadedSprite;
            Debug.Log("스프라이트 로드 성공");
        }

        else
        {
            Debug.LogError("스프라이트 로드 실패");
        }
    }
}
