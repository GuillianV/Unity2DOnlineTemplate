using System;
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

    public int roomCounter = 0;
    
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
        if (!playerInstance.isFull)
        {
            playerInstance.CreateServerPlayerData(fromClientId, username);

        }
        else
        {
            playerInstance.SendPlayerError(fromClientId,String.Format("{0} is full.",roomId));
        }


        if (GameLogic.CanStartBeforeRoomIsFull && playerInstance.clients.Count >= GameLogic.MinPlayersInRoom)
        {

            playerInstance.isFull = true;
            playerInstance.roomStarted = true;
            NetworkManager.Singleton.Server.SendToAll(Message.Create(MessageSendMode.reliable, (ushort)ServerToClientId.gameStarted));
            
        }

    }
    
    
    #endregion

    #region Static methods

    public static bool RoomExist(string roomId)
    {
        return  Singleton.rooms.ContainsKey(roomId);
    }

    public static string CreateRoom(string roomId)
    {
        if (!RoomExist(roomId))
        {
            PlayerInstance playerInstance = Singleton.gameObject.AddComponent<PlayerInstance>();
            playerInstance.StartPlayerInstancee(roomId,GetRoomCounter());
            Singleton.rooms.Add(roomId,playerInstance);
        }

        return roomId;
    }

    public static void DestroyRoom(string _roomId)
    {
        RoomInstance roomInstance = Singleton;
        PlayerInstance playerInstance = roomInstance.rooms[_roomId];
        playerInstance.DestroyPlayerInstance();
        Debug.Log("Room "+_roomId+" has been destroyed");
    }

    public static void RemovePlayer(ushort _id)
    {
        RoomInstance roomInstance = Singleton;

        foreach (PlayerInstance playerInstance in roomInstance.rooms.Values)
        {
            playerInstance.DestroyPlayer(_id);
            
            if (playerInstance.clients.Count == 0 && playerInstance.clientsEntities.Count == 0)
            {
                DestroyRoom(playerInstance.roomId);
                
               
            }
        }
    }

    public static int GetRoomCounter()
    {
        RoomInstance roomInstance = Singleton;
        roomInstance.roomCounter += 10;
        return Singleton.roomCounter;
    }
    
    

    #endregion
    
  
}
