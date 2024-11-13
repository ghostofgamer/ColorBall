using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float _screenHalfWidthInWorldUnit = 4f;
    [SerializeField] float _minHalfHeightInWorldUnit = 6f;
    [SerializeField] Camera _mainCam;

    [Header("Shake Parameters")]
    [SerializeField] float _duration = 1f;
    [SerializeField] float _strength;
    [SerializeField] [Range(0,10)] int _vibrato;
    [SerializeField] float _randomnes;

    private void OnEnable()
    {
        GameEvents.OnGameOver += ShakeCamera;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= ShakeCamera;
    }

    private void Start()
    {
        AdjustCameraSize();
    }

    private void ShakeCamera()
    {
        transform.DOShakePosition(_duration, _strength, _vibrato, _randomnes, false, true);
    }

    private void AdjustCameraSize()
    {
        float camSize = _screenHalfWidthInWorldUnit / _mainCam.aspect;

        if (camSize < _minHalfHeightInWorldUnit)
        {
            camSize = _minHalfHeightInWorldUnit;
        }

        _mainCam.orthographicSize = camSize;
    }
}
