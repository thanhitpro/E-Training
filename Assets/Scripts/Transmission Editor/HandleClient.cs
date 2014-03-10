using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class HandleClient : MonoBehaviour  {
	private bool sendCommand = false;
	public bool SendCommand {
		get {
			return sendCommand;
		}
		set {
			sendCommand = value;
		}
	}
	public string componentName ="";
	public string ComponentName {
		get {
			return componentName;
		}
	}

	private bool firstAssign = true;
	private	string dataFromClient ;

	private string dataToClient = "";

	public string DataToClient {
		get {
			return dataToClient;
		}
		set {
			dataToClient = value;
		}
	}

	private NetworkStream networkStream;
	TcpClient clientSocket;
	string clNo;
	bool isStopThread = false;
	private Thread ctThread;
	public void startClient(TcpClient inClientSocket, string clineNo)
	{
		this.clientSocket = inClientSocket;
		this.clNo = clineNo;
		ctThread = new Thread(doChat);
		ctThread.Start();
	}
	~HandleClient()
	{
		//ctThread.Join();
	}

	void Update ()
    {
		sendData();
	}

	public void closeThread() {
		isStopThread = true;
		networkStream.Close();
		this.clientSocket.Close();
	}

	private void sendData() {
		//Debug.Log(sendCommand);
		//Debug.Log(dataToClient);
		if (sendCommand) {
			Debug.Log(">>0 Send command");
			byte[] sendBytes = null;
			string serverResponse = null;
			serverResponse = dataToClient;
			sendBytes = Encoding.ASCII.GetBytes(serverResponse);
			networkStream.Write(sendBytes, 0, sendBytes.Length);
			networkStream.Flush();
			Debug.Log(" >>1 " + serverResponse);
			sendCommand = false;
		}
	}
	
	private void doChat()
	{
		byte[] bytesFrom = new byte[2048];
		dataFromClient = null;
		while ((true))
		{
			try
			{
				networkStream = clientSocket.GetStream();
				bytesFrom = new byte[(int)clientSocket.ReceiveBufferSize];
				networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
				dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
				dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
				Debug.Log(" >>3 " + "From client - " + clNo + " " + dataFromClient);

				if(firstAssign)
				{
					componentName = dataFromClient;
					Debug.Log("TEN CUA COMPONENT LA: " + componentName);
				
				}
				else{
					if(dataFromClient != componentName)
					{
						componentName = dataFromClient;
						
						//EngineComponent ec = new EngineComponent("",componentName, false);
						//ec.setRedSilhouette();
					}
				}			

			}
			catch (System.Exception ex)
			{
				Debug.Log(" >>2 " + ex.ToString());
				if (isStopThread) {
					ctThread.Abort();
					break;
				}
			}				
		}
	}
}
