using System.Globalization;
using TowerDefence;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    private GameManager _gameManager;

    public Text LifeCountText;

    public Text WaveNumberText;

    public Text WaveTimerText;

    public Text MoneyText;

    public Text EnemyText;

    public GameObject GameOverPanel;

    public GameObject VictoryPanel;

    public GameObject GamePanel;

    public AudioClip[] Clips;

    // Use this for initialization
    void Start()
    {
        _gameManager = GameManager.Instance;        
        GameManager.Instance.GameOverEvent += OnGameOverEvent;
        GameManager.Instance.VictoryEvent += OnVictoryEvent;
    }

    private void OnVictoryEvent()
    {
        GamePanel.SetActive(false);
        VictoryPanel.SetActive(true);
        var audio = GetComponent<AudioSource>();
        if (audio != null)
        {
            Debug.Log("Play victory music");
            transform.GetChild(0).GetComponent<AudioSource>().Stop();
            audio.clip = Clips[0];
            audio.Play();
        }
    }

    private void OnGameOverEvent()
    {
        GamePanel.SetActive(false);
        GameOverPanel.SetActive(true);
        var audio = GetComponent<AudioSource>();
        if (audio != null)
        {
            Debug.Log("Play end music");
            transform.GetChild(0).GetComponent<AudioSource>().Stop();
            audio.clip = Clips[1];
            audio.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        LifeCountText.text = _gameManager.LifeCount.ToString();
        WaveNumberText.text = _gameManager.WaveIndex.ToString();
        WaveTimerText.text = _gameManager.WaveTimer.ToString("f0", CultureInfo.InvariantCulture);
        MoneyText.text = _gameManager.Money.ToString();
        EnemyText.text = _gameManager.EnemyCount.ToString();
    }
}//Bill Conti - Victory (From Rocky OST)