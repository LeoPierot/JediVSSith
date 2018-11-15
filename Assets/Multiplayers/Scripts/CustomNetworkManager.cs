using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class CustomNetworkManager : NetworkManager 
{
    public CustomNetworkDiscovery netDiscovery;

    private bool isHost = false;
    private int numberOfClient = 0;

    void OnDestroy() {
        StopNetworking();
    }

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

    //Start and stop networking
    public void StartNetworking()
    {
        if(IsClientConnected()){
            return;
        }
        networkAddress = LocalIPAddress();
        
        netDiscovery.InitializeNetworkDiscovery();
    }
    public void StopNetworking()
    {
        if(!IsClientConnected()){
            return;
        }

        if(isHost){
            print("Stop host");
            foreach (NetworkConnection connection in NetworkServer.connections)
            {
                if(connection != null){
                    connection.Disconnect();
                }
            }
            isHost = false;
            numberOfClient = 0;
        }
        print("Stop client");
        StopClient();

        networkAddress = LocalIPAddress();
    }

    // Host and client
    public void StartAsHost()
    {
        isHost = true;
        numberOfClient = 1;
        StartHost();
    }

    public void StartAsClient(string fromAddress)
    {
        networkAddress = fromAddress;
        isHost = false;
        StartClient();
    }

    //Connnect and disconnect client
	public override void OnServerConnect(NetworkConnection conn)
	{
        base.OnServerConnect(conn);
        numberOfClient++;

        if(numberOfClient == matchSize){
            netDiscovery.StopBroadcast();
        }
	}

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        numberOfClient--;

        if(numberOfClient < matchSize){
            netDiscovery.StartAsServer();
        }
    }

}