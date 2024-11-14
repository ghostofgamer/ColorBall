
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Internal Components")]
    [SerializeField] Rigidbody2D _rigidybody;
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] TrailRenderer _trailRenderer;
    [SerializeField] ParticleSystem _splatFX;

    [Header("External Components")]
    [SerializeField] GameCustomizationSO _gameCustom;

    private int _moveDirection = 1;
    private int _currentColorIndex;

    Vector2 _startingPos;

    enum PlayerState { isStarting, isPlaying, isDeath }
    PlayerState _currentState;

    private void OnEnable()
    {
        GameEvents.OnWallActivated += ChangeColor;
    }

    private void OnDisable()
    {
        GameEvents.OnWallActivated -= ChangeColor;
    }

    private void Start()
    {
        _startingPos = transform.position;

        _rigidybody.bodyType = RigidbodyType2D.Kinematic;
        _rigidybody.gravityScale = _gameCustom.GravityScale;

        _currentState = PlayerState.isStarting;
    }

    private void FixedUpdate()
    {
        if (_currentState == PlayerState.isPlaying)
        {
            Move();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Obstacle obstacle = collision.GetComponent<Obstacle>();

            if (obstacle.ColorIndex != _currentColorIndex)
            {
                PlayerDeath();
                return;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Wall wall = other.gameObject.GetComponent<Wall>();

            if (wall.ColorIndex != _currentColorIndex)
            {
                PlayerDeath();
                return;
            }
            else
            {
                GameEvents.CallOnPlayerRedirect();

                _splatFX.Play();
                Redirect(other.transform);
            }
        }
        else
        {
            PlayerDeath();
        }
    }

    private void PlayerDeath()
    {
        if (_currentState == PlayerState.isDeath) return;

        _currentState = PlayerState.isDeath;

        GameEvents.CallOnGameOver();

        _splatFX.Play();
        _rigidybody.bodyType = RigidbodyType2D.Kinematic;
        _rigidybody.velocity = Vector2.zero;
        _renderer.enabled = false;
    }

    public void Revive()
    {
        _currentState = PlayerState.isStarting;

        if (WallParent.ActiveWallParent.transform.position.x < 0)
        {
            _startingPos.x *= -1;
        }

        _moveDirection = _startingPos.x > 0 ? -1 : 1;
        transform.position = _startingPos;

        _rigidybody.bodyType = RigidbodyType2D.Kinematic;
        _rigidybody.velocity = Vector2.zero;
        _renderer.enabled = true;
    }

    public void ChangeColor(int colorIndex)
    {
        _currentColorIndex = colorIndex;
        Color color = _gameCustom.ColorList[_currentColorIndex];

        // change ball color
        // _renderer.color = color;
        _renderer.sprite = _gameCustom.SpriteList[colorIndex];

        // change trail renderer color
        _trailRenderer.material.color = color;

        var psmain = _splatFX.main;
        psmain.startColor = color;
    }

    public void HandleJumpInput()
    {
        if (_currentState == PlayerState.isDeath) return;

        if (_currentState == PlayerState.isStarting)
        {
            _currentState = PlayerState.isPlaying;
            _rigidybody.bodyType = RigidbodyType2D.Dynamic;

            GameEvents.CallOnGameStarted();
        }

        SoundManager.Instance.PlayAudio(AudioType.JUMP);
        Jump();
    }

    private void Redirect(Transform wallTransform)
    {
        _moveDirection = wallTransform.position.x > 0 ? -1 : 1;

        // Flip face
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);
    }

    private void Move()
    {
        _rigidybody.velocity = new Vector2(_gameCustom.HorizontalSpeed * _moveDirection, _rigidybody.velocity.y);
    }

    private void Jump()
    {
        _rigidybody.velocity = new Vector2(_rigidybody.velocity.x, _gameCustom.JumpForce);
    }
}
