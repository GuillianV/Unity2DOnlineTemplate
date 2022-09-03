using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
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
  

    
      #region Local Methods

    public void ConnectClientToPlayers(ushort playerId)
    {
     //   NetworkManager.Singleton.Server.SendToAll(AddSpawnData(Message.Create(MessageSendMode.reliable, (ushort)ServerToClientId.playerConnected)));
    }

    public void ConnectPlayersToClient(ushort playerId, ushort toClientId)
    {
       // NetworkManager.Singleton.Server.Send(AddSpawnData(Message.Create(MessageSendMode.reliable, (ushort)ServerToClientId.playerConnected)), toClientId);
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

