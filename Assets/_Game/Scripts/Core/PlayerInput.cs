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

#if UNITY_WEBGL
        GetInput();
#endif
    }

    private void GetInput()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("GetUnput");
            _player.HandleJumpInput();
        }
    }

    private void GetMobileInput()
    {

        if (Input.touchCount > 0)
        {
            Debug.Log("GetMobileUnput");
            Touch touch = Input.touches[0];
            if (touch.phase == TouchPhase.Began)
            {
                _player.HandleJumpInput();
            }
        }
    }
}
