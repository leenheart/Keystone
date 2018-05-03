using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Player
{
    public string playerName;
    public GameObject avatar;
    public int connectionId;
}

public class Client : MonoBehaviour {

    private const int MAW_CONNECTION = 100;
    private int port = 5701;

    private int hostId;
    private int webHostId;

    private int reliableChannel;
    private int unreliableChannel;

    private int ourClientId;
    private int connectionId;

    private float connectionTime;
    private bool isConnected = false;
    private bool isStarted = false;
    private byte error;

    private string playerName;

    public GameObject playerPrefab;
    public Dictionary<int, Player> players = new Dictionary<int, Player>();

    public void Connect()
    {
        //Does the player have a name ?
        playerName = "default name";


        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(cc, MAW_CONNECTION);

        hostId = NetworkTransport.AddHost(topo, port, null);
        connectionId = NetworkTransport.Connect(hostId, "LOCALHOST", port, 0, out error);

        connectionTime = Time.time;
        isConnected = true;
    }

    private void Update()
    {
        if (!isConnected) return;


        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;
        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
        switch (recData)
        {
            case NetworkEventType.DataEvent:
                string msg = Encoding.Unicode.GetString(recBuffer, 0 ,dataSize);
                Debug.Log("Receiving : " + msg);

                string[] splitData = msg.Split('|');
                switch (splitData[0])
                {
                    case "ASKNAME":
                        OnAskName(splitData);
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
        Send("NAMEIS" + playerName, reliableChannel);

        //Create all the other players
        for(int i = 2; i > data.Length -1; i++)
        {
            string[] d = data[i].Split('%');
            SpawnPlayer(d[0], int.Parse(d[1]));
        }
    }

    private void SpawnPlayer( string playerName, int cnnId)
    {
        GameObject go = Instantiate(playerPrefab);

        // Is this ours ?
        if (cnnId == ourClientId)
        {
            //Do Some staff to spawn
            isStarted = true;
        }

        Player p = new Player();
        p.avatar = go;
        p.playerName = playerName;
        p.connectionId = cnnId;
        players.Add(cnnId, p);

    }
    private void PlayerDisconnected(int cnnId)
    {
        Destroy(players[cnnId].avatar);
        players.Remove(cnnId);
    }

    private void Send(string message, int channelId)
    {
        Debug.Log("Sending : " + message);
        byte[] msg = Encoding.Unicode.GetBytes(message);
        NetworkTransport.Send(hostId, connectionId, channelId, msg, message.Length * sizeof(char), out error);

    }
}
