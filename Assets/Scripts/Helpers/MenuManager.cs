using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private List<Menu> menus;

    private Menu currentMenu;
    private readonly Stack<Menu> history = new();

    private void Start()
    {
        foreach (var menu in menus)
        {
            menu.SetMenuManager(this);
            menu.Initialize();
            menu.Hide();
        }

        Show(menus.First());
    }

    public void Show(Menu menu, bool remember = true)
    {
        if (currentMenu)
        {
            if (remember)
                history.Push(currentMenu);

            currentMenu.Hide();
        }

        menu.Show();
        currentMenu = menu;
    }

    public void Show<T>(bool remember = true) where T : Menu
    {
        var menu = menus.FirstOrDefault(m => m is T);

        if (menu != null)
        {
            if (currentMenu)
            {
                if (remember)
                    history.Push(currentMenu);

                currentMenu.Hide();
            }

            menu.Show();
            currentMenu = menu;
        }
    }

    public void Back(bool remember = false)
    {
        if (history.TryPop(out var previousMenu))
            Show(previousMenu, remember);
    }

    public T GetMenu<T>() where T : Menu
    {
        foreach (var menu in menus)
            if (menu is T tView)
                return tView;

        return null;
    }

    public Menu GetCurrentMenu()
    {
        return currentMenu;
    }
}