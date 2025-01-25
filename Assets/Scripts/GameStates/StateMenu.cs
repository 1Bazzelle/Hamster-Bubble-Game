using UnityEngine;

public class StateMenu : GameState
{
    public override void Enter()
    {
        UIManager.Instance.ChangeScreen(UIManager.ScreenID.MainMenu);
    }
    public override void Update()
    {

    }
    public override void Exit()
    {

    }
}
