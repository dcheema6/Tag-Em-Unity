using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    [SerializeField] GameObject camera;

    [SerializeField] GameObject inGameUI;
    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject endMenu;
    [SerializeField] Transform leaderBoard;

    [SerializeField] Text scoreText;
    [SerializeField] Text turnText;
    [SerializeField] Text timeLeft;

    [SerializeField] Player mainPlayer;
    [SerializeField] Player otherPlayer;

    [SerializeField] uint gameTime;

    float begin;
    short gameState;

    Player p1;
    Player p2;

    List<string[]> highScores;
    List<Transform> hsEntries;

    void Start()
    {
        inGameUI.SetActive(false);
        endMenu.SetActive(false);
        leaderBoard.gameObject.SetActive(false);
        startMenu.SetActive(true);
        gameState = 0;
        PopulateHighScoreList();
        InstantiateLeaderBoard();
    }

    void Update()
    {
        if (gameState == 1)
        {
            if ((Time.time - begin) >= gameTime)
            {
                EndGame();            
                return;
            }

            UpdateInGameUI();
        }
    }

    void EndGame()
    {
        camera.SetActive(true);
        endMenu.SetActive(true);
        timeLeft.text = "TIME IS UP!";

        if (p1.isMyTurn())
            turnText.text = "You Lost :(";
        else
            turnText.text = "YOU WON Xp";

        UpdateHighScores();
        p1.Die();
        p2.Die();
        gameState = 2;
        SaveHighScores();
    }

    void UpdateInGameUI()
    {
        scoreText.text = "Your Score " + p1.Score;
        timeLeft.text = "Time Left " + (uint)(gameTime - (Time.time - begin));

        if (p1.isMyTurn())
            turnText.text = p1.DisplayText + " Turn";
        else
            turnText.text = p2.DisplayText + " Turn";
    }

    void PopulateHighScoreList()
    {
        highScores = new List<string[]>();
        string hsl = PlayerPrefs.GetString("HSL");
        string[] scores = hsl.Split('\n');
        
        for (int i = 0; i < 9; i++)
        {
            if (i < scores.Length && scores[i] != "") 
                highScores.Add(scores[i].Split('\t'));
            else
            {
                string[] t = new string[2];
                t[0] = "NAME"; t[1] = "0";
                highScores.Add(t);
            }
        }
    }

    void UpdateHighScores()
    {
        int n = 0;

        while (n < 9)
        {
            if (p2.Score < 0 && p1.Score < 0) break;

            if (p2.Score > -1 && Int32.Parse(highScores[n][1]) <= p2.Score)
            {
                string[] temp = new string[2];
                temp[0] = "AI"; temp[1] = "" + p2.Score;
                highScores.Insert(n, temp);
                p2.Score = -1;
            }

            if (p1.Score > -1 && Int32.Parse(highScores[n][1]) <= p1.Score)
            {
                string[] temp = new string[2];
                temp[0] = "HUMAN"; temp[1] = "" + p1.Score;
                highScores.Insert(n, temp);
                p1.Score = -1;
            }

            n++;
        }

        while (highScores.Count > 9)
        {
            highScores.RemoveAt(9);
        }
    }

    void SaveHighScores()
    {
        string s = "";
        
        foreach (string[] hs in highScores)
        {
            s += hs[0] + "\t" + hs[1] + "\n";
        }

        PlayerPrefs.SetString("HSL", s);
        PlayerPrefs.Save();
    }

    void InstantiateLeaderBoard()
    {
        hsEntries = new List<Transform>();
        Transform entryTemplate = leaderBoard.Find("EntryTemplate");
        entryTemplate.gameObject.SetActive(false);

        for (int i = 0; i < highScores.Count; i++)
        {
            Transform entryTransform = Instantiate(entryTemplate, leaderBoard.transform);
            RectTransform ert = entryTransform.GetComponent<RectTransform>();
            ert.anchoredPosition = new Vector2(0, -50 * i);
            entryTransform.gameObject.SetActive(true);

            hsEntries.Add(entryTransform);
        }
    }

    public void StartGame()
    {
        begin = Time.time;

        p1 = Instantiate(mainPlayer, transform.position, transform.rotation);
        p2 = Instantiate(otherPlayer, transform.position + new Vector3(0.0f, 0.0f, 30.0f),
        transform.rotation);
        
        p2.AIHelper(p1.transform);

        startMenu.SetActive(false);
        endMenu.SetActive(false);
        camera.SetActive(false);
        inGameUI.SetActive(true);
        gameState = 1;
    }

    public void BackToMenu()
    {
        endMenu.SetActive(false);
        leaderBoard.gameObject.SetActive(false);
        startMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        EditorApplication.isPlaying = false;
    }

    public void ShowLeaderBoard()
    {
        for (int i = 0; i < hsEntries.Count; i++)
        {
            string rank;
            switch(i)
            {
                default:
                    rank = (i + 1) + "TH";
                    break;
                case 0:
                    rank = (i + 1) + "ST";
                    break;
                case 1:
                    rank = (i + 1) + "ND";
                    break;
                case 2:                    
                    rank = (i + 1) + "RD";
                    break;
            }

            hsEntries[i].Find("Ranking").GetComponent<Text>().text = rank + "    "
                + highScores[i][0];
            hsEntries[i].Find("Score").GetComponent<Text>().text = highScores[i][1];
        }

        startMenu.SetActive(false);
        leaderBoard.gameObject.SetActive(true);
    }
}
