using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    private Game _gameController;

    public Text LifeCountText;

    public Text WaveTimerText;

    public Text MoneyText;

    // Use this for initialization
    void Start()
    {
        _gameController = gameObject.GetComponent<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        LifeCountText.text = _gameController.LifeCount.ToString();
        WaveTimerText.text = _gameController.WaveTimer.ToString(CultureInfo.InvariantCulture);
        MoneyText.text = _gameController.Money.ToString();
    }
}