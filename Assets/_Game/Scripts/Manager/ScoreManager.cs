using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] int _scoreMultiple = 1;
    [SerializeField] private int _coin;
    [SerializeField]private Text _coinText;
    
    private int _score;
    
    public int Score => _score;
    
    public int Coin=>_coin; 

    public static event System.Action<int> OnScoreUpdated;

    static ScoreManager _instance;

    public static ScoreManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ScoreManager>(includeInactive: true);
            }
            return _instance;
        }
    }

    private void OnEnable()
    {
        GameEvents.OnPlayerRedirect += AddScore;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerRedirect -= AddScore;
    }

    private void Start()
    {
        _coin = PlayerPrefs.GetInt("Coin",0);
        // _coinText.text = _coin.ToString();
    }

    public void AddCoin(int value)
    {
        _coin += value;
        PlayerPrefs.SetInt("Coin", _coin);
        Debug.Log("Сохраняем " + _coin);
        CoinViewer coinViewer = FindObjectOfType<CoinViewer>();
        coinViewer.ShowCoinText(_coin);
        // _coinText.text = _coin.ToString();
    }
    
    public void AddScore()
    {
        _score += _scoreMultiple;

        OnScoreUpdated?.Invoke(_score);
    }

    public void UpdateBestScore()
    {
        int bestScore;

        bestScore = SaveData.GetBestScore();
        if (_score > bestScore)
        {
            SaveData.SetBestScore(_score);
        }
    }
}
