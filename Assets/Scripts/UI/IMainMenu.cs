using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class IMainMenu : IScreen
{
    private VisualElement root;
    private VisualElement screenElements;
    public void Initialize(VisualTreeAsset tree, VisualElement root)
    {
        screenElements = tree.Instantiate();
        root.Add(screenElements);

        this.root = root;

        SubscribeButtons();
    }

    public void Update()
    {

    }

    private void SubscribeButtons()
    {
        root.Q<Button>("StartButton").clicked += OnStartButtonClick;
        root.Q<Button>("QuitButton").clicked += OnQuitButtonClick;
    }

    #region On Click Events
    private void OnStartButtonClick()
    {
        UIManager.Instance.ChangeScreen(UIManager.ScreenID.PlayerScreen);
    }
    private void OnQuitButtonClick()
    {
        Application.Quit();
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #endif
    }
    #endregion

}
