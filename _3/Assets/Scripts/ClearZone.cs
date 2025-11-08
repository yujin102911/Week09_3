using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ClearZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            if (GameManager.Instance != null)
                GameManager.Instance.TryClearStage();
            else
                Debug.LogWarning("❗ GameManager Instance가 없습니다!");
        }
    }
}
