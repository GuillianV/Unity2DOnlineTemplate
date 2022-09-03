using RiptideNetworking;
using System.Collections.Generic;
using Riptide.Demos.DedicatedClient;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData
{

    public ushort Id { get; private set; }
    public bool IsLocal { get; private set; }
    public string username { get; private set; }



    #region Static Methods

    public static PlayerData FactoryPlayerData(ushort _Id, bool _IsLocal, string _username )
    {
        PlayerData p = new PlayerData();
        p.Id = _Id;
        p.username = _username;
        p.IsLocal = _IsLocal;

        return p;
    }
    
     
    
    
    #endregion



   
    
}