using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public GameObject leaderBoard;
    public GameObject respawnText;
    public Transform entryContainer;
    public Transform entryTemplate;
    public InputField usernameField;
    public Text networkStats;
    public Text timeLeft_Txt;

    private List<HighScoreEntry> highscores;

    float templateHeight = 50f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }

        entryTemplate.gameObject.SetActive(false);

        highscores = new List<HighScoreEntry>();
    }

    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        usernameField.interactable = false;
        Client.instance.ConnectToServer();
    }

    public void DisplayNetworkVariables()
    {
        networkStats.text = $"Latency: {GameManager.instance.latency}ms | Time Delta: {GameManager.instance.timeDelta}ms";
    }

    public void SetLeaderboardDisplay()
    {

        // Store all the scores and usernames in a list
        foreach (KeyValuePair<int, PlayerManager> entry in GameManager.players)
        {
            // do something with entry.Value or entry.Key
            AddHighscoreEntry(entry.Value.score, entry.Value.username);
        }

        // Sort the highscores
        SortLeaderBoard();

        // Display the list
        int i = 0;
        foreach (HighScoreEntry entry in highscores)
        {
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
            i++;
            entryTransform.gameObject.SetActive(true);

            entryTransform.Find("Name").GetComponent<Text>().text = entry.username;
            entryTransform.Find("Score").GetComponent<Text>().text = entry.score.ToString();
        }
        
        leaderBoard.SetActive(true);
        
    }

    public void RemoveLeaderboard()
    {
        highscores = new List<HighScoreEntry>();

        for (int i = 1; i < entryContainer.childCount; i++)
        {
            Destroy(entryContainer.GetChild(i).gameObject);
        }
        leaderBoard.SetActive(false);
    }

    private void AddHighscoreEntry(int _score, string _username)
    {
        highscores.Add(new HighScoreEntry
        {
            score = _score,
            username = _username
        });
    }

    private void SortLeaderBoard()
    {
        highscores.Sort(new SortDecending());
    }

    public class HighScoreEntry
    {
        public int score;

        public string username;
    }

    private class SortDecending : IComparer<HighScoreEntry>
    {
        int IComparer<HighScoreEntry>.Compare(HighScoreEntry a, HighScoreEntry b)
        {
            return b.score - a.score;
        }
    }

}
