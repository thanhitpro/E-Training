using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ETraining;
 
public class Server : MonoBehaviour { 
	int counter = 0;
	private Rect grp;
	private Rect box;
	TcpListener serverSocket;// = new TcpListener(IPAddress.Loopback,9999);
	TcpClient clientSocket;// = default(TcpClient);	
	HandleClient client ;//= new HandleClient();

	private string currentComponentInEditor = "";
	private EngineComponent prevComponent = null;
	private bool firstAssign = true;
	private TransmissionEditorController script;

    void Awake ()
    {
		//serverSocket.Stop();
		serverSocket = new TcpListener(IPAddress.Loopback,9999);
		serverSocket.Start();
		clientSocket = default(TcpClient);
		Debug.Log(" >> " + "Server Started");
		grp = new Rect(Screen.width/3f,Screen.height/4f,Screen.width/2.5f,Screen.height/2.5f);
		box = new Rect(0f,0f,Screen.width/2.5f,Screen.height/2.4f);

		script = transform.GetComponent(typeof(TransmissionEditorController)) as TransmissionEditorController;
		client = transform.GetComponent(typeof(HandleClient)) as HandleClient;

    }
 
    void OnApplicationQuit ()
    {
		Cleanup();
    }
 
    void Cleanup ()
    {
		clientSocket.Close();
		serverSocket.Stop();
		client.closeThread();
		Debug.Log(" >> " + "exit");
    } 
	void OnDisable()
	{
		Cleanup();
	}
    ~Server ()
    {
		Cleanup();
    }
 
    void Update ()
    {
		if (counter == 0) {
			counter += 1;
			clientSocket = serverSocket.AcceptTcpClient();
			Debug.Log(" >> " + "Client No:" + counter.ToString() + " started!");
			client.startClient(clientSocket, counter.ToString());

		}

		if(script.DataToClient != "") {
			client.SendCommand = true;
			client.DataToClient = script.DataToClient;
		}
		if(firstAssign)
		{
			currentComponentInEditor = client.ComponentName;
			firstAssign = false;
			if(currentComponentInEditor != "")
			{
				if(!GameObject.Find(client.ComponentName)) 
				{
					prevComponent.reset();
					currentComponentInEditor = "";
					return;			
				}

				if(prevComponent != null) prevComponent.reset();
				if(GameObject.Find(script.DataToClient))
				{
					EngineComponent ecInUnity = new EngineComponent("",client.DataToClient, false);
					ecInUnity.reset();
					script.PrevHitted = "";
				}

				EngineComponent ec = new EngineComponent("",client.ComponentName, false);
				ec.enableBoxCollider(true);
				ec.reset();
				ec.setRedSilhouette();

				prevComponent = ec;
			}
		}
		else
		{
			if(currentComponentInEditor != client.ComponentName)
			{
				if(!GameObject.Find(client.ComponentName))
				{
					prevComponent.reset();
					currentComponentInEditor = "";
					return;
				}
				currentComponentInEditor = client.ComponentName;
				if(prevComponent != null) prevComponent.reset();

				if(GameObject.Find(script.DataToClient))
				{
					EngineComponent ecInUnity = new EngineComponent("",client.DataToClient, false);
					ecInUnity.reset();
					script.PrevHitted = "";
				}


				EngineComponent ec = new EngineComponent("",client.ComponentName, false);
				ec.enableBoxCollider(true);
				ec.reset();
				ec.setRedSilhouette();

				prevComponent = ec;
			}
		}
    }

	void OnGUI ()
	{
//		MessageData m = new MessageData();
//		GUI.BeginGroup(grp);
//		GUI.Box(box,"Send message");
//		GUI.Label(new Rect(80f,50f,100f,25f),"Message");
//		m.stringData = GUI.TextField(new Rect(180f,50f,100f,25f),m.stringData);
//		if(GUI.Button(new Rect(100f,150f,70f,25f),"Send"))
//		{
//			m.stringData="";
//		}
//		GUI.EndGroup();
	}
}