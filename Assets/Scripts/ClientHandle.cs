using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using System;
public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
        GameManager.instance.StartSendingTimeStamp();
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        if (GameManager.players.ContainsKey(_id))
        {
            GameManager.players[_id].transform.position = _position;
        }
        
    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        if (GameManager.players.ContainsKey(_id))
        {
            GameManager.players[_id].transform.rotation = _rotation;
        }
        
    }

    public static void PlayerShoot(Packet _packet)
    {
        int _id = _packet.ReadInt();

        AudioManager.instance.GunShot();

        if (GameManager.players.ContainsKey(_id))
        {
            GameManager.players[_id].gun.gunAnimator.SetTrigger("Fire");
        }
        
    }
    
    public static void PlayerAnimation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        bool _isMoving = _packet.ReadBool();


        if(GameManager.players.ContainsKey(_id))
        {
            GameManager.players[_id].animator.SetBool("isRunning", _isMoving);
        }
        
    }
    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();

        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
    }

    public static void PlayerHealth(Packet _packet)
    {
        int _id = _packet.ReadInt();
        float _health = _packet.ReadFloat();

        GameManager.players[_id].SetHealth(_health);
    }

    public static void PlayerRespawn(Packet _packet)
    {
        int _id = _packet.ReadInt();

        GameManager.players[_id].Respawn();
    }

    public static void CalculateTimeDelta(Packet _packet)
    {
        GameManager.instance.roundtrip = (int)((long)(DateTime.UtcNow - 
            new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds - _packet.ReadLong());

        GameManager.instance.latency = GameManager.instance.roundtrip / 2;

        int serverDelta = (int)(_packet.ReadLong() - (long)(DateTime.UtcNow -
            new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds);
        
        GameManager.instance.timeDelta = serverDelta + GameManager.instance.latency;

        UIManager.instance.DisplayNetworkVariables();
    }

    public static void SyncClock(Packet _packet)
    {
        DateTime dateNow = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);

        GameManager.instance.serverClock = dateNow.AddMilliseconds(_packet.ReadLong() +
            GameManager.instance.timeDelta).ToLocalTime();

        /*        if (!GameManager.instance.clockStarted)
                {
                    GameManager.instance.endTime = GameManager.instance.serverClock.AddMilliseconds(60000 + GameManager.instance.timeDelta);

                    GameManager.instance.clockStarted = true;
                }
        */
        if (GameManager.instance.clockStarted)
        {
            int _timeLeft = (GameManager.instance.endTime - GameManager.instance.serverClock).Seconds;
            UIManager.instance.timeLeft_Txt.text = $"Match Time Remaining: {_timeLeft} sec";

            if (_timeLeft <= 0)
            {
                Debug.Log("Game Over");

                
            }
        }
        
    }

    public static void GameTime(Packet _packet)
    {
        Debug.Log("Game Started");
        GameManager.instance.StartMatch();
        DateTime dateNow = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);

        if (!GameManager.instance.clockStarted)
        {
            GameManager.instance.endTime = dateNow.AddMilliseconds(_packet.ReadLong() + GameManager.instance.timeDelta);

            GameManager.instance.clockStarted = true;
        }
    }

    public static void EndGame(Packet _packet)
    {
        Debug.Log("Game Has Ended");
        for(int i = 0; i < GameManager.players.Count; i++)
        {
            int _id = _packet.ReadInt();
            int _score = _packet.ReadInt();

            GameManager.players[_id].score = _score;
            // GameManager.players[_packet.ReadInt()].score = _packet.ReadInt();
        }
        GameManager.instance.EndMatch();
        // Receive Packet containing the highscores 
        // Display leader
    }

    public static void WaitingForPlayers(Packet _packet)
    {
        UIManager.instance.timeLeft_Txt.text = "Waiting for more Players";
        UIManager.instance.RemoveLeaderboard();
    }

    public static void PlayerHit(Packet _packet)
    {
        AudioManager.instance.PlayerHit();
    }

    public static void PlayerRespawnRotation(Packet _packet)
    {
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.players[Client.instance.myId].camera.RespawnPlayer(_rotation);
    }
}
