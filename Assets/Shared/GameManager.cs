using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private GameObject gameObject;

    private static GameManager m_Instance;
    public static GameManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new GameManager();
                m_Instance.gameObject = new GameObject("_gameManager");
                m_Instance.gameObject.AddComponent<InputController>();
            }
            return m_Instance;
        }
    }

    private InputController m_InputController;
    public InputController InputController
    {
        get
        {
            if (m_InputController == null)
                m_InputController = gameObject.GetComponent<InputController>();
            return m_InputController;
        }
    }

    private List<Player> m_players;
    public Player Player
    {
        set
        {
            if (m_players == null)
                m_players = new List<Player>();
                value.TakeHit();

            m_players.Add(value);
        }
    }

    public void TurnChanged(Player t)
    {
        foreach (Player p in m_players)
            p.SetTurn(false);
    }
}
