using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GameState curState;

    public LevelCollection levelCollection;

    public LevelManager levelManager;

    public List<Player> players;

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
    }

    private void OnEnable()
    {
        ChangeState(new StateMenu());
    }
    private void Update()
    {
        curState.Update();
    }

    public void ChangeState(GameState newState)
    {
        curState?.Exit();
        curState = newState;
        curState.Enter();
    }

    public void LockPlayerMovement()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].LockMovement();
        }
    }
    public void UnlockPlayerMovement()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].UnlockMovement();
        }
    }
}
