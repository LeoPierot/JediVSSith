using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Threading;

public class CustomNetworkDiscovery : NetworkDiscovery {
 
	public bool hostFound = false;

	public CustomNetworkManager netManager;

	public override void OnReceivedBroadcast(string fromAddress, string data){
		print("Host found");
		InitializeNetworkClient(fromAddress);
    }
	
	public void InitializeNetworkDiscovery(){
		//print("Start coroutine waitBroadcast");
		if(!Initialize()){
			Debug.LogError("Can't initialize the network discovery !");
			return;
		}
		StartAsClient();
		StartCoroutine(WaitBroadcast());
	}

	IEnumerator WaitBroadcast(){
        print("Start looking for server...");
        yield return new WaitForSeconds(2);
		if(!hostFound){
			print("No host founded, start as server.");
			InitializeNetworkHost();
		}
    }

	public void InitializeNetworkHost(){
		StopBroadcast();
		Thread.Sleep(200);
		netManager.StartAsHost();
		StartAsServer();
		print("Server started on " + NetworkManager.singleton.networkAddress + " !");
	}

	public void InitializeNetworkClient(string fromAddress){
		if(!hostFound){
			StopBroadcast();
        	netManager.StartAsClient(fromAddress);
			hostFound = true;
			print("Client started on " + fromAddress + " !");
		}
	}
}
