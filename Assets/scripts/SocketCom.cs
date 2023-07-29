using UnityEngine;
using TMPro;
using System;
using System.Text;
using System.Net;
using System.Runtime.InteropServices;
#if UNITY_STANDALONE
using System.Net.Sockets;
#endif

public class SocketCom : MonoBehaviour
{
#if UNITY_STANDALONE
    private const int PortSend = 12346; // Choose a port number for communication
    private TcpClient clientSender;
    private bool socketConnected = false;

    private const int PortReceive = 12345; // Choose a port number for communication
    private TcpListener server;
    private TcpClient clientReceiver;
    private NetworkStream stream;
    private byte[] receiveBuffer = new byte[1024];
#endif

    [DllImport("__Internal")]
    private static extern void sendDataToWeb(string data);
   
    [SerializeField]
    private GameObject gm;
    private gms gmSc = new gms();

    
    [SerializeField]
    private TextMeshProUGUI txtLog;

    private string dataFromFlash;

    private void Awake()
    {
        gmSc = gm.GetComponent<gms>();
    }


#if UNITY_STANDALONE
    private void Start()
    {

        log("unity started");
        try
        {
            clientSender = new TcpClient();
            clientSender.Connect("localhost", PortSend);
            socketConnected = true;
            log("yes");
        }
        catch (Exception e)
        {
            socketConnected = false;
            log($"no: error msg = {e.Message}");
        }

        server = new TcpListener(IPAddress.Any, PortReceive);
        
        server.Start();
        log($"is bound: {server.Server.IsBound}");
        //log($"is connected: {server.Server.Connected}");

        // Start listening for incoming connections
        server.BeginAcceptTcpClient(HandleIncomingConnection, null);
        //log($"is connected: {server.Server.Connected}");
    }


    private void HandleIncomingConnection(IAsyncResult ar)
    {
        log("HandleIncomingConnection");

        clientReceiver = server.EndAcceptTcpClient(ar);
        stream = clientReceiver.GetStream();

        // Start receiving data from the client
        stream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, ReceiveCallback, null);
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        
        log("ReceiveCallback");
        int bytesRead = stream.EndRead(ar);
        if (bytesRead <= 0)
        {
            log("bytesRead <= 0");
            CloseConnection();
            return;
        }

        log("Process the received message");

        dataFromFlash += Encoding.UTF8.GetString(receiveBuffer, 0, bytesRead);
        //log("Received message from Adobe AIR: " + message);
        if(dataFromFlash.Substring(dataFromFlash.Length-2) == "$$")
        {
            executeUnity(dataFromFlash.Remove(dataFromFlash.Length-2));
            dataFromFlash = "";
        }

        // Continue listening for incoming data
        stream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, ReceiveCallback, null);
    }
    private void CloseConnection()
    {
        log("CloseConnection");
        stream.Close();
        clientReceiver.Close();

        // Start listening for another incoming connection
        server.BeginAcceptTcpClient(HandleIncomingConnection, null);
    }

    private void OnDestroy()
    {
        if (stream != null)
            stream.Close();

        if (clientReceiver != null)
            clientReceiver.Close();

        if (server != null)
            server.Stop();
    }
#endif
    public void executeUnity(string message)
    {
        gmSc.executeUnity(message);
    }

    


    public void sendData(string data)
    {
#if UNITY_EDITOR

#elif UNITY_WEBGL
        sendDataToWeb(data);
#elif UNITY_STANDALONE
        if (!socketConnected)
        {
            log("client not connected");
            return;
        }

        log(data);
        NetworkStream stream = clientSender.GetStream();
        byte[] message = Encoding.ASCII.GetBytes(data);
        stream.Write(message, 0, message.Length);
#endif

    }



    private void log(string str)
    {
        txtLog.text += str + "\n";
    }

}

