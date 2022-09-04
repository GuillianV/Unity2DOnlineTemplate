using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Riptide.Demos.DedicatedClient;
using RiptideNetworking;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInstance : MonoBehaviour
{
    
    #region Instance

    private static PlayerInstance _singleton;
    public static PlayerInstance Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(PlayerInstance)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    #endregion
    
    
    public Dictionary<ushort, PlayerData> clients = new Dictionary<ushort, PlayerData>();
    public Dictionary<ushort, GameObject> clientsEntities = new Dictionary<ushort, GameObject>();

    private void Awake()
    {
            
        Singleton = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }


        
    #region MessageHandlers

    
    //Start Game
    [MessageHandler((ushort)ServerToClientId.gameStarted)]
    private static void GameStarted(Message message)
    {
        Debug.Log("<color=green>Game Started !</color>");
        //Change scene or whatever
        
        CreateAll();
        
    }
    
    
    //Start Game
    [MessageHandler((ushort)ServerToClientId.playerError)]
    private static void PlayerError(Message message)
    {
        Debug.Log("Error from server : <color=red>"+message.GetString()+"</color>");
        //Return Menu
        
    }
    
     
    //Get clients into player
    [MessageHandler((ushort)ServerToClientId.playerConnected)]
    private static void PlayerConnected(Message message)
    {
        ConnectOne(message.GetUShort(), message.GetString(), message.GetInt());
    }
    
    #endregion


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
            GameObject go = Instantiate(GameLogic.Singleton.PlayerPrefab, Vector3.zero, Quaternion.identity, GameLogic.Singleton.Map.transform);
            go.name = playerData.username;
            
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
    
 
    
   
    
    

}

