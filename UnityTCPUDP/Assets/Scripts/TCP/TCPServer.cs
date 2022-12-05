using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCPServer : MonoBehaviour
{
	
	private TcpListener tcpListener;

	private Thread tcpListenerThread;
	
	private TcpClient connectedTcpClient;


	[SerializeField] private string serverIP = "127.0.0.1";

	[SerializeField] private int serverPort = 8052;


	public char restartServer;

	[TextArea]
	[Tooltip("Doesn't do anything. Just shows the current status in inspector")]
	[SerializeField] private string currentStatus = "Status will be shown here.";

	private string connectedTo = "Connected To: Null";

	// Use this for initialization
	void Start()
	{
		// Start TcpServer background thread
		
		if(tcpListenerThread == null)
			tcpListenerThread = new Thread(new ThreadStart(RecieveIncomingMessages));
		tcpListenerThread.IsBackground = true;
		tcpListenerThread.Start();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			SendMessage();
		}
		else if (Input.GetKeyDown(KeyCode.N))
        {
			SpawnCube();
        }
		else if (Input.GetKeyDown(KeyCode.R))
        {
			RestartServer();
        }

		currentStatus = connectedTo;
	}

	private void RecieveIncomingMessages()
	{
		try
		{
			// Create listener on specified port ip			
			tcpListener = new TcpListener(IPAddress.Parse(serverIP), serverPort);
			tcpListener.Start();

			Debug.Log("Server has been started and listening on: " + serverIP + ":" + serverPort);
			Byte[] bytes = new Byte[1024];
			while (true)
			{
				using (connectedTcpClient = tcpListener.AcceptTcpClient())
				{
					connectedTo = "Connected to: " + connectedTcpClient.Client.RemoteEndPoint + " and local end point is: " + connectedTcpClient.Client.LocalEndPoint;
					// Get a stream object for reading 					
					using (NetworkStream stream = connectedTcpClient.GetStream())
					{
						int length;
						// Read incomming stream into byte arrary 						
						while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
						{
							var incommingData = new byte[length];
							Array.Copy(bytes, 0, incommingData, 0, length);
							// Convert byte array to string message 							
							string clientMessage = Encoding.ASCII.GetString(incommingData);
							Debug.Log("Client said: " + clientMessage);
						}
					}
				}
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("SocketException " + socketException.ToString());
		}
	}

	private void SendMessage()
	{
		if (connectedTcpClient == null)
		{
			return;
		}

		try
		{
			// Get a stream object for writing. 			
			NetworkStream stream = connectedTcpClient.GetStream();
			if (stream.CanWrite)
			{
				string serverMessage = "Sphe is a message from your server.";
				// Convert string message to byte array.                 
				byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage);
				// Write byte array to socketConnection stream.               
				stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
				Debug.Log("Message sent to Unreal, with the length of " + serverMessageAsByteArray.Length);
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket Exception: " + socketException);
		}
	}

	private void SpawnCube()
	{
		if (connectedTcpClient == null)
		{
			return;
		}

		try
		{
			// Get a stream object for writing. 			
			NetworkStream stream = connectedTcpClient.GetStream();
			if (stream.CanWrite)
			{
				string serverMessage = "Cube is a message from your server.";
				// Convert string message to byte array.                 
				byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage);
				// Write byte array to socketConnection stream.               
				stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
				Debug.Log("Message sent to Unreal, with the length of " + serverMessageAsByteArray.Length);
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
		}
	}

    private void OnDisable()
    {
		//Destroy the thread aftewards

		if(tcpListenerThread!=null)
			tcpListenerThread.Abort();
    }

	void RestartServer()
    {
		tcpListener.Stop();

		if (tcpListenerThread != null)
			tcpListenerThread.Abort();

		tcpListenerThread = new Thread(new ThreadStart(RecieveIncomingMessages));
		tcpListenerThread.IsBackground = true;
		tcpListenerThread.Start();
	}
}