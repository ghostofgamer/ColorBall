using UnityEngine;
using DG.Tweening;

public class WallParent : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] [Range(1,5)] int _wallColorCount;
    [SerializeField] float _tweenDuration = 1f;

    float _defaultXPos;

    public static WallParent ActiveWallParent;

    private void Start()
    {
        TweeningOnSpawn();
        SetWallColor();
    }

    private void TweeningOnSpawn()
    {
        _defaultXPos = transform.position.x;
        float tweenXPos = _defaultXPos > 0 ? _defaultXPos - 3 : _defaultXPos + 3;
        transform.DOMoveX(tweenXPos, _tweenDuration);
    }

    public void Init()
    {
        ActiveWallParent = this;
        int randomColorIndex = Random.Range(0, _wallColorCount);
        GameEvents.CallOnWallActivated(randomColorIndex);
    }

    private void SetWallColor()
    {
        Wall[] shuffleWall = new Wall[transform.childCount];

        // register all child to the array
        for (int i = 0; i < shuffleWall.Length; i++)
        {
            shuffleWall[i] = transform.GetChild(i).GetComponent<Wall>();
        }

        // shuffle the array
        for (int i = 0; i < shuffleWall.Length; i++)
        {
            int randomIndex = Random.Range(i, shuffleWall.Length);

            Wall temp = shuffleWall[i];
            shuffleWall[i] = shuffleWall[randomIndex];
            shuffleWall[randomIndex] = temp;
        }

        // change array color
        for (int i = 0; i < shuffleWall.Length; i++)
        {
            int limitColorIndex = i % _wallColorCount;
            shuffleWall[i].ChangeColor(limitColorIndex);
        }
    }
    
    public void Destroy()
    {
        transform.DOMoveX(_defaultXPos, _tweenDuration).OnComplete(()=> Destroy(gameObject));
    }
}
