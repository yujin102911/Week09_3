using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Game/Stage Data")]
public class StageData : ScriptableObject
{
    [Header("Stage Number")]
    public int stageNumber;

    [Header("ClearCondition Object")]
    public MonoBehaviour clearConditionComponent;

    [Header("Additional")]
    public float timeLimit = 0f;
    //public AudioClip bgmClip;
}
