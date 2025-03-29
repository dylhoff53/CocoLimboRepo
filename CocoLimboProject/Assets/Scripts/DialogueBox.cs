using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    [Header("Message Text Stuff")]
    public string characterNameString;
    public string[] messageLines;
    public string fullMessage = "";
    public string currentMessage = "";
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogue;

    public CSVReader reader;

    private int charIndex = 0;
    private int sentenceIndex = 0;

    [Header("Timer Stuff")]
    public float timePerChar;
    public float timeBetweenSentence;
    public float speedVariance;

    private float modifiedTimePerChar;
    private float timer = 0f;
    private float sentenceTimer = 0f;
    private bool currentMessageFull = false;
    private bool running = false;

    [Header("Audio Stuff")]
    public AudioSource soundEffect;
    public float pitchShiftLow;
    public float pitchShiftHigh;

    public void BeginDisplay()
    {
        messageLines = reader.getSentences();
        modifiedTimePerChar = timePerChar;

        fullMessage = messageLines[0];
        currentMessageFull = false;
        running = true;
        dialogue.text = "";
        if (characterNameString != null)
        {
            characterName.text = characterNameString;
        } 
        else
        {
            characterName.text = "";
        }
    }

    private void EndDisplay()
    {
        running = false;
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        if (running == true)
        {
            if (currentMessageFull == false)
            {
                timer += Time.deltaTime;

                if (timer > modifiedTimePerChar)
                {
                    UpdateCurrentMessage();
                    timer = 0f;
                }
            }
            else
            {

                sentenceTimer += Time.deltaTime;
                if (sentenceTimer > timeBetweenSentence)
                {
                    charIndex = 0;
                    if (sentenceIndex < messageLines.Length - 1)
                    {
                        sentenceIndex++;
                        fullMessage = messageLines[sentenceIndex];
                        currentMessageFull = false;
                        timer = 0f;
                    }
                    else
                    {
                        EndDisplay();
                    }
                }
            }
        }
    }

    private void UpdateCurrentMessage()
    {
        charIndex++;
        modifiedTimePerChar = timePerChar * Random.Range(1 - speedVariance, 1 + speedVariance);
        currentMessage = fullMessage.Substring(0,charIndex);
        dialogue.text = currentMessage;
        PitchAndPlaySoundEffect();
        if (currentMessage.Length == fullMessage.Length)
        {
            currentMessageFull = true;
            sentenceTimer = 0f;
        }
    }

    private void PitchAndPlaySoundEffect()
    {
        soundEffect.pitch = Random.Range(pitchShiftLow, pitchShiftHigh);
        soundEffect.Play();
    }

}
