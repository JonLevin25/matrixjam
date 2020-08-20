using System;
using System.Collections.Generic;
using System.Linq;
using MatrixJam.Team14;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MenuType
{
    None,
    MainMenu,
    PauseMenu,
    LoseMenu,
    WinMenu
}


public class GameMenu : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private MenuType initMenu;
    [SerializeField] private MenuType[] showOverlayIn;
    [SerializeField] private MenuType[] showCreditsIn;
    [SerializeField] private MenuType[] showRestartBtnIn;
    [SerializeField] private MenuType[] showResumeBtnIn;
    [SerializeField] private MenuType[] showTitleIn;
    [SerializeField] private MenuType[] showQuitToMenuBtnIn;
    [SerializeField] private MenuType[] showQuitGameBtnIn;

    [Header("hard-refs")]
    [SerializeField] private GameObject overlay;
    [SerializeField] private GameObject resumeBtn;
    [SerializeField] private GameObject creditsBtn;
    [SerializeField] private GameObject restartBtn;
    [SerializeField] private GameObject quitGameBtn;
    [SerializeField] private GameObject quitToMenuBtn;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject titleSpace;
    [Space]
    
    [Header("dynamic refs")]
    [SerializeField] private GameObject[] showInMain;
    [SerializeField] private GameObject[] showInPause;
    [SerializeField] private GameObject[] showInLose;
    [SerializeField] private GameObject[] showInWin;

    public event Action OnResume;

    public MenuType CurrMenu { get; private set; }

    private IEnumerable<GameObject[]> AllObjectArrays
    {
        get
        {
            yield return showInMain;
            yield return showInPause;
            yield return showInLose;
            yield return showInWin;
        }
    }

    private IEnumerable<GameObject> AllObjects => AllObjectArrays
        .SelectMany(x => x)
        .Where(x => x != null);

    private void Start()
    {
        GameManager.ResetEvent += OnGameReset;
        ShowMenu(initMenu);
    }

    private void Update()
    {
        if (CurrMenu == MenuType.MainMenu)
            HandleMainMenuInput();
    }

    private void OnDestroy() => GameManager.ResetEvent -= OnGameReset;

    public void ShowMenu(MenuType menu)
    {
        Debug.Log(nameof(ShowMenu) + $"({menu})");
        var shouldPause = menu == MenuType.PauseMenu;
        Time.timeScale = shouldPause ? 0f : 1f;
        
        if (CurrMenu == MenuType.PauseMenu && menu == MenuType.None) OnResume?.Invoke();
        CurrMenu = menu;
        if (menu == MenuType.None)
        {
            gameObject.SetActive(false);
            return;
        }
        
        ShowInternal(menu);
    }

    private void ShowInternal(MenuType menu)
    {
        gameObject.SetActive(true);
        HideAll();

        var toShow = ObjectsToShow(menu);
        
        foreach (var obj in toShow)
        {
            try
            {
                obj.SetActive(true);
            } catch(UnassignedReferenceException) { }
        }
        
        ShowConditional(overlay, showOverlayIn);
        ShowConditional(title, showTitleIn);
        ShowConditional(resumeBtn, showResumeBtnIn);
        ShowConditional(creditsBtn, showCreditsIn);
        ShowConditional(restartBtn, showRestartBtnIn);
        ShowConditional(quitGameBtn, showQuitGameBtnIn);
        ShowConditional(quitToMenuBtn, showQuitToMenuBtnIn);
    }

    private void OnGameReset()
    {
        HideMenu();
    }

    private void ShowConditional(GameObject btn, IEnumerable<MenuType> menus)
    {
        var shouldShow = menus.Contains(CurrMenu);
        if (btn) btn.SetActive(shouldShow);
    }

    public void HideMenu()
    {
        Debug.Log(nameof(HideMenu));
        ShowMenu(MenuType.None);
    }

    public void ShowCredits(bool show)
    {
        Debug.Log(nameof(ShowCredits) + $"({show})");
        credits.SetActive(show);
    }

    public void RestartGame()
    {
        Debug.Log(nameof(RestartGame));
        SceneManager.LoadScene(1);
    }

    public void ExitToMenu()
    {
        Debug.Log(nameof(ExitToMenu));
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Debug.Log(nameof(ExitGame));
        Application.Quit();
    }

    private GameObject[] ObjectsToShow(MenuType menu)
    {
        try
        {
            switch (menu)
            {
                case MenuType.None:
                    return new GameObject[0];
                case MenuType.MainMenu:
                    return showInMain;
                case MenuType.PauseMenu:
                    return showInPause;
                case MenuType.LoseMenu:
                    return showInLose;
                case MenuType.WinMenu:
                    return showInWin;
                default:
                    throw new ArgumentOutOfRangeException(nameof(menu), menu, null);
            }
        }
        catch (UnassignedReferenceException)
        {
            return new GameObject[0];
        }
    }

    private void HideAll()
    {
        credits.SetActive(false);
        
        foreach (var obj in AllObjects)
            obj.SetActive(false);
    }

    private void HandleMainMenuInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            RestartGame();

        if (Input.GetKeyDown(KeyCode.Escape))
            ExitGame();
    }
}
