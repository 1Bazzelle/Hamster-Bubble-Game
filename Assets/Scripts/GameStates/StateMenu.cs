using UnityEngine;

public class StateMenu : GameState
{
    public override void Enter()
    {
        GameManager.Instance.ChangeState(new StateLevel());
    }
    public override void Update()
    {

    }
    public override void Exit()
    {

    }
}
