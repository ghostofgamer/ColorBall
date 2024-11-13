using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float _moveSpeed;

    int _moveDirection;
    float _defaultYPos;

    [SerializeField] private SpriteRenderer _renderer;

    [Header("External Components")]
    [SerializeField] GameCustomizationSO _gameCustom;

    private int _colorIndex;

    public int ColorIndex => _colorIndex;

    private void ChangeColor(int colorIndex)
    {
        _colorIndex = colorIndex;

        _renderer.color = _gameCustom.ColorList[_colorIndex];
    }

    private void Start()
    {
        ChangeColor(Random.Range(0, _gameCustom.ColorList.Count));

        _defaultYPos = transform.position.y;
        float randomYPos = Random.Range(0f, 1f) >= .5f ? 1 : -1;
        transform.position = new Vector2(transform.position.x, transform.position.y * randomYPos);

        _moveDirection = transform.position.y > 0 ? -1 : 1;
    }

    private void Update()
    {
        transform.Translate(Vector2.up * _moveSpeed * _moveDirection * Time.deltaTime);

        if (Mathf.Abs(transform.position.y) > _defaultYPos)
        {
            Destroy(this.gameObject);
        }
    }
}
