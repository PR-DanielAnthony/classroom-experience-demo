using UnityEngine;

public abstract class Menu : MonoBehaviour
{
    protected MenuManager menuManager;

    public void SetMenuManager(MenuManager menuManager)
    {
        this.menuManager = menuManager;
    }

    public virtual void Initialize()
    {
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}