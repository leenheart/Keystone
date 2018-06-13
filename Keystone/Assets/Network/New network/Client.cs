﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player
{
    public string playerName;
    public GameObject avatar;
    public int connectionId;
    public string DefOrAtt;
}

public class Client : MonoBehaviour
{

    private const int MAW_CONNECTION = 100;
    private int port = 3778;

    private int hostId;

    private int reliableChannel;
    private int unreliableChannel;

    private int ourClientId;
    private int connectionId;

    private float connectionTime;
    private bool isConnected = false;
    private bool isStarted = false;
    private bool GameLunch = false;
    private byte error;

    private string playerName;

    public GameObject playerPrefab;
    public Dictionary<int, Player> players = new Dictionary<int, Player>();

    public void Connect()
    {
        //Does the player have a name ?
        playerName = "default name";

        DontDestroyOnLoad(gameObject);

        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(cc, MAW_CONNECTION);

        hostId = NetworkTransport.AddHost(topo, 0);
        connectionId = NetworkTransport.Connect(hostId, "25.49.49.53" /*"25.48.15.239"*/, port, 0, out error);

        connectionTime = Time.time;
        isConnected = true;

    }

    private void Update()
    {
        if (!isConnected) return;
        //Debug.Log("Try to connect !");

        if (!GameLunch && players.Count >= 2)
        {
            GameObject.Find("LockAndStart").GetComponent<Image>().enabled = true;
            GameObject.Find("LockAndStart").GetComponent<Button>().enabled = true;
            GameLunch = true;
        }
        if (GameLunch && players.Count < 2)
        {
            //Erreur disconnect mette en pause ou quelqu chose
        }


        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;

        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
        NetworkError networkError = (NetworkError)error;
        if (networkError != NetworkError.Ok)
        {
            Debug.LogError(string.Format("Error recieving event: {0} with recHostId: {1}, recConnectionId: {2}, recChannelId: {3}", networkError, recHostId, connectionId, channelId));
        }
        switch (recData)
        {
            case NetworkEventType.ConnectEvent:
                print("connectionRecieved");
                break;

            case NetworkEventType.DisconnectEvent:
                Application.Quit();
                break;

            case NetworkEventType.DataEvent:
                string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                //Debug.Log("Receiving : " + msg);

                string[] splitData = msg.Split('|');
                string[] coord;

                switch (splitData[0])
                {
                    case "":
                        break;

                    case "Refresh":
                        players[int.Parse(splitData[1])].avatar.GetComponent<Guardian>().Hp = int.Parse(splitData[2]);
                        break;

                    case "MOOVE":
                        coord = splitData[2].Split('%');
                        players[int.Parse(splitData[1])].avatar.GetComponent<Guardian>().Moove(new Vector3(int.Parse(coord[0].Split('.')[0]), 0, int.Parse(coord[1].Split('.')[0])));
                        break;

                    case "SPELL1":
                        coord = splitData[2].Split('%');
                        players[int.Parse(splitData[1])].avatar.GetComponent<Guardian>().Spell1Activation(new Vector3(int.Parse(coord[0].Split('.')[0]), 0, int.Parse(coord[1].Split('.')[0])));
                        break;

                    case "SPELL2":
                        coord = splitData[2].Split('%');
                        players[int.Parse(splitData[1])].avatar.GetComponent<Guardian>().Spell2Activation(new Vector3(int.Parse(coord[0].Split('.')[0]), 0, int.Parse(coord[1].Split('.')[0])));
                        break;

                    case "SPELL3":
                        coord = splitData[2].Split('%');
                        players[int.Parse(splitData[1])].avatar.GetComponent<Guardian>().Spell3Activation(new Vector3(int.Parse(coord[0].Split('.')[0]), 0, int.Parse(coord[1].Split('.')[0])));
                        break;

                    case "SPELL4":
                        coord = splitData[2].Split('%');
                        players[int.Parse(splitData[1])].avatar.GetComponent<Guardian>().Spell4Activation(new Vector3(int.Parse(coord[0].Split('.')[0]), 0, int.Parse(coord[1].Split('.')[0])));
                        break;

                    case "UpdatePlayer":
                        coord = splitData[2].Split('%');
                        // Debug.Log("avant  " + players[int.Parse(splitData[1])].avatar.transform.position);
                        players[int.Parse(splitData[1])].avatar.transform.position = new Vector3(float.Parse(coord[0]), float.Parse(coord[1]), float.Parse(coord[2]));
                        //players[int.Parse(splitData[1])].avatar.transform.rotation = Quaternion.Euler(float.Parse(coord[3]), float.Parse(coord[4]), float.Parse(coord[5]));
                        //Debug.Log("apres  " + players[int.Parse(splitData[1])].avatar.transform.rotation);
                        break;

                    case "PASSTURN":
                        GameObject.Find("Manager").GetComponent<Manager>().endNextTurn = Time.time;
                        if(splitData[1] == "Attacker")
                        {
                            GameObject.Find("Manager").GetComponent<Manager>().PlayeurNow = GameObject.Find("Manager").GetComponent<Manager>().Attacker;
                        }
                        else
                        {
                            GameObject.Find("Manager").GetComponent<Manager>().PlayeurNow = GameObject.Find("Manager").GetComponent<Manager>().Defender;
                        }
                        break;

                    case "ASKNAME":
                        OnAskName(splitData);
                        break;

                    case "Start":
                        GameObject.Find("Manager").GetComponent<Manager>().enabled = true;
                        GameObject.Find("Manager").GetComponent<Manager>().Defender = players[hostId].avatar;
                        GameObject.Find("Manager").GetComponent<Manager>().Attacker = players[this.connectionId].avatar;
                        GameObject.Find("Manager").GetComponent<Manager>().PlayerAvatar = players[this.connectionId].avatar;

                        foreach (KeyValuePair<int, Player> dic in players)
                        {
                            dic.Value.avatar.GetComponent<Rigidbody>().isKinematic = false;
                        }
                        break;

                    case "GM":
                        GameObject.Find("MapGeneration 1").GetComponent<Generation>().Generate();
                        break;

                    case "EM":
                        Debug.Log(splitData[1]);
                        GameObject.Find("MapGeneration 1").GetComponent<Generation>().MapString += splitData[1];
                        break;

                    case "Obstacle":
                        ///Debug.Log(splitData[1] + splitData[2] + splitData[3] + splitData[4]);
                        DontDestroyOnLoad(Instantiate(Resources.Load(splitData[1]), new Vector3(float.Parse(splitData[2]), float.Parse(splitData[3]), float.Parse(splitData[4])), new Quaternion()));
                        break;

                    case "CNN":
                        SpawnPlayer(splitData[1], int.Parse(splitData[2]));
                        break;

                    case "DC":
                        PlayerDisconnected(int.Parse(splitData[1]));
                        break;

                    default:
                        Debug.Log("Invalid message :" + msg);
                        break;
                }

                break;

            case NetworkEventType.BroadcastEvent:

                break;
        }
    }

