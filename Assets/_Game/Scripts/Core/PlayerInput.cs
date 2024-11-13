using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] Player _player;

    private void Update()
    {
#if UNITY_EDITOR
        GetInput();
#endif

#if UNITY_ANDROID
        GetMobileInput();
#endif
    }

    private void GetInput()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            _player.HandleJumpInput();
        }
    }

    private void GetMobileInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                _player.HandleJumpInput();
            }
        }
    }
}
