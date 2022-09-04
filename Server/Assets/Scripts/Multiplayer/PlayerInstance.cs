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
    public bool roomStarted;
    public bool isFull;
    public int roomCounter;

    public GameObject Map;
    

    private void Awake()
    {
        if (clients == null)
        {
            clients = new Dictionary<ushort, PlayerData>();
        }

        if (clientsEntities == null)
        {
            clientsEntities = new Dictionary<ushort, GameObject>();
        }
       
    }

    public void StartPlayerInstancee(string _roomId, int _roomCounter)
    {

        this.roomId = _roomId;
        this.roomCounter = _roomCounter;
        if (Map == null)
        {
            Map = Instantiate(GameLogic.Singleton.Map, new Vector3(0, 0, roomCounter), Quaternion.identity);
        }
    }
    

    #region Local Methods

    public void CreateServerPlayerData(ushort _id, string _username)
    {
        //Get Player Username
        string username = !string.IsNullOrEmpty(_username) ? _username : "Guest";
        
        
        PlayerData playerData = PlayerData.FactoryPlayerData(_id,username,roomId,clients.Count);
        //Create Player
        GameObject playerGo = Instantiate(GameLogic.Singleton.PlayerPrefab, Vector3.zero, Quaternion.identity,Map.transform);
        playerGo.name =
            $"Player {username}";

        
        
        //Pour le joueur crée, ajoute tous les joueurs
        foreach (ushort othersIds in clients.Keys)
        {
            ConnectPlayersToClient(_id,othersIds);
        }

        //Add player to list
        clients.Add(_id, playerData);
        clientsEntities.Add(_id,playerGo);
        
        //Pour tous les joueurs existants, ajoute le joueur crée,
        ConnectClientToPlayers(_id);

    }
    
    public void ConnectClientToPlayers(ushort playerId)
    {
        PlayerData playerData = clients[playerId];
        NetworkManager.Singleton.Server.SendToAll(playerData.AddData(Message.Create(MessageSendMode.reliable, (ushort)ServerToClientId.playerConnected)));
    }

    public void ConnectPlayersToClient(ushort playerId, ushort toClientId)
    {
        PlayerData playerData = clients[playerId];
        NetworkManager.Singleton.Server.Send(playerData.AddData(Message.Create(MessageSendMode.reliable, (ushort)ServerToClientId.playerConnected)), toClientId);
    }

    public void SendPlayerError(ushort playerId ,string message)
    {
        Message errorMessage = Message.Create(MessageSendMode.reliable, (ushort)ServerToClientId.playerError);
        errorMessage.AddString(message);
        NetworkManager.Singleton.Server.Send(errorMessage,playerId);

    }

    public void DestroyPlayerInstance()
    {
     
        foreach (GameObject clientsEntitiesValue in clientsEntities.Values)
        {
            Destroy(clientsEntitiesValue);
        }
        clients.Clear();
        clientsEntities.Clear();
        Destroy(this);
    }

    
    public void DestroyPlayer(ushort _playerId)
    {
     
        foreach (KeyValuePair<ushort,GameObject> clientsEntities in clientsEntities)
        {
            if (clientsEntities.Key == _playerId)
            {
                Destroy(clientsEntities.Value);
                
            }
            
           
        }
        clients.Remove(_playerId);
        clientsEntities.Remove(_playerId);
    }

    
    #endregion

 
    

}

