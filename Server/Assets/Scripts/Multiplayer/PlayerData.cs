using RiptideNetworking;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData
{

    public ushort Id { get; private set; }
    public string username { get; private set; }

    public string roomId { get; private set; }
    
    public int side { get; private set; }


    #region Static Methods

    public static PlayerData FactoryPlayerData(ushort _Id, string _username ,string _roomId, int _side )
    {
        PlayerData p = new PlayerData();
        p.Id = _Id;
        p.username = _username;
        p.roomId = _roomId;
        p.side = _side;

        return p;
    }
    
    public Message AddData(Message message)
    {
        message.AddUShort(Id);
        message.AddString(username);
        message.AddInt(side);
        return message;
    }
    
    
     
    
    
    #endregion



   
    
}