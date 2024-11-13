using UnityEditor;
using UnityEngine;

public class HelpMenuItem : MonoBehaviour
{
    [MenuItem("SansDev/Help/Documentation (online)")]
    static void OpenDocumantation()
    {
        //Application.OpenURL($"{System.Environment.CurrentDirectory}/Documentation.pdf");

        string documentationLink = AdResources.Instance.DocumentationLink;

        if (!string.IsNullOrEmpty(documentationLink))
            Application.OpenURL(documentationLink);
    }

    [MenuItem("SansDev/Help/Get More Games")]
    static void OpenPortofolio()
    {
        Application.OpenURL($"https://codecanyon.net/user/sansdevs/portfolio");
    }
}
