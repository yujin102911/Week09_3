using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("참조")]
    public InputRecorder inputRecorder;
    public GameManager gameManager;

    [Header("UI 텍스트")]
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI stageText;

    [Header("버튼들")]
    public Button startRecordButton;
    public Button playButton;
    public Button stopButton;

    [Header("엔딩 패널")]
    public GameObject endingPanel;

    [Header("색상 설정")]
    public Color idleColor = Color.white;
    public Color recordingColor = Color.red;
    public Color replayingColor = Color.green;

    private void Start()
    {
        if (inputRecorder == null)
            inputRecorder = FindObjectOfType<InputRecorder>();
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();

        if (endingPanel != null)
            endingPanel.SetActive(false);
    }

    private void Update()
    {
        UpdateRecorderStatus();
        UpdateStageInfo();
        UpdateButtons();
    }

    private void UpdateRecorderStatus()
    {
        if (inputRecorder == null || statusText == null)
            return;

        if (inputRecorder.isRecording)
        {
            statusText.text = "● REC";
            statusText.color = recordingColor;
        }
        else if (inputRecorder.isReplaying)
        {
            statusText.text = "▶ REPLAY";
            statusText.color = replayingColor;
        }
        else
        {
            statusText.text = "⏸ IDLE";
            statusText.color = idleColor;
        }
    }

    private void UpdateStageInfo()
    {
        if (gameManager == null || stageText == null)
            return;

        int current = gameManager.currentStage + 1;
        int total = gameManager.respawnPoints.Length;
        stageText.text = $"STAGE {current} / {total}";
    }

    private void UpdateButtons()
    {
        if (inputRecorder == null)
            return;

        // 버튼이 전부 연결되어 있지 않은 경우 예외 방지
        if (!startRecordButton || !playButton || !stopButton)
            return;

        // 상태별 제어
        if (inputRecorder.isRecording)
        {
            // 🎙️ 녹화 중: 녹화만 중단 가능
            startRecordButton.interactable = false;
            playButton.interactable = false;
            stopButton.interactable = true;
        }
        else if (inputRecorder.isReplaying)
        {
            // ▶ 재생 중: 모든 버튼 잠금
            startRecordButton.interactable = false;
            playButton.interactable = false;
            stopButton.interactable = false;
        }
        else
        {
            // ⏸ 대기 중
            bool hasRecordedData = inputRecorder.HasRecordedData();

            startRecordButton.interactable = true;   // 언제나 새 녹화 가능
            playButton.interactable = hasRecordedData; // 녹화 데이터 있을 때만 재생 가능
            stopButton.interactable = false;
        }
    }

    public void ShowEndingPanel()
    {
        if (endingPanel == null) return;
        endingPanel.SetActive(true);
        Debug.Log("🎬 엔딩 패널 표시 완료!");
    }
}
