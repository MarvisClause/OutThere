using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour 

{   private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;

    public static HighscoreTable _instance;
    private void Awake()
    {
        _instance = this;
        //highscoreEntryList = new List<HighscoreEntry>();
        
        entryContainer = transform.Find("Score_Entry_Container");
        entryTemplate = entryContainer.Find("HighScore_Entry_Template");

        entryTemplate.gameObject.SetActive(false);

        AddHighscoreEntry(1555, "Test");

        string jsonString = PlayerPrefs.GetString("Score Table");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        //Sort entry list by score
        for (int i = 0; i <highscores.highscoreEntryList.Count; i++)
        {
            for (int j = i+1; j < highscores.highscoreEntryList.Count ; j++)
            {
                if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)
                {
                    //Swap
                    HighscoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }
        highscoreEntryTransformList = new List<Transform>();
        foreach(HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry,entryContainer,highscoreEntryTransformList);
        }
    }   
    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry,Transform container,List<Transform> transformList)
    {    
        float templateHeight = 50f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH"; break;

            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;

        }
        entryTransform.Find("Pos_Text").GetComponent<Text>().text = rankString;

        int score = highscoreEntry.score;

        entryTransform.Find("Score_Text").GetComponent<Text>().text = score.ToString();

        string name = highscoreEntry.name;

        entryTransform.Find("Name_Text").GetComponent<Text>().text = name;

        transformList.Add(entryTransform);
    }  

    public void AddHighscoreEntry(int score,string name)
    { 
        //Create HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name }; 

        //Load save Highscores
        string jsonString = PlayerPrefs.GetString("Score Table");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
         
        //Add new entry to Highscores
        highscores.highscoreEntryList.Add(highscoreEntry);
         
        //Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("Score Table", json);
        PlayerPrefs.Save();

    }
    
    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList; 
    }
    
    //Represent a single High score entry 
    [System.Serializable]
    private class HighscoreEntry {
        public int score;
        public string name;
    }
}
