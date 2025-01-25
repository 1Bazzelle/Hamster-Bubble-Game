using UnityEngine;

public class StateLevel : GameState
{
    public override void Enter()
    {
        GameManager.Instance.levelManager.LoadSelectedLevel();
    }
    public override void Update()
    {

    }
    public override void Exit()
    {

    }
}
