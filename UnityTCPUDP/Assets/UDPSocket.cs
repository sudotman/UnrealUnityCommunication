using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class UDPSocket : MonoBehaviour
{
	
	private TcpListener tcpListener;

	private Socket socket;

	private Thread udpSocketThread;



	private UdpClient udpClient;

	private UdpClient connectedUdpClient;


	[SerializeField] private string serverIP = "127.0.0.1";

	[SerializeField] private int serverPort = 8052;

	[InspectorButton("RestartServer",100)]
	public char restartServer;

	[TextArea]
	[Tooltip("Doesn't do anything. Just shows the current status in inspector")]
	[SerializeField] private string currentStatus = "Status will be shown here.";

	private string connectedTo = "Connected To: Null";

	[Header("Debug")]
	[SerializeField] private bool sendMessagesForDebug = false;
	[SerializeField] private KeyCode debugMessagesKey = KeyCode.K;

	// Use this for initialization
	void Start()
	{
		// Start UDP Server background thread
		
		if(udpSocketThread == null)
			udpSocketThread = new Thread(new ThreadStart(RecieveIncomingMessages));
		udpSocketThread.IsBackground = true;
		udpSocketThread.Start();

	}

	void Update()
	{
		if (Input.GetKeyDown(debugMessagesKey))
		{
			SendMessage();
		}

		currentStatus = connectedTo;
	}

	private void RecieveIncomingMessages()
	{
		try
		{
			// Create listener on specified port ip			
			//tcpListener = new TcpListener(IPAddress.Parse(serverIP), serverPort);

			socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			socket.Bind(new IPEndPoint(IPAddress.Parse(serverIP), serverPort));

			//tcpListener.Start();

			Debug.Log("Server has been started and listening on: " + serverIP + ":" + serverPort);
			Byte[] bytes = new Byte[1024];
			while (true)
			{

				EndPoint point = new IPEndPoint(IPAddress.Any, 0);

				byte[] buffer = new byte[1024];

				int length = socket.ReceiveFrom(buffer, ref point); //recieve actual data

				string message = Encoding.UTF8.GetString(buffer, 0, length);

				//string clientMessage = Encoding.ASCII.GetString(incommingData);
				Debug.Log("Client said: " + message);


			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("SocketException " + socketException.ToString());
		}
	}

	private void SendMessage()
	{

		try
		{


			udpClient = new UdpClient(serverPort);

			udpClient.Connect(IPAddress.Parse(serverIP), serverPort);

			IPEndPoint point = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);

			byte[] message = Encoding.UTF8.GetBytes("This is a debug message");
			udpClient.Send(message, message.Length);


			udpClient.Close();


		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket Exception: " + socketException);
		}

		
	}


    private void OnDisable()
    {
		//Destroy the thread aftewards

		if(udpSocketThread!=null)
			udpSocketThread.Abort();
    }

	void RestartServer()
    {
		tcpListener.Stop();

		if (udpSocketThread != null)
			udpSocketThread.Abort();

		udpSocketThread = new Thread(new ThreadStart(RecieveIncomingMessages));
		udpSocketThread.IsBackground = true;
		udpSocketThread.Start();
	}

}