using UnityEngine;
using UnityEngine.UI;

public class CoinViewer : MonoBehaviour
{
    private Text _coinText;

    private void Start()
    {
        _coinText = GetComponent<Text>();
        _coinText.text = PlayerPrefs.GetInt("Coin").ToString();
    }

    public void ShowCoinText(int value )
    {
        _coinText.text = value.ToString();
        Debug.Log("SHOWWWWWWW" );
    }
}