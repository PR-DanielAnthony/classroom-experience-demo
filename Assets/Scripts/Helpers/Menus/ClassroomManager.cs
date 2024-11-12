using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassroomManager : Menu
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotContainer;
    [SerializeField] private GameObject clipboard;
    [SerializeField] private Button clipboardButton;
    [SerializeField] private Text playerName;

    private readonly List<GameObject> commands = new();
    private GameObject player;

    public override void Initialize()
    {
        clipboardButton.onClick.RemoveAllListeners();
        clipboardButton.onClick.AddListener(() => clipboard.SetActive(!clipboard.activeSelf));
    }

    public override void Show()
    {
        base.Show();

        if (!player)
        {
            player = Instantiate(playerPrefab);

            if (GameManager.Instance.User != null)
                playerName.text = $"WELCOME {GameManager.Instance.User.name}";

            else
                playerName.text = "WELCOME GUEST";
        }

        ClearClipboardData();
        LoadClipboardData();
    }

    private void ClearClipboardData()
    {
        commands.Clear();

        foreach (Transform command in slotContainer)
            Destroy(command.gameObject);
    }

    private void LoadClipboardData()
    {
        foreach (var data in GameManager.Instance.Clipboard.data)
            CreateClipboardSlot(data);
    }

    public void CreateClipboardSlot(ClipboardData data)
    {
        GameObject option = Instantiate(slotPrefab, slotContainer);
        ChecklistSlot checklistSlot = option.GetComponent<ChecklistSlot>();
        checklistSlot.Initialize();
        commands.Add(option);

        if (data != null)
        {
            checklistSlot.ClipboardData = data;
            checklistSlot.UpdateUI();
            checklistSlot.DeleteSlot += () =>
            {
                commands.Remove(option);
                option.SetActive(false);
            };
        }
    }
}