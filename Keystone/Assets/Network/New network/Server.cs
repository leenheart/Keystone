using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerClient
{
    public int connectionId;
    public string playerName;
}

public class Server : MonoBehaviour
{

    public GameObject Map;

    private int nbReady = 0;

    private float nextUpdate = 0;

    private const int MAW_CONNECTION = 100;
    private int port = 7777;

    private int DefenderId;
    private int hostId;
    private int webHostId;

    private int reliableChannel;
    private int unreliableChannel;

    private bool GameLunch = false;
    private bool isStarted = false;
    private byte error;

    private List<ServerClient> clients = new List<ServerClient>();
    private ServerClient ClientServer = new ServerClient();

    public GameObject playerPrefab;
    public Dictionary<int, Player> players = new Dictionary<int, Player>();

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(cc, MAW_CONNECTION);

        hostId = NetworkTransport.AddHost(topo, port, null);
        webHostId = NetworkTransport.AddWebsocketHost(topo, port, null);

        isStarted = true;
        //Debug.Log(isStarted);

        ClientServer.connectionId = hostId;
        ClientServer.playerName = "Server";
        SpawnPlayer(ClientServer.playerName, hostId);

    }

    void Update()
    {
        if (!isStarted) return;

        if (!GameLunch && players.Count >= 2)
        {
            GameObject.Find("LockAndStart").GetComponent<Image>().enabled = true;
            GameObject.Find("LockAndStart").GetComponent<Button>().enabled = true;
            GameLunch = true;
        }
        if (GameLunch && players.Count < 2)
        {
            Application.Quit();
            //Erreur disconnect mette en pause ou quelqu chose
        }
        if (nextUpdate <= Time.time)
        {

            foreach (KeyValuePair<int, Player> dic in players)
            {

                Transform t = dic.Value.avatar.transform;
                //Debug.Log(t.position.x + "%" + t.position.y + "%" + t.position.z + "%" + t.rotation.x + "%" + t.rotation.y + "%" + t.rotation.z);
                Send("UpdatePlayer|" + dic.Key + "|" + t.position.x + "%" + t.position.y + "%" + t.position.z + "%" + t.rotation.x + "%" + t.rotation.y + "%" + t.rotation.z, unreliableChannel, clients);
            }

            nextUpdate = Time.time + 0.1f;
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
                Debug.Log("Player" + connectionId + "has connected");
                OnConnection(connectionId);
                break;
            case NetworkEventType.DataEvent:
                string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                //Debug.Log("Receive from " + connectionId + " :" + msg);
                string[] splitData = msg.Split('|');

                switch (splitData[0])
                {
                    case "MOOVE":
                        Moove(connectionId, splitData[1]);
                        break;

                    case "SPELL1":
                        Spell1(connectionId, splitData[1]);
                        break;

                    case "SPELL2":
                        Spell2(connectionId, splitData[1]);
                        break;

                    case "SPELL3":
                        Spell3(connectionId, splitData[1]);
                        break;

                    case "SPELL4":
                        Spell4(connectionId, splitData[1]);
                        break;

                    case "PASSTURN":
                        GameObject.Find("Manager").GetComponent<Manager>().endNextTurn = Time.time;
                        break;

                    case "NAMEIS":
                        OnNameIs(connectionId, splitData[1]);
                        break;

                    case "READY":
                        nbReady++;
                        if (nbReady >= 2)
                        {
                            GameObject.Find("Manager").GetComponent<Manager>().enabled = true;
                            GameObject.Find("Manager").GetComponent<Manager>().Defender = players[hostId].avatar;
                            GameObject.Find("Manager").GetComponent<Manager>().Attacker = players[connectionId].avatar;
                            GameObject.Find("Manager").GetComponent<Manager>().PlayerAvatar = players[hostId].avatar;
                            foreach (KeyValuePair<int, Player> dic in players)
                            {
                                dic.Value.avatar.GetComponent<Rigidbody>().isKinematic = false;
                            }
                            Send("Start|", reliableChannel, clients);
                            PassTurn();
                        }
                        DefenderId = connectionId;
                        break;

                    case "ASKGENERATEMAP":
                        //Debug.Log("askGenerate map");
                        GenerateMap(connectionId);
                        break;

                    default:
                        Debug.Log("Invalid message :" + msg);
                        break;
                }
                break;

            case NetworkEventType.DisconnectEvent:
                Debug.Log("Player" + connectionId + "has deconnected");
                OnDisconnection(connectionId);
                Application.Quit();
                break;

            case NetworkEventType.BroadcastEvent:

                break;
        }
    }

    private void sendObstacle()
    {
        //recuperer et envoyer tous les obstacle !
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            string name = "";
            if (g.name == "dec(Clone)")
            {
                name = "dec";
            }
            else
            {
                name = "tree";
            }

            Transform t = g.transform;
            Send("Obstacle|" + name + "|" + t.position.x + "|" + t.position.y + "|" + t.position.z , reliableChannel, clients);

        }
    }

    private void OnNameIs(int cnnId, string playerName)
    {
        //Link the name to the connection ID
        clients.Find(x => x.connectionId == cnnId).playerName = playerName;

        // Tell everybody taht a new player has connected
        Send("CNN|" + playerName + '|' + cnnId, reliableChannel, clients);
        SpawnPlayer(playerName, cnnId);
    }

    private void OnConnection(int cnnId)
    {
        // add him to a list
        ServerClient c = new ServerClient();
        c.connectionId = cnnId;
        c.playerName = "TEMPS";
        clients.Add(c);

        //when the player join, tell hin is ID

        // Request his name and send it to others players
        string msg = "ASKNAME|" + cnnId + "|" + ClientServer.playerName + "%" + ClientServer.connectionId + "|";
        foreach (ServerClient sc in clients)
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
        players.Remove(cnnId);

        //tell everyone he has disconnected
        Send("DC|" + cnnId, reliableChannel, clients);
    }

    private void Send(string message, int channelId, int cnnId)
    {
        List<ServerClient> c = new List<ServerClient>();
        c.Add(clients.Find(x => x.connectionId == cnnId));
        Send(message, channelId, c);
    }
    private void Send(string message, int channelId, List<ServerClient> c)
    {
        byte[] msg = Encoding.Unicode.GetBytes(message);
        foreach (ServerClient sc in c)
        {
            NetworkTransport.Send(hostId, sc.connectionId, channelId, msg, message.Length * sizeof(char), out error);
            NetworkError networkError = (NetworkError)error;
            if (networkError != NetworkError.Ok)
            {
                Debug.LogError(string.Format("Error recieving event: {0} with recHostId: {1}, recConnectionId: {2}, recChannelId: {3}", networkError, hostId, sc.connectionId, channelId));
            }
        }
        //Debug.Log("Sending : " + message);
    }

    public void GenerateMap(int cnnId)
    {
        string s = Map.GetComponent<Generation>().MapString;
        for (int i = 0; i < /*size map X*/50; i++)
        {
            //Debug.Log(s.Substring(50 * i, 50 ));
            Send("EM|" + s.Substring(50 * i, 50), reliableChannel, cnnId);
        }

        {
            Send("GM|", reliableChannel, cnnId);
        }

        sendObstacle();
    }

    private void SpawnPlayer(string playerName, int cnnId)
    {
        GameObject go = Instantiate(playerPrefab, new Vector3(2, 3, 25), new Quaternion());

        Player p = new Player();
        p.avatar = go;
        p.playerName = playerName;
        p.connectionId = cnnId;


        // Is this ours ?
        if (cnnId == hostId)
        {
            //Do Some staff to spawn
            go.transform.position = new Vector3(48, 3, 25);
            p.DefOrAtt = "Def";
            isStarted = true;
        }
        else
        {
            p.DefOrAtt = "Att";
        }
        players.Add(cnnId, p);

    }

    public void Ready()
    {
        nbReady++;
        if (nbReady >= 2)
        {
            Send("Start|", reliableChannel, clients);
            GameObject.Find("Manager").GetComponent<Manager>().enabled = true;
            GameObject.Find("Manager").GetComponent<Manager>().Defender = players[hostId].avatar;
            GameObject.Find("Manager").GetComponent<Manager>().Attacker = players[DefenderId].avatar;
            GameObject.Find("Manager").GetComponent<Manager>().PlayerAvatar = players[hostId].avatar;
            foreach (KeyValuePair<int, Player> dic in players)
            {
                dic.Value.avatar.GetComponent<Rigidbody>().isKinematic = false;
            }
            PassTurn();
        }
    }

    public void PassTurn()
    {
        GameObject.Find("Manager").GetComponent<Manager>().endNextTurn = Time.time;
        Send("PASSTURN|", reliableChannel, clients);
    }

    public void Moove(int cnnId, string posXY)
    {
        string[] coord = posXY.Split('%');
        players[cnnId].avatar.GetComponent<Guardian>().Moove(new Vector3(int.Parse(coord[0].Split('.')[0]), 0, int.Parse(coord[1].Split('.')[0])));
        //Send("MOOVE|" + cnnId + "|" + posXY, reliableChannel, clients);
    }

    public void Spell1(int cnnId, string posXY)
    {
        string[] coord = posXY.Split('%');
        players[cnnId].avatar.GetComponent<Guardian>().Spell1Activation(new Vector3(int.Parse(coord[0].Split('.')[0]), 0, int.Parse(coord[1].Split('.')[0])));
        //Send("SPELL1|" + cnnId + "|" + posXY, reliableChannel, clients);
    }

    public void Spell2(int cnnId, string posXY)
    {
        string[] coord = posXY.Split('%');
        players[cnnId].avatar.GetComponent<Guardian>().Spell2Activation(new Vector3(int.Parse(coord[0].Split('.')[0]), 0, int.Parse(coord[1].Split('.')[0])));
        //Send("SPELL2|" + cnnId + "|" + posXY, reliableChannel, clients);
    }

    public void Spell3(int cnnId, string posXY)
    {
        string[] coord = posXY.Split('%');
        players[cnnId].avatar.GetComponent<Guardian>().Spell3Activation(new Vector3(int.Parse(coord[0].Split('.')[0]), 0, int.Parse(coord[1].Split('.')[0])));
        //Send("SPELL3|" + cnnId + "|" + posXY, reliableChannel, clients);
    }
    public void Spell4(int cnnId, string posXY)
    {
        string[] coord = posXY.Split('%');
        players[cnnId].avatar.GetComponent<Guardian>().Spell4Activation(new Vector3(int.Parse(coord[0].Split('.')[0]), 0, int.Parse(coord[1].Split('.')[0])));
        //Send("SPELL4|" + cnnId + "|" + posXY, reliableChannel, clients);
    }

    public void Moove(Vector3 hitPoint)
    {
        Send("MOOVE|" + hostId + "|" + hitPoint.x + "%" + hitPoint.z, reliableChannel, clients);
    }

    public void Spell(Vector3 hitPoint, int NumSpell)
    {
        Send("SPELL" + NumSpell + "|" + hostId + "|" + hitPoint.x + "%" + hitPoint.z, reliableChannel, clients);
    }
}
