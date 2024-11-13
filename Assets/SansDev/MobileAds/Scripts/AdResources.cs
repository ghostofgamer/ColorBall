using UnityEngine;

public class AdResources : MonoBehaviour
{
    private static AdResources instance;

    public static AdResources Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<AdResources>("AdResources");
            }
            return instance;
        }
    }

    [SerializeField] private AdsData _adsData;
    public AdsData AdsData => _adsData;

    [SerializeField][TextArea] private string _documentationLink;
    public string DocumentationLink => _documentationLink;
}
