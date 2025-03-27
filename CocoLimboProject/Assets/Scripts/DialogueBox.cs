using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    //Message stuff
    public string characterNameString;
    public string[] messageLines;
    public string fullMessage = "";
    public string currentMessage = "";
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogue;

    private int charIndex = 0;
    private int sentenceIndex = 0;

    //Timer
    public float timePerChar;
    public float timeBetweenSentence;
    public float speedVariance;

    private float modifiedTimePerChar;
    private float timer = 0f;
    private float sentenceTimer = 0f;
    private bool currentMessageFull = false;

    //Audio
    public AudioSource soundEffect;
    public float pitchShiftLow;
    public float pitchShiftHigh;

    void Start()
    {
        modifiedTimePerChar = timePerChar;
        fullMessage = messageLines[0];

        dialogue.text = "";
        if (characterNameString != null)
        {
            characterName.text = characterNameString;
        } else
        {
            characterName.text = "";
        }
    }

    void Update()
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
                if (sentenceIndex < messageLines.Length-1)
                {
                    sentenceIndex++;
                    fullMessage = messageLines[sentenceIndex];
                    currentMessageFull = false;
                    timer = 0f;
                } 
                else
                {
                    //Done showing all sentences
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