    private void OnAskName(string[] data)
    {
        // Set this clients ID
        ourClientId = int.Parse(data[1]);

        //Send our name to the server
        Send("NAMEIS|" + playerName, reliableChannel);

        //Create all t he other players
        for (int i = 2; i < data.Length - 1; i++)
        {
            string[] d = data[i].Split('%');
            SpawnPlayer(d[0], int.Parse(d[1]));
        }
    }

    private void SpawnPlayer(string playerName, int cnnId)
    {
        GameObject go = Instantiate(playerPrefab, new Vector3(2, 3, 25), new Quaternion());

        Player p = new Player();
        p.avatar = go;
        p.playerName = playerName;
        p.connectionId = cnnId;

        // Is this ours ?
        if (cnnId == ourClientId)
        {
            go.transform.rotation = Quaternion.Euler(0, 90, 0);
            //Do Some staff to spawn
            go.GetComponentsInChildren<SpriteRenderer>()[0].enabled = true;
            go.GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;

            isStarted = true;
            p.DefOrAtt = "Att";
        }
        else
        {
            go.transform.rotation = Quaternion.Euler(0, -90, 0);
            go.GetComponentsInChildren<SpriteRenderer>()[0].enabled = false;
            go.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;
            p.DefOrAtt = "Def";
            go.transform.position = new Vector3(48, 3, 25);
        }

        players.Add(cnnId, p);

        //Debug.Log("spawn player " + playerName + players.Count);

    }
    private void PlayerDisconnected(int cnnId)
    {
        Destroy(players[cnnId].avatar);
        players.Remove(cnnId);
    }

    private void Send(string message, int channelId)
    {
        // Debug.Log("Sending : " + message);
        byte[] msg = Encoding.Unicode.GetBytes(message);
        NetworkTransport.Send(hostId, connectionId, channelId, msg, message.Length * sizeof(char), out error);


    }

    public void GenerateMap()
    {
        Send("ASKGENERATEMAP|", reliableChannel);
    }
    public void Ready()
    {
        Send("READY|", reliableChannel);
    }

    public void PassTurn()
    {
        GameObject.Find("Manager").GetComponent<Manager>().endNextTurn = Time.time;
        Send("PASSTURN|", reliableChannel);
    }

    public void Moove(Vector3 hitPoint)
    {
        Send("MOOVE|" + hitPoint.x + "%" + hitPoint.z, reliableChannel);
    }

    public void Spell(Vector3 hitPoint, int NumSpell)
    {
        Send("SPELL" + NumSpell + "|" + hitPoint.x + "%" + hitPoint.z, reliableChannel);
    }
}
