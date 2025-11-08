using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Star : MonoBehaviour
{
    [Header("별을 수집했을 때의 효과 (선택사항)")]
    public AudioClip collectSound;
    public ParticleSystem collectEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            // 씬 내 StarCollectCondition을 찾아 별 수집 보고
            StarCollectCondition condition = FindObjectOfType<StarCollectCondition>();
            if (condition != null)
                condition.RegisterStarCollected();

            // 효과 재생
            if (collectEffect != null)
                Instantiate(collectEffect, transform.position, Quaternion.identity);
            if (collectSound != null)
                AudioSource.PlayClipAtPoint(collectSound, transform.position);

            // 별 제거
            Destroy(gameObject);
        }
    }
}
