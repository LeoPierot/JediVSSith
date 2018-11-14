using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Threading;
using System.Net;
using System.Net.Sockets;

public class Network : NetworkDiscovery {
 
	public bool host = false;
	public bool hostFound = false;

	public string LocalIPAddress()
	{
		IPHostEntry host;
		string localIP = "";
		host = Dns.GetHostEntry(Dns.GetHostName());
		foreach (IPAddress ip in host.AddressList)
		{
			if (ip.AddressFamily == AddressFamily.InterNetwork)
			{
				localIP = ip.ToString();
				break;
			}
		}
		return localIP;
	}

	void Start(){
		NetworkManager.singleton.networkAddress = LocalIPAddress();
	}

	public override void OnReceivedBroadcast(string fromAddress, string data){
		print("Received a broadcast message from host");
		InitializeNetworkClient(fromAddress);
    }
	
	public void InitializeGlobalNetwork(){
		print("Start coroutine waitBroadcast");
		if(!Initialize()){
			Debug.Log("Problem, can't initialize the network discovery !");
			return;
		}
		StartAsClient();
		StartCoroutine(WaitBroadcast());
	}

	IEnumerator WaitBroadcast(){
        print("Start looking for server...");
        yield return new WaitForSeconds(5);
		if(!hostFound){
			print("No client founded, start as server.");
			InitializeNetworkHost();
		}
    }

	public void InitializeNetworkHost(){
		StopBroadcast();
		Thread.Sleep(200);
		NetworkManager.singleton.StartHost();
		StartAsServer();
		host = true;
		print("Start the server !");
	}

	public void InitializeNetworkClient(string fromAddress){
		if(!hostFound){
			print("address : "+fromAddress);
			NetworkManager.singleton.networkAddress = fromAddress;
        	NetworkManager.singleton.StartClient();
			hostFound = true;
			print("Start the client !");
		}
	}

	public void OnDestroy(){
		if(host){
			print("Host stopped");
			NetworkManager.singleton.StopHost();
		}else{
			print("Client stopped");
			NetworkManager.singleton.StopClient();
		}
		StopBroadcast();
	}

}
