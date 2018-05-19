using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private bool _isTowerSelected;

    private GameObject _gameObject;

    private bool _canBePlaced = false;

    private GameObject _hitByTowerGameObject;    

    public int LifeCount = 4;

    public int Money = 500;

    public float WaveTimer;

    public float WaveTimerStartTime = 40;

    public float WaveTimerStep = 20f;    

    public GameObject TowerPrefab;

    public LayerMask hitLayers;

    public GameState State;

    public delegate void NextWaveDelegate();

    public event NextWaveDelegate NextWaveEvent;

    public static GameManager Instance { get; set; }

    void Awake()
    {
        if (Instance == null || Instance != this)
            Instance = this;
    }

	// Use this for initialization
	void Start ()
	{
        State = GameState.Start;
        WaveTimer = WaveTimerStartTime;
	}
	
	// Update is called once per frame
    void Update()
    {
        if (_isTowerSelected)
        {
            FollowCursor();
            if (Input.GetMouseButtonUp(0))
            {
                PlaceTower();
            }
        }

        if (State == GameState.Wave)
            WaveTimer -= Time.deltaTime;

        if (WaveTimer <= 0)
        {
            SpawnNextWave();
        }
    }

    public void SpawnNextWave()
    {
        WaveTimer = WaveTimerStartTime + WaveTimerStep;
        WaveTimerStartTime = WaveTimer;
        if (NextWaveEvent != null) NextWaveEvent.Invoke();
        State = GameState.Wave;
    }

    private void PlaceTower()
    {
        if (!_canBePlaced) return;
        var tower = _gameObject.GetComponent<Tower>();
        if (Money - tower.TowerCost < 0) return;        
        _gameObject.transform.position = _hitByTowerGameObject.transform.position;
        _hitByTowerGameObject.tag = "Tower";
        Money -= tower.TowerCost;
        tower.RestoreColors();
        tower.State = TowerState.Patroling;
        _isTowerSelected = false;
        _gameObject = null;
        _hitByTowerGameObject = null;
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SelectTower()
    {
        _isTowerSelected = true;
        _gameObject = Instantiate(TowerPrefab);
    }

    public void FollowCursor()
    {
        var mouse = Input.mousePosition;
        var castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (!Physics.Raycast(castPoint, out hit, Mathf.Infinity, hitLayers)) return;
        Debug.Log("Hit by raycast - " + hit.transform.name);
        var tower = _gameObject.GetComponent<Tower>();
        _gameObject.transform.position = hit.point;
        _hitByTowerGameObject = hit.transform.gameObject;
        if (hit.transform.tag == "TowerPlace")
        {
            tower.Colorize(Color.green);
            _canBePlaced = true;
        }
        else
        {
            tower.Colorize(Color.red);
            _canBePlaced = false;
        }
    }
}

public enum GameState
{
    Start,
    Wave    
}