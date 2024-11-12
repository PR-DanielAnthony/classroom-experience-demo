using UnityEngine;

public class Poll : MonoBehaviour
{
    [SerializeField] private TextMesh message;
    [SerializeField] private TextMesh creator;
    [SerializeField] private TextMesh options;

    public PollCommand PollCommand { get; set; }

    public void UpdateUI()
    {
        message.text = PollCommand.message;
        creator.text = PollCommand.creator;

        foreach (var option in PollCommand.options)
            options.text += $"{option.id}) {option.text}\n";
    }
}