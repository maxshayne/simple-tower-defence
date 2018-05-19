using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    private GameManager _gameManager;

    public Text LifeCountText;

    public Text WaveTimerText;

    public Text MoneyText;

    // Use this for initialization
    void Start()
    {
        _gameManager = GameManager.Instance;        
    }

    // Update is called once per frame
    void Update()
    {
        LifeCountText.text = _gameManager.LifeCount.ToString();
        WaveTimerText.text = _gameManager.WaveTimer.ToString(CultureInfo.InvariantCulture);
        MoneyText.text = _gameManager.Money.ToString();
    }
}