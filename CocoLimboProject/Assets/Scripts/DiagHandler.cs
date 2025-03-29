using UnityEngine;

public class DiagHandler : MonoBehaviour
{
    public DialogueBox dialogueBox;
    public CSVReader reader;
    public void BeginDisplay()
    {
        reader.conversationID = "3";
        dialogueBox.gameObject.SetActive(true);
        dialogueBox.BeginDisplay();
    }
}
