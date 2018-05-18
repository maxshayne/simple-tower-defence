using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{

    public GameObject TowerPrefab;

    public LayerMask hitLayers;

    private bool _isTowerSelected;

    private GameObject _gameObject;

    private bool _canBePlaced = false;

    private GameObject _hitByTowerGameObject;

	// Use this for initialization
	void Start ()
	{

	    //var startPos = Place.transform.position;

	    //for (int i = 0; i < 10; i++)
	    //{
	    //    for (int j = 0; j < 10; j++)
	    //    {
	    //        var pos = new Vector3(startPos.x + 10 * i, startPos.y, startPos.z + 10 * j);
	    //        Instantiate(Place, pos, Quaternion.identity);
	    //    }
	    //}
		
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
    }

    private void PlaceTower()
    {
        if (!_canBePlaced) return;
        _gameObject.transform.position = _hitByTowerGameObject.transform.position;
        _hitByTowerGameObject.tag = "Tower";
        var tower = _gameObject.GetComponent<TowerController>();
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
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, hitLayers))
        {
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
}
