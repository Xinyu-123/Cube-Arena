using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;

    public DateTime serverClock;
    public int timeDelta, latency, roundtrip;
    public bool clockStarted = false;
    public long gameTimer = 0;
    public DateTime endTime;
    public long waitTimer = 0;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }

    }

  
    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;

        if(_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, _rotation);
        }

        _player.GetComponent<PlayerManager>().Initialize(_id, _username);
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }

    public void EndMatch()
    {
        instance.clockStarted = false;
        UIManager.instance.timeLeft_Txt.text = $"Match Over, Restarting in 10 seconds";

        // Display the leaderboard
        UIManager.instance.SetLeaderboardDisplay();
    }

    public void StartMatch()
    {
        UIManager.instance.RemoveLeaderboard();
    }

    public void StartSendingTimeStamp()
    {
        StartCoroutine(SendTimeStamp());
    }

    private IEnumerator SendTimeStamp()
    {
        ClientSend.ClientTimeStamp();

        yield return new WaitForSeconds(5f);
        StartCoroutine(SendTimeStamp());
    }
}
