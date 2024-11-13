using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] int _scoreMultiple = 1;

    private int _score;
    public int Score => _score;

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
