using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private VisualElement root;

    [SerializeField] private VisualTreeAsset mainMenu;
    [SerializeField] private VisualTreeAsset playerScreen;
    [SerializeField] private VisualTreeAsset levelSelect;
    [SerializeField] private VisualTreeAsset level;

    [Header("Templates")]
    public VisualTreeAsset missionTemplate;

    [Header("Sprites")]
    public Sprite emptyPictureSlot;

    public float countdown = -1;

    public enum ScreenID
    {
        None,
        MainMenu,
        PlayerScreen,
        LevelSelect,
        Level
    }

    private IScreen curScreen;

    private void Awake()
    {
        #region Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        #endregion

        root = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {
        
    }
    private void Update()
    {
        curScreen?.Update();
    }

    public void ChangeScreen(ScreenID newScreen)
    {
        root.Clear();

        switch (newScreen)
        {
            case ScreenID.MainMenu:

                curScreen = new IMainMenu();
                curScreen.Initialize(mainMenu, root);

                break;
            case ScreenID.PlayerScreen:

                curScreen = new IPlayerScreen();
                curScreen.Initialize(playerScreen, root);

                break;
            case ScreenID.LevelSelect:

                curScreen = new ILevelSelect();
                curScreen.Initialize(levelSelect, root);

                break;
            case ScreenID.Level:

                curScreen = new ILevel();
                curScreen.Initialize(level, root);

                break;

            default:

                curScreen = null;

                break;
        }
    }
}