using UnityEngine;
using System.Collections.Generic;

public class InputRecorder : MonoBehaviour
{
    public PlayerController player;
    public bool isRecording = false;
    public bool isReplaying = false;

    private float startTime;
    private List<InputFrame> recordedFrames = new List<InputFrame>();
    private int replayIndex = 0;

    private void Update()
    {
        if (isRecording) RecordInputs();
        else if (isReplaying) ReplayInputs();
    }

    public void StartRecording()
    {
        recordedFrames.Clear();
        isRecording = true;
        isReplaying = false;
        startTime = Time.time;
        Debug.Log("녹화 시작");
    }
    public void StopRecording()
    {
        isRecording = false;
        Debug.Log($"녹화 끝. {recordedFrames.Count}프레임 녹화됨");
    }

    public void StartReplay()
    {
        replayIndex = 0;
        isReplaying = true;
        isRecording = false;
        startTime = Time.time;
        Debug.Log("재생 시작");
    }
    public void StopReplay()
    {
        isReplaying = false;
        isRecording = false;
        replayIndex = 0;
        recordedFrames.Clear(); // 완전 초기화
        if (player != null)
            player.Halt();
        Debug.Log("⏹️ Replay stopped and reset.");
    }

    private void RecordInputs()
    {
        InputFrame frame = new InputFrame();
        frame.time = Time.time - startTime;
        frame.left = Input.GetKey(KeyCode.A);
        frame.right = Input.GetKey(KeyCode.D);
        frame.jump = Input.GetKey(KeyCode.Space);
        recordedFrames.Add(frame);
    }
    private void ReplayInputs()
    {
        if (replayIndex >= recordedFrames.Count)
        {
            isReplaying = false;
            Debug.Log("재생 끝");
            recordedFrames.Clear(); // 완전 초기화
            return;
        }
        float elapsed = Time.time - startTime;
        InputFrame current = recordedFrames[replayIndex];

        if(elapsed >= current.time)
        {
            player.SetInput(current.left, current.right, current.jump);
            replayIndex++;
        }
    }
    public bool HasRecordedData()
    {
        return recordedFrames != null && recordedFrames.Count > 0;
    }


}
