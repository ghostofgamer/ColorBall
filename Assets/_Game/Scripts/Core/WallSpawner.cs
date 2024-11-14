using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float _wallGap = 3f;

    private int _wallDirection = 1;
    private int _wallCount = 0;
    private bool _obstacleEnabled = false;
    private bool _canSpawnObstacle = true;

    [Header("External Components")]
    [SerializeField] Obstacle _obstaclePrefab;
    [SerializeField] GameCustomizationSO _gameCustom;

    private List<WallParent> _wallParentList = new List<WallParent>();

    private WallParent _selectedWall;

    private void OnEnable()
    {
        GameEvents.OnPlayerRedirect += ChangeActiveWall;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerRedirect -= ChangeActiveWall;
    }

    private void Start()
    {
        SpawnWall();
        SpawnWall();

        _wallParentList[0].Init();
    }

    private void SpawnWall()
    {
        CalculateDifficulty();
        SpawnObstacle();

        WallParent wallParent = Instantiate(_selectedWall, transform);
        // wallParent.transform.position = Vector2.right * _wallGap * _wallDirection;
        wallParent.transform.position = Vector2.right * 6.3f * _wallDirection;
        wallParent.gameObject.name = $"Wall Parent {_wallCount}";

        _wallParentList.Add(wallParent);

        _wallDirection *= -1;
        _wallCount++;
    }

    private void SpawnObstacle()
    {
        if (!_obstacleEnabled) return;

        if (_canSpawnObstacle)
        {
            StartCoroutine(SpawnObstacleRoutine());
        }
    }

    IEnumerator SpawnObstacleRoutine()
    {
        _canSpawnObstacle = false;
        Instantiate(_obstaclePrefab, transform);
        yield return new WaitForSeconds(Random.Range(4f,10f));
        _canSpawnObstacle = true;
    }

    private void ChangeActiveWall()
    {
        _wallParentList[0].Destroy();

        _wallParentList.RemoveAt(0);
        _wallParentList[0].Init();

        SpawnWall();
    }

    private void CalculateDifficulty()
    {
        if (_wallCount < 3)
        {
            _selectedWall = _gameCustom.WallParentList[0];
        }
        else if (_wallCount >= 3 && _wallCount < 10)
        {
            _selectedWall = _gameCustom.WallParentList[1];
        }
        else if (_wallCount >= 10 && _wallCount < 15)
        {
            _selectedWall = _gameCustom.WallParentList[Random.Range(1, 3)];
        }
        else if (_wallCount >= 15 && _wallCount < 25)
        {
            _selectedWall = _gameCustom.WallParentList[Random.Range(2, 4)];
        }
        else if (_wallCount >= 25)
        {
            if (_obstacleEnabled) return;
            _obstacleEnabled = true;
        }
    }
}
