using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager Instance;

    bool _vibrationEnable = true;

    private void OnEnable()
    {
        Instance = this;
    }
    private void OnDisable()
    {
        Instance = null;
    }

    private void Start()
    {
        _vibrationEnable = SaveData.GetVibrateState();
    }

    public void ToggleVibration()
    {
        _vibrationEnable = !_vibrationEnable;
        SaveData.SetVibrateState(_vibrationEnable);
    }

    public void StartVibration()
    {
        if (!_vibrationEnable) return;

        Vibration.Vibrate(20);

#if UNITY_EDITOR
        Debug.Log("Vibrate!");
#endif
    }
}
