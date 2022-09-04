
using RiptideNetworking;
using RiptideNetworking.Utils;
#if !UNITY_EDITOR
using System;
#endif
using UnityEngine;

namespace Riptide.Demos.DedicatedServer
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
        
        [SerializeField] private ushort port;
        [SerializeField] private ushort maxClientCount;
        public Server Server { get; private set; }

        #region Unity Events

        private void Awake()
        {
            Singleton = this;
        }

        private void Start()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;

#if UNITY_EDITOR
            RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
#else
            Console.Title = "Server";
            Console.Clear();
            Application.SetStackTraceLogType(UnityEngine.LogType.Log, StackTraceLogType.None);
            RiptideLogger.Initialize(Debug.Log, true);
#endif

            Server = new Server();
            Server.ClientConnected += NewPlayerConnected;
            Server.ClientDisconnected += PlayerLeft;

            Server.Start(port, maxClientCount);
        }

        private void FixedUpdate()
        {
            if (Server != null)
            { Server.Tick();
                
            }
           
        }

        private void OnApplicationQuit()
        {
            if (Server == null)
            {
                return; 
            }
            Server.Stop();

            Server.ClientConnected -= NewPlayerConnected;
            Server.ClientDisconnected -= PlayerLeft;
        }


        #endregion

        #region Events Handlers
        
        private void NewPlayerConnected(object sender, ServerClientConnectedEventArgs e)
        {
            //Event for getting new connection
          
        }

        private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
        {
            RoomInstance.RemovePlayer(e.Id);
        }
        #endregion

   
    }
}