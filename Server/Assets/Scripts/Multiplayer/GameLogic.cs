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

    [SerializeField] private int maxPlayersInRoom;
    public int MaxPlayersInRoom => maxPlayersInRoom;

    [SerializeField] private int minPlayersInRoom;
    public int MinPlayersInRoom => minPlayersInRoom;

    [SerializeField] private bool canStartBeforeRoomIsFull;
    public bool CanStartBeforeRoomIsFull => canStartBeforeRoomIsFull;

    
    
    
    #region Unity Events

    private void Awake()
    {
        Singleton = this;
    }

    #endregion
    

    

}
