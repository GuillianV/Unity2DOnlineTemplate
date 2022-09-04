using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    #region Instance

    private static GameLogic _singleton;
    public static GameLogic Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(GameLogic)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    #endregion

    [SerializeField] private GameObject playerPrefab;
    public GameObject PlayerPrefab => playerPrefab;

    
    [Header("Rooms options")]
    [SerializeField]
    public int maxPlayersInRoom = 4;
    [SerializeField]
    public int minPlayersInRoom = 1;
    [SerializeField]
    public bool canStartBeforeRoomIsFull = true;
    
    
    [SerializeField]
    public static int MaxPlayersInRoom;
    [SerializeField]
    public static int MinPlayersInRoom;
    [SerializeField]
    public static bool CanStartBeforeRoomIsFull;
    
    
    #region Unity Events

    private void Awake()
    {
        Singleton = this;
        MaxPlayersInRoom = maxPlayersInRoom;
        MinPlayersInRoom = minPlayersInRoom;
        CanStartBeforeRoomIsFull = canStartBeforeRoomIsFull;
    }

    #endregion
    

    

}
