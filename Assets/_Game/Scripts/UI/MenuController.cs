using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuType
{
    None,
    Main,
    Credit,
    Rate,
    Setting,
    Gameplay,
    GameOver,
    Exit,
    Revive
}

public class MenuController : Singleton<MenuController>
{
    [SerializeField] MenuType _startupMenu;
    [SerializeField] List<Menu> _menus = new List<Menu>();

    Hashtable _menuTable = new Hashtable();
    Stack<Menu> _menuStack = new Stack<Menu>();

    public Stack<Menu> MenuStack => _menuStack;

    private void Start()
    {
        RegisterAllMenus();

        OpenMenu(_startupMenu);
    }

    public void SwitchMenu(MenuType type)
    {
        CloseMenu();
        OpenMenu(type);
    }

    public void OpenMenu(MenuType type)
    {
        if (!MenuExist(type))
        {
            Debug.LogWarning($"You are trying to open a Menu {type} that has not been registered.");
            return;
        }

        Menu menu = GetMenu(type);
        menu.OpenMenu();
        _menuStack.Push(menu);
    }

    public void CloseMenu()
    {
        if (_menuStack.Count <= 0)
        {
            Debug.LogWarning("MenuController CloseMenu ERROR: No menus in stack!");
            return;
        }
        Menu lastMenuStack = _menuStack.Pop();

        // Disable GameObject
        lastMenuStack.CloseMenu();
    }

    private void RegisterAllMenus()
    {
        foreach (Menu menu in _menus)
        {
            RegisterMenu(menu);

            // disable menu after register to hash table.
            menu.CloseMenu();
        }
        Debug.Log("Successfully registered all menus.");
    }

    private void RegisterMenu(Menu menu)
    {
        if (menu.Type == MenuType.None)
        {
            Debug.LogWarning($"You are trying to register a {menu.Type} type menu that has not allowed.");
            return;
        }

        if (MenuExist(menu.Type))
        {
            Debug.LogWarning($"You are trying to register a Menu {menu.Type} that has already been registered.");
            return;
        }

        _menuTable.Add(menu.Type, menu);
    }

    private Menu GetMenu(MenuType type)
    {
        if (!MenuExist(type)) return null;
        return (Menu)_menuTable[type];
    }

    private bool MenuExist(MenuType type)
    {
        return _menuTable.ContainsKey(type);
    }

    public static MenuType CurrentMenu
    {
        get
        {
            return Instance._menuStack.Peek().Type;
        }
    }
}
