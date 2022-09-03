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
                Debug.Log($"{nameof(GameLogic)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }
        
    #endregion
    
    public GameObject PlayerPrefab => playerPrefab;
    public int SideValue => sideValue;

    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefab;

    [Header("Parameters")] 
    [SerializeField] private int sideValue;
    
    #region Unity Events

    private void Awake()
    {
        Singleton = this;
    }


    public void SetSideValue(int _sideValue)
    {

        this.sideValue = _sideValue;

    }

    #endregion

  
}