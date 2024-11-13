using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Menu : MonoBehaviour
{

    public MenuType Type;

    [SerializeField] Canvas _canvasHolder;
    [SerializeField] bool _useTweeningUI;

    public event System.Action OnMenuOpenedEvent;
    public event System.Action OnMenuClosedEvent;

    bool _isOpen;

    protected virtual void Awake()
    {
        _isOpen = _canvasHolder.enabled;
    }

    public void OpenMenu()
    {
        if (_isOpen) return;
        _isOpen = true;
        
        _canvasHolder.enabled = true;
        OnMenuOpened();

        if (_useTweeningUI) OnMenuOpenedEvent?.Invoke();
    }

    public void CloseMenu()
    {
        if (!_isOpen) return;
        _isOpen = false;

        if (_useTweeningUI)
        {
            OnMenuClosedEvent?.Invoke();
            return;
        }

        _canvasHolder.enabled = false;
        OnMenuClosed();
    }

    protected virtual void OnMenuOpened()
    {

    }

    protected virtual void OnMenuClosed()
    {

    }

    protected void OnButtonPressed(Button button, UnityAction buttonListener)
    {
        if (!button)
        {
            Debug.LogWarning($"There is a 'Button' that is not attached to the '{gameObject.name}' script,  but a script is trying to access it.");
            return;
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(DefaultButtonListener);
        button.onClick.AddListener(buttonListener);

        void DefaultButtonListener()
        {
            // additional listener for all button menu
            SoundManager.Instance.PlayAudio(AudioType.POP);
            VibrationManager.Instance.StartVibration();
        }
    }
}
