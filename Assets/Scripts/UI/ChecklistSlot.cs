using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChecklistSlot : MonoBehaviour
{
    [SerializeField] private Text command;
    [SerializeField] private Text commandType;
    [SerializeField] private Text description;
    [SerializeField] private Button button;

    public ClipboardData ClipboardData { get; set; }
    public Action DeleteSlot;

    public void Initialize()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => { StartCoroutine(OnButtonClick()); });
    }

    private IEnumerator OnButtonClick()
    {
        button.enabled = false;
        var classroomTransform = GameManager.Instance.ClassroomTransform;

        switch (ClipboardData.actionType)
        {
            case ClipboardActionType.Object:
                var objectPos = ClipboardData.objectData.transformData.position;
                var objectRot = ClipboardData.objectData.transformData.rotation;
                var objectScale = ClipboardData.objectData.transformData.scale;
                yield return GameManager.Instance.LoadAssetBundle(ClipboardData.objectData.url, classroomTransform,
                    objectPos, objectRot, objectScale);
                break;
            case ClipboardActionType.Presentation:
                var presentationPos = ClipboardData.presentationData.transformData.position;
                var presentationRot = ClipboardData.presentationData.transformData.rotation;
                var presentationScale = ClipboardData.presentationData.transformData.scale;
                yield return GameManager.Instance.LoadAssetBundle(ClipboardData.presentationData.url,
                    classroomTransform, presentationPos, presentationRot, presentationScale);
                var videoPlayer = FindFirstObjectByType<VideoPlayerController>();
                videoPlayer.VideoPlayer.url = ClipboardData.presentationData.videoUrl;
                videoPlayer.VideoPlayer.Prepare();
                videoPlayer.VideoPlayer.prepareCompleted += videoPlayer.PlayVideo;
                break;
            case ClipboardActionType.Teleport:
                var newPosition = ClipboardData.teleportData.transformData.position;
                FindFirstObjectByType<PlayerMovement>().Teleport(newPosition);
                break;
            case ClipboardActionType.Poll:
                var pollPos = ClipboardData.pollData.transformData.position;
                var pollRot = ClipboardData.pollData.transformData.rotation;
                var pollScale = ClipboardData.pollData.transformData.scale;
                yield return GameManager.Instance.LoadAssetBundle(ClipboardData.pollData.url,
                    classroomTransform, pollPos, pollRot, pollScale);
                var poll = FindFirstObjectByType<Poll>();
                poll.PollCommand = ClipboardData.pollData;
                poll.UpdateUI();
                break;
            default:
                Debug.Log($"COMMAND: {ClipboardData.commandData.name} - {ClipboardData.commandData.description}");
                break;
        }

        DeleteSlot?.Invoke();
        yield return null;
    }

    public void UpdateUI()
    {
        switch (ClipboardData.actionType)
        {
            case ClipboardActionType.Object:
                command.text = ClipboardData.objectData.name;
                description.text = ClipboardData.objectData.description;
                break;
            case ClipboardActionType.Presentation:
                command.text = ClipboardData.presentationData.name;
                description.text = ClipboardData.presentationData.description;
                break;
            case ClipboardActionType.Teleport:
                command.text = ClipboardData.teleportData.name;
                description.text = ClipboardData.teleportData.description;
                break;
            case ClipboardActionType.Poll:
                command.text = ClipboardData.pollData.name;
                description.text = ClipboardData.pollData.description;
                break;
            default:
                command.text = ClipboardData.commandData.name;
                description.text = ClipboardData.commandData.description;
                break;
        }

        commandType.text = ClipboardData.actionType.ToString();
    }
}