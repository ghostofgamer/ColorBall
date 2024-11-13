using UnityEngine;

public class MenuInput : MonoBehaviour
{
    private void Update()
    {
        GetMobileInput();
    }

    private void GetMobileInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if ((MenuController.CurrentMenu == MenuType.Main))
            {
                SoundManager.Instance.PlayAudio(AudioType.POP);

                MenuController.Instance.OpenMenu(MenuType.Exit);
            }
            else if (MenuController.CurrentMenu == MenuType.Credit ||
                     MenuController.CurrentMenu == MenuType.Setting ||
                     MenuController.CurrentMenu == MenuType.Rate)
            {
                MenuController.Instance.CloseMenu();
            }
        }
    }
}
