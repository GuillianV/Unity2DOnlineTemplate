using System.Collections;
using System.Collections.Generic;
using Riptide.Demos.DedicatedServer;
using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;

public class RoomInstance : MonoBehaviour
{
    #region Instance

    private static RoomInstance _singleton;
    public static RoomInstance Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(RoomInstance)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }
    
    
    public Dictionary<string, PlayerInstance> rooms = new Dictionary<string, PlayerInstance>();
    

    #endregion
    
    
    private void Awake()
    {
            
        Singleton = this;
        DontDestroyOnLoad(gameObject);
        RiptideLogger.Initialize(Debug.Log,Debug.Log,Debug.LogWarning,Debug.LogError,false);

    }

    
    
          
    #region MessageHandlers

    
    [MessageHandler((ushort) ClientToServerId.playerConnect)]
    public static void PlayerConnect(ushort fromClientId, Message message)
    {
        RoomInstance roomInstance = Singleton;
        
        string username = message.GetString();
        string roomId = message.GetString();

        if (!string.IsNullOrEmpty(roomId))
        {
            //Check que la room existe
            if (!RoomExist(roomId))
            {
                CreateRoom(roomId);

            }
        }
        else
        {
            CreateRoom(roomId);
        }

        PlayerInstance playerInstance = roomInstance.rooms[roomId];
        playerInstance.CreateServerPlayerData(fromClientId, username);
        
        
        //Pour le joueur, ajoute tous les joueurs
        foreach (PlayerData otherPlayerData in playerInstance.clients.Values)
        {
            playerInstance.C
            otherPlayerData.ConnectPlayersToClient(id);
        }
           

        //Pour tous les joueurs, ajoute un joueur,
      
        player.ConnectClientToPlayers();
    
        
        //Si le serveur est plein, on lance la partie
        if (PlayersList.Count == NetworkManager.Singleton.Server.MaxClientCount)
        {
            
            NetworkManager.Singleton.Server.SendToAll(Message.Create(MessageSendMode.reliable, (ushort)ServerToClientId.startGame));

        }
        
    }
    
    
    #endregion
    
    
    public static bool RoomExist(string roomId)
    {
       return  Singleton.rooms.ContainsKey(roomId);
    }

    public static string CreateRoom(string roomId)
    {
        if (!RoomExist(roomId))
        {
            Singleton.rooms.Add(roomId,new PlayerInstance(roomId));
        }

        return roomId;
    }
    
    
}
