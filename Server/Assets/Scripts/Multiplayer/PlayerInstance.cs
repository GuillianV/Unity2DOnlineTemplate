using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Riptide.Demos.DedicatedClient;
using Riptide.Demos.DedicatedServer;
using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInstance : MonoBehaviour
{
  
    
    public Dictionary<ushort, PlayerData> clients = new Dictionary<ushort, PlayerData>();
    public Dictionary<ushort, GameObject> clientsEntities = new Dictionary<ushort, GameObject>();


    public string roomId;

    public PlayerInstance(string _roomId)
    {
        this.roomId = _roomId;
    }
  


    #region Static Methods

    
    public static bool DisconnectAll()
    {
        try
        {
            PlayerInstance instance = Singleton;
            foreach (ushort id in instance.clients.Keys)
            {
                //DestroyEntity
                if (instance.clientsEntities.ContainsKey(id))
                {
                    Destroy(instance.clientsEntities[id]);
                    instance.clientsEntities.Remove(id);
                }
                 
            
                //DestroyData
                if (instance.clients.ContainsKey(id))
                    instance.clients.Remove(id);
          
            }
        
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return false;
        }
        
        
    }
    
    public static bool DisconnectOne(ushort id)
    {

        try
        {
            PlayerInstance instance = Singleton;
        
            if (instance.clients.ContainsKey(id) && instance.clientsEntities.ContainsKey(id))
            {
            
                if (instance.clients[id].IsLocal)
                {
                    DisconnectAll();
                }
                else
                {
            
                    //DestroyEntity
                    if (instance.clientsEntities.ContainsKey(id))
                    {
                        Destroy(instance.clientsEntities[id]);
                        instance.clientsEntities.Remove(id);
                    }
                 
            
                    //DestroyData
                    if (instance.clients.ContainsKey(id))
                        instance.clients.Remove(id);

                }
                
            
            }

            
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return false;
        }
        

    }


    
    //Get Data of client Connected
    public static void ConnectOne(ushort id, string username, int sideValue)
    {
        
        PlayerData player;
       
        if (id == NetworkManager.Singleton.Client.Id)
        {
            player = PlayerData.FactoryPlayerData(id,true,username);
            GameLogic.Singleton.SetSideValue(sideValue);
        }
        else
        {
            player = PlayerData.FactoryPlayerData(id,true,username);

        }
        Singleton.clients.Add(id,player);
    }

    
    //Instanciate Client
    public static void CreateOne(ushort id)
    {
        PlayerInstance instance = Singleton;
        if (instance.clients.ContainsKey(id))
        {
            PlayerData playerData = instance.clients[id];
            GameObject go = Instantiate(GameLogic.Singleton.PlayerPrefab, Vector3.zero, Quaternion.identity);
            
            switch (GameLogic.Singleton.SideValue)
            {
                case 1:
                    //Premier point de vue
                    break;
                
                case 2:
                    //Second point de vue
                    break;
            }
            
            instance.clientsEntities.Add(id,go);
            
        }
        
    }

    //Instancie tous les clients
    public static void CreateAll()
    {
        foreach (ushort id in Singleton.clients.Keys)
        {
            CreateOne(id);
        }
        
    }
    
    
    
    
    

    #endregion
    
      #region Local Methods

    public void ConnectClientToPlayers()
    {
        NetworkManager.Singleton.Server.SendToAll(AddSpawnData(Message.Create(MessageSendMode.reliable, (ushort)ServerToClientId.playerConnected)));
    }

    public void ConnectPlayersToClient(ushort toClientId)
    {
        NetworkManager.Singleton.Server.Send(AddSpawnData(Message.Create(MessageSendMode.reliable, (ushort)ServerToClientId.playerConnected)), toClientId);
    }
    
    
    public void CreateServerPlayerData(ushort _id, string _username)
    {
        //Get Player Username
        string username = !string.IsNullOrEmpty(_username) ? _username : "Guest";
        
        
        PlayerData playerData = PlayerData.FactoryPlayerData(_id,username,roomId,clients.Count);
        //Create Player
        GameObject playerGo = Instantiate(GameLogic.Singleton.PlayerPrefab, Vector3.zero, Quaternion.identity);
        playerGo.name =
            $"Player {username}";


        //Add player to list
        clients.Add(_id, playerData);
        clientsEntities.Add(_id,playerGo);
    }
 

    #endregion
    
  
    
    #region Static Methods

  

 


 
    
    

    #endregion
 
    
   
    
    

}

