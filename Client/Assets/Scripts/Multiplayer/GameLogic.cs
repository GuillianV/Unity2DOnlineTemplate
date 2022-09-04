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
    public GameObject Map => map;
    public int SideValue => sideValue;

    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject map;
    
    [Header("Parameters")] 
     private int sideValue;
    
    #region Unity Events

    private void Awake()
    {
        Singleton = this;
        if (Map == null)
        {
            this.map = GameObject.FindWithTag("Map");
        }
    }


    public void SetSideValue(int _sideValue)
    {

        this.sideValue = _sideValue;

    }

    #endregion

  
}