using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("각 스테이지별 리스폰 포인트 (씬에 존재)")]
    public Transform[] respawnPoints;

    [Header("각 스테이지별 클리어 조건 (IClearCondition을 구현한 컴포넌트)")]
    public MonoBehaviour[] clearConditions; // IClearCondition 구현체만 넣기

    [Header("현재 스테이지 번호")]
    public int currentStage = 0;

    private PlayerController player;
    private InputRecorder inputRecorder;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        player = FindObjectOfType<PlayerController>();
        inputRecorder = FindObjectOfType<InputRecorder>();
    }

    private IClearCondition CurrentCondition =>
        (clearConditions != null && clearConditions.Length > currentStage)
        ? clearConditions[currentStage] as IClearCondition
        : null;

    private Transform CurrentSpawn =>
        (respawnPoints != null && respawnPoints.Length > currentStage)
        ? respawnPoints[currentStage]
        : null;

    // ✅ 리스폰
    public void RespawnPlayer()
    {
        inputRecorder = FindObjectOfType<InputRecorder>();
        if (inputRecorder != null)
            inputRecorder.StopReplay();

        if (player == null)
            player = FindObjectOfType<PlayerController>();
        player.Halt();

        Transform spawn = CurrentSpawn;
        if (spawn == null)
        {
            Debug.LogWarning("SpawnPoint가 설정되지 않았습니다!");
            return;
        }

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        player.transform.position = spawn.position;

        Debug.Log($"🔁 Player Respawn at Stage {currentStage}");
    }

    // ✅ 스테이지 클리어 시도
    public void TryClearStage()
    {
        inputRecorder = FindObjectOfType<InputRecorder>();
        if (inputRecorder != null)
            inputRecorder.StopReplay();

        IClearCondition condition = CurrentCondition;

        if (condition == null)
        {
            Debug.Log($"✅ 스테이지 {currentStage}는 조건 없음 → 자동 클리어");
            NextStage();
            return;
        }

        if (condition.IsCleared())
        {
            Debug.Log($"🎉 스테이지 클리어: {currentStage}");
            NextStage();
        }
        else
        {
            Debug.Log($"❌ 클리어 조건({condition.GetConditionName()}) 미달성");
        }
    }

    // ✅ 다음 스테이지
    public void NextStage()
    {
        inputRecorder = FindObjectOfType<InputRecorder>();
        if (inputRecorder != null)
            inputRecorder.StopReplay();

        currentStage++;

        // 🔥 모든 스테이지 클리어 시점 감지
        if (currentStage >= respawnPoints.Length)
        {
            Debug.Log("🏁 모든 스테이지 클리어! 엔딩 호출!");

            // 엔딩 패널 표시
            UIManager ui = FindObjectOfType<UIManager>();
            if (ui != null)
                ui.ShowEndingPanel();

            return;
        }

        Debug.Log($"➡️ 다음 스테이지 진입: {currentStage}");
        RespawnPlayer();
    }
}
