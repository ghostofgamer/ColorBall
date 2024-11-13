using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;

    [Header("External Components")]
    [SerializeField] GameCustomizationSO _gameCustom;

    private int _colorIndex;

    public int ColorIndex => _colorIndex;

    public void ChangeColor(int colorIndex)
    {
        _colorIndex = colorIndex;

        _renderer.color = _gameCustom.ColorList[_colorIndex];
    }
}
