using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{

    private bool _isTowerSelected;

    private GameObject _gameObject;

    private bool _canBePlaced = false;

    private GameObject _hitByTowerGameObject;

    public int LifeCount = 4;

    public int Money = 500;

    public float WaveTimer = 40;

    public GameObject TowerPrefab;

    public LayerMask hitLayers;

    public delegate void NextWaveDelegate();

    public event NextWaveDelegate NextWaveEvent;

    public static Game Instance { get; set; }

    void Awake()
    {
        if (Instance == null || Instance != this)
            Instance = this;
    }

	// Use this for initialization
	void Start ()
	{

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

        WaveTimer -= Time.deltaTime;

        if (WaveTimer <= 0)
        {
            SpawnNextWave();
        }
    }

    private void SpawnNextWave()
    {
        WaveTimer = 60;
        NextWaveEvent.Invoke();
    }

    private void PlaceTower()
    {
        if (!_canBePlaced) return;
        var tower = _gameObject.GetComponent<TowerController>();
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
        var tower = _gameObject.GetComponent<TowerController>();
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
