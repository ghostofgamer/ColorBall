using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/New Game Customization", fileName = "Game Customization")]
public class GameCustomizationSO : ScriptableObject
{
    [Header("Color Theme")]
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _darkColor;

    [SerializeField] private List<Color> _colorList;
    [SerializeField] private List<Sprite> _spriteList;
    [SerializeField] private List<Sprite> _spriteWalls;

    [Space]
    [SerializeField] private List<WallParent> _wallParentList;

    [Header("Player Customization")]
    [SerializeField] float _jumpForce;
    [SerializeField] float _horizontalSpeed;
    [SerializeField] float _gravityScale;

    public Color DefaultColor => _defaultColor;
    
    public Color DarkColor => _darkColor;

    public List<Color> ColorList => _colorList;
    
    public List<Sprite> SpriteList => _spriteList;
    
    public List<Sprite> SpriteWalls => _spriteWalls;
    
    public List<WallParent> WallParentList => _wallParentList;

    public float JumpForce => _jumpForce;
    
    public float HorizontalSpeed => _horizontalSpeed;
    
    public float GravityScale => _gravityScale;

}