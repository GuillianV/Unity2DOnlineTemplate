
using System;
using System.Collections.Generic;
using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Riptide.Demos.DedicatedClient
{
    public enum ServerToClientId : ushort
    {
        playerConnected = 1,
        gameStarted = 2,
        playerError = 3,
    }
    public enum ClientToServerId : ushort
    {
        playerConnect = 1,
    }

    public class NetworkManager : MonoBehaviour
    {
        #region Instance

        private static NetworkManager _singleton;
        public static NetworkManager Singleton
        {
            get => _singleton;
            private set
            {
                if (_singleton == null)
                    _singleton = value;
                else if (_singleton != value)
                {
                    Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying object!");
                    Destroy(value);
                }
            }
        }

        #endregion
        
        [SerializeField] private string ip;
        [SerializeField] private ushort port;

      
        public Client Client { get; private set; }

        #region Unity Events

        private void Awake()
        {
            
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

            Client = new Client();
            Client.Connected += DidConnect;
            Client.ConnectionFailed += FailedToConnect;
            Client.ClientConnected += PlayerJoined;
            Client.ClientDisconnected += PlayerLeft;
            Client.Disconnected += DidDisconnect;

       
            Connect();
        }

        private void FixedUpdate()
        {
            Client.Tick();
        }

        private void OnApplicationQuit()
        {
            Client.Disconnect();

            Client.Connected -= DidConnect;
            Client.ConnectionFailed -= FailedToConnect;
            Client.ClientDisconnected -= PlayerLeft;
            Client.Disconnected -= DidDisconnect;
            Client.ClientConnected -= PlayerJoined;
        }

        #endregion
        
        #region Events Handlers

        
        public void Connect()
        {
            Client.Connect($"{ip}:{port}");
        }

        private void DidConnect(object sender, EventArgs e)
        {
            Debug.Log("Did Connect");
            //S'est connecté au serveur
            
            
            //On envoie les données (username..). On peut très bien le faire avec un bouton sur un canva
            Message message = Message.Create(MessageSendMode.reliable, (ushort) ClientToServerId.playerConnect);
            message.AddString("MyPlayer");
            message.AddString("room-01");
            Singleton.Client.Send(message);
            
        }

        private void FailedToConnect(object sender, EventArgs e)
        {
            PlayerInstance.DisconnectAll();
        }

        private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
        {
            PlayerInstance.DisconnectOne(e.Id);
        }

        private void PlayerJoined(object sender, ClientConnectedEventArgs e)
        {
            
        }
        
        private void DidDisconnect(object sender, EventArgs e)
        { 
            PlayerInstance.DisconnectAll();
            SceneManager.LoadScene(0);
        }
        
        #endregion
    }
}