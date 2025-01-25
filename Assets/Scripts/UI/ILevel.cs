using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ILevel : IScreen
{
    private VisualElement root;
    private VisualElement screenElements;

    private bool countdownOver = false;
    private float goTextTimer = 3;

    public void Initialize(VisualTreeAsset tree, VisualElement root)
    {
        screenElements = tree.Instantiate();
        root.Add(screenElements);

        this.root = root;

        root.Q<Label>("CountdownLabel").style.display = DisplayStyle.None;
        countdownOver = false;
        goTextTimer = 3;

        SubscribeButtons();
    }

    public void Update()
    {
        if(UIManager.Instance.countdown != -1 && countdownOver == false)
        {
            root.Q<Label>("CountdownLabel").style.display = DisplayStyle.Flex;
            root.Q<Label>("CountdownLabel").text = "" + (int)(UIManager.Instance.countdown + 1);
        }
        if(UIManager.Instance.countdown == -1 && countdownOver == false)
        {
            root.Q<Label>("CountdownLabel").text = "GO";
            goTextTimer -= Time.deltaTime;
        }
        if (goTextTimer < 0 && countdownOver == false)
        {
            countdownOver = true;
            root.Q<Label>("CountdownLabel").style.display = DisplayStyle.None;
        }
        
    }
    private void SubscribeButtons()
    {

    }

    #region On Click Events



    #endregion
}
