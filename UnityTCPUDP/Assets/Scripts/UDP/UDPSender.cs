using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


public class UDPSender : MonoBehaviour
{
	private UdpClient socket;


	[SerializeField] private string serverIP = "127.0.0.1";

	[SerializeField] private int serverPort = 8052;

    [SerializeField] private string stringToBeSent = "Test String";

	[SerializeField] KeyCode keyCodeToSend = KeyCode.K;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyCodeToSend))
        {
			SendMessage();
        }
    }

	private void SendMessage()
	{
		try
		{
            socket = new UdpClient(serverPort);

            socket.Connect(IPAddress.Parse(serverIP), serverPort);

            IPEndPoint point = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);

            byte[] message = Encoding.UTF8.GetBytes(stringToBeSent);
            socket.Send(message, message.Length);


            socket.Close();


        }
		catch (SocketException socketException)
		{
			Debug.Log("Socket Exception: " + socketException);
		}

	}
}





