using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour 
{   private Transform _entryContainer;
    private Transform _entryTemplate;
    private List<Transform> _highscoreEntryTransformList;
    [SerializeField]private int _maxQuantOfCompetetors; 

    public static HighscoreTable _instance;
    private void Awake()
    {
        // For manual clean list of res
        //PlayerPrefs.DeleteAll(); 

        _instance = this;
        _highscoreEntryTransformList = new List<Transform>();
        _entryContainer = transform.Find("Score_Entry_Container");
        _entryTemplate = _entryContainer.Find("HighScore_Entry_Template");

        _entryTemplate.gameObject.SetActive(false);

        string jsonString = PlayerPrefs.GetString("Score Table");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString); 

         

        if (highscores == null)
        {// Reload
            jsonString = PlayerPrefs.GetString("Score Table");
            highscores = JsonUtility.FromJson<Highscores>(jsonString);
        }
        UpdateHighscoreTable();
    }   
    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry,Transform container,List<Transform> transformList)
    {    //Height
        float templateHeight = 50f;
        Transform entryTransform = Instantiate(_entryTemplate, container);
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

        string time = highscoreEntry.time.ConvertToText();

        entryTransform.Find("Time_Text").GetComponent<Text>().text = time;

        string name = highscoreEntry.name;

        entryTransform.Find("Name_Text").GetComponent<Text>().text = name;

        transformList.Add(entryTransform); 
    }  

    public void AddHighscoreEntry(int score,TimeRes time,string name)
    { 
        //Create HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score,time = time ,name = name }; 

        //Load save Highscores
        string jsonString = PlayerPrefs.GetString("Score Table");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null)
        {
            // There's no stored table, initialize
            highscores = new Highscores()
            {
                highscoreEntryList = new List<HighscoreEntry>()
            };
        }

        //Add new entry to Highscores
        highscores.highscoreEntryList.Add(highscoreEntry);
         
        //Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("Score Table", json);
        PlayerPrefs.Save();

    } 
    public void DelHighscoreEntry()
    {
        
    }

    public void UpdateHighscoreTable()
    { 
        
        for (int i = 0; i < _highscoreEntryTransformList.Count; i++)
        {
            Destroy(_highscoreEntryTransformList[i].gameObject);
        }
        _highscoreEntryTransformList.Clear();
         
        // Load data form highscore list
        string jsonString = PlayerPrefs.GetString("Score Table");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        // Sorting by coefficient ( coefficient = score / time )
        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
            {
                if (highscores.highscoreEntryList[j].score / highscores.highscoreEntryList[j].time.ConvertToSeconds()
                    > highscores.highscoreEntryList[i].score / highscores.highscoreEntryList[i].time.ConvertToSeconds())
                {
                    //Swap
                    HighscoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }
        int highscoreListCount=highscores.highscoreEntryList.Count;
        for (int i = highscoreListCount; i > _maxQuantOfCompetetors; i--)
        {
            highscores.highscoreEntryList.RemoveAt(i-1);
        }
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, _entryContainer, _highscoreEntryTransformList);
        } 
        
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
        public TimeRes time;
        
    }
} 
    [System.Serializable]
    public struct TimeRes 
    {
       public float seconds;
       public float minutes;   

    public float ConvertToSeconds()
    {
        return minutes *60+seconds; 
    }
       
    public string ConvertToText()
    {
        return ((int)minutes).ToString("D2") + ":" + ((int)seconds).ToString("D2");
    }
    public void ResetTime()
    {
        minutes = seconds = 0;
    }
    }
