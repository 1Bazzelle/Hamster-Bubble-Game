using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ILevelSelect : IScreen
{
    private VisualElement root;
    private VisualElement screenElements;

    private VisualElement levelList;

    public void Initialize(VisualTreeAsset tree, VisualElement root)
    {
        screenElements = tree.Instantiate();
        root.Add(screenElements);

        this.root = root;

        levelList = root.Q<VisualElement>("LevelContainer");

        int levelAmount = GameManager.Instance.levelCollection.GetLevelAmount();
        for(int i = 0; i < levelAmount; i++)
        {

            GameObject levelObject = GameManager.Instance.levelCollection.GetLevelIndex(i);
            Level curLevel = levelObject.GetComponent<Level>();

            Button level = new();
            level.AddToClassList("level-image");
            level.style.backgroundImage = new StyleBackground(curLevel.levelImage);

            level.userData = levelObject;

            level.clicked += () => OnLevelClick((GameObject)level.userData);

            levelList.Add(level);
        }
    }

    public void Update()
    {

    }

    #region On Click Events

    private void OnLevelClick(GameObject levelObject)
    {
        UIManager.Instance.ChangeScreen(UIManager.ScreenID.Level);
        GameManager.Instance.LoadLevel(levelObject);
    }

    #endregion
}
