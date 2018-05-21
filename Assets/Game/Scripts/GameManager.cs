using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private bool _isTowerSelected;

    private GameObject _gameObject;

    private bool _canBePlaced = false;

    private GameObject _hitByTowerGameObject;

    private AudioSource _backgroundAudioSource;

    public int LifeCount = 4;

    public int Money = 500;

    public int EnemyCount = 0;

    public int WaveNumber = 1;

    public float WaveTimer;

    public float WaveTimerStartTime = 40;

    public float WaveTimerStep = 20f;

    public List<GameObject> TowerPrefabs;
    
    public LayerMask hitLayers;

    public GameState State;

    public delegate void NextWaveDelegate();

    public delegate void GameOverDelegate();

    public event NextWaveDelegate NextWaveEvent;

    public event GameOverDelegate GameOverEvent;

    public event GameOverDelegate VictoryEvent;

    public static GameManager Instance { get; set; }

    void Awake()
    {
        if (Instance == null || Instance != this)
            Instance = this;
    }

	// Use this for initialization
	void Start ()
	{
        _backgroundAudioSource = transform.GetChild(0).GetComponent<AudioSource>();
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
            else if (_gameObject != null && Input.GetMouseButtonUp(1))
            {
                Destroy(_gameObject);
                _gameObject = null;
                _isTowerSelected = false;
            }
        }

        if (State == GameState.Wave)
            WaveTimer -= Time.deltaTime;

        if (WaveTimer <= 0)
        {
            SpawnNextWave();
        }

        if (LifeCount <= 0)
        {
            GameOver();
        }

        if (WaveNumber >= 10 && EnemyCount == 0)
        {
            Victory();
        }
    }

    private void Victory()
    {
        Time.timeScale = 0;
        if (VictoryEvent != null) VictoryEvent.Invoke();
        enabled = false;
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        if (GameOverEvent != null) GameOverEvent.Invoke();
        enabled = false;
    }

    public void SpawnNextWave()
    {
        if (WaveNumber >= 10) return;
        if (!_backgroundAudioSource.isPlaying)
            _backgroundAudioSource.Play();
        WaveTimer = WaveTimerStartTime + WaveTimerStep;
        WaveTimerStartTime = WaveTimer;
        WaveNumber++;        
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
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SelectTower(int towerIndex)
    {
        var tower = TowerPrefabs[towerIndex].GetComponent<Tower>();
        if (Money - tower.TowerCost < 0) return;
        _isTowerSelected = true;
        _gameObject = Instantiate(TowerPrefabs[towerIndex]);
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