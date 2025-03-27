using UnityEngine;
using System;

public class CSVReader : MonoBehaviour
{
    public TextAsset textAssetData;

    public string[] sentences;

    public string conversationID;


    public string[] getSentences()
    {
        string[] newSentences;
        int sentenceCount = 0;
        readCSV();
        for (int i = 0; i < sentences.Length; i++)
        {
            if (sentences[i] == conversationID)
            {
                for (int j = i+1; sentences[j] != "*"; j++)
                {
                    sentenceCount++;
                }
                newSentences = new string[sentenceCount];
                for (int k = 0; k < sentenceCount; k++)
                {
                    newSentences[k] = sentences[i + k + 1];
                }
                return newSentences;
            }
        }
        
        return null;
    }

    private void readCSV()
    {
        sentences = textAssetData.text.Split(new string[] { ",", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        /*for (int i = 0; i < data.Length; i++)
        {
            Debug.Log(data[i]);
        }*/
    }

    
}
