﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ServerClient
{
    public int connectionId;
    public string playerName;
}

public class Server : MonoBehaviour {

    private const int MAW_CONNECTION = 100;
    private int port = 5701;

    private int hostId;
    private int webHostId;

    private int reliableChannel;
    private int unreliableChannel;

    private bool isStarted = false;
    private byte error;

    private List<ServerClient> clients = new List<ServerClient>();

    private void Start()
    {
        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(cc, MAW_CONNECTION);

        hostId = NetworkTransport.AddHost(topo, port, null);
        webHostId = NetworkTransport.AddWebsocketHost(topo, port, null);

        isStarted = true;
    }

    void Update()
    {
        if (!isStarted) return;

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
            case NetworkEventType.ConnectEvent:
                Debug.Log("Player" + connectionId + "has connected");
                OnConnection(connectionId);
                break;
            case NetworkEventType.DataEvent:
                string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                Debug.Log("Receive from " + connectionId + " :" + msg);
                string[] splitData = msg.Split('|');

                switch (splitData[0])
                {
                    case "NAMEIS":
                        OnNameIs(connectionId,splitData[1]);
                        break;
                    case "CNN":
                        break;
                    case "DC":
                        break;

                    default:
                        Debug.Log("Invalid message :" + msg);
                        break;
                }
                break;

            case NetworkEventType.DisconnectEvent:
                Debug.Log("Player" + connectionId + "has deconnected");
                OnDisconnection(connectionId);
                break;

            case NetworkEventType.BroadcastEvent:

                break;
        }
    }

    private void OnNameIs(int cnnId, string playerName)
    {
        //Link the name to the connection ID
        clients.Find(x => x.connectionId == cnnId).playerName = playerName;

        // Tell everybody taht a new player has connected
        Send("CNN|" + playerName + '|' + cnnId,reliableChannel,clients);
    }

    private void OnConnection( int cnnId)
    {
        // add him to a list
        ServerClient c = new ServerClient();
        c.connectionId = cnnId;
        c.playerName = "TEMPS";
        clients.Add(c);


        //when the player join, tell hin is ID

        // Request his name and send it to others players
        string msg = "ASKNAME|" + cnnId + "|";
        foreach(ServerClient sc in clients)
        {
            msg += sc.playerName + "%" + sc.connectionId + "|";
        }
        msg = msg.Trim('|');

        Send(msg, reliableChannel, cnnId);
    }
    private void OnDisconnection(int cnnId)
    {
        //Remove this player from our client list
        clients.Remove(clients.Find(x => x.connectionId == cnnId));

        //tell everyone he has disconnected
        Send("DC|" + cnnId, reliableChannel, clients);
    }

    private void Send (string message, int channelId, int cnnId)
    {
        List<ServerClient> c = new List<ServerClient>();
        c.Add(clients.Find(x => x.connectionId == cnnId));
        Send(message, channelId, c);
    }

    private void Send(string message, int channelId, List<ServerClient> c)
    {
        Debug.Log("Sending : " + message);
        byte[] msg = Encoding.Unicode.GetBytes(message);
        foreach ( ServerClient sc in c)
        {
            NetworkTransport.Send(hostId, sc.connectionId, channelId, msg, message.Length * sizeof(char), out error);
        }

    }
}
