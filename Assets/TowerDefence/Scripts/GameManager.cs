using System.Collections.Generic;
using TowerDefence.Data;
using TowerDefence.Enemies;
using TowerDefence.Towers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefence
{
    public class GameManager : MonoBehaviour
    {
        private bool _isTowerSelected;
        private TowerDummy _dummy;
        private bool _canBePlaced = false;
        private GameObject _hitByTowerGameObject;
        private AudioSource _backgroundAudioSource;
        
        public int LifeCount = 4;
        public int Money = 500;
        public int EnemyCount = 0;
        public int WaveIndex = -1;
        public float WaveTimer;
        public float WaveTimerStartTime = 40;
        public float WaveTimerStep = 20f;
        public List<TowerDummy> TowerDummyPrefabs;
        public List<Tower> TowerPrefabs;
        public LayerMask hitLayers;
        public GameState State;

        public delegate void NextWaveDelegate();
        public delegate void GameOverDelegate();
        
        public event NextWaveDelegate NextWaveEvent;
        public event GameOverDelegate GameOverEvent;
        public event GameOverDelegate VictoryEvent;

        [SerializeField] private Spawn[] spawns;
        [SerializeField] private LevelInfo levelInfo;
        
        private int _selectedIndex;

        public static GameManager Instance { get; set; }

        private readonly List<Tower> _placedTowers = new();

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
            foreach (var spawn in spawns)
            {
                spawn.Configure(levelInfo);
            }
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
                else if (_dummy != null && Input.GetMouseButtonUp(1))
                {
                    Destroy(_dummy.gameObject);
                    _dummy = null;
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

            if (WaveIndex >= 10 && EnemyCount == 0)
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
            if (WaveIndex >= 10) return;
            if (!_backgroundAudioSource.isPlaying)
                _backgroundAudioSource.Play();
            WaveTimer = WaveTimerStartTime + WaveTimerStep;
            WaveTimerStartTime = WaveTimer;
            WaveIndex++;        
            if (NextWaveEvent != null) NextWaveEvent.Invoke();
            State = GameState.Wave;
            foreach (var tower in _placedTowers)
            {
                tower.Initialize();
            }
        }

        private void PlaceTower()
        {
            if (!_canBePlaced) return;
            var tower = Instantiate(TowerPrefabs[_selectedIndex], null);
            Destroy(_dummy.gameObject);
            tower.transform.position = _hitByTowerGameObject.transform.position;
            tower.Configure();
            _hitByTowerGameObject.tag = "Tower";
            Money -= tower.TowerCost;
            _isTowerSelected = false;
            _dummy = null;
            _hitByTowerGameObject = null;
            _placedTowers.Add(tower);
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
            _selectedIndex = towerIndex;
            _dummy = Instantiate(TowerDummyPrefabs[towerIndex], null);
            _dummy.Configure();
        }

        public void FollowCursor()
        {
            var mouse = Input.mousePosition;
            var castPoint = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;
            if (!Physics.Raycast(castPoint, out hit, Mathf.Infinity, hitLayers)) return;
            Debug.Log("Hit by raycast - " + hit.transform.name);
            _dummy.transform.position = hit.point;
            _hitByTowerGameObject = hit.transform.gameObject;
            if (hit.transform.tag == "TowerPlace")
            {
                _dummy.Colorize(Color.green);
                _canBePlaced = true;
            }
            else
            {
                _dummy.Colorize(Color.red);
                _canBePlaced = false;
            }
        }
    }

    public enum GameState
    {
        Start,
        Wave    
    }
}