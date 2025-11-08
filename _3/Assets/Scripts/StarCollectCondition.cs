using UnityEngine;

public class StarCollectCondition : MonoBehaviour, IClearCondition
{
    [Header("총 별 개수 (씬에 존재하는 Star 오브젝트 개수와 동일해야 함)")]
    public int totalStars = 0;
    private int collectedStars = 0;

    private void Start()
    {
        // 자동으로 Star 오브젝트 개수 감지
        Star[] stars = FindObjectsOfType<Star>();
        totalStars = stars.Length;
    }

    public void RegisterStarCollected()
    {
        collectedStars++;
        Debug.Log($"🌟 별 {collectedStars}/{totalStars} 수집됨");

        if (collectedStars >= totalStars)
        {
            Debug.Log("⭐ 모든 별을 수집했습니다!");
        }
    }

    public bool IsCleared()
    {
        return collectedStars >= totalStars && totalStars > 0;
    }

    public string GetConditionName()
    {
        return $"별 {collectedStars}/{totalStars} 수집";
    }
}
