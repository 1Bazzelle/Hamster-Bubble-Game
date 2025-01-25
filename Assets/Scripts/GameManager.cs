using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GameState curState;

    public LevelCollection levelCollection;

    private LevelManager levelManager;

    public List<Player> players = new();

    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private Camera uiCam;

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
        levelManager = new();

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

    public void AddPlayer()
    {
        GameObject newPlayer = Instantiate(playerPrefab);
        newPlayer.SetActive(false);
        Player playerScript = newPlayer.GetComponent<Player>();
        playerScript.playerIndex = players.Count+1;
        players.Add(playerScript);
    }

    /*
    public void AddPlayer(int joystickID)
    {
        GameObject newPlayer = Instantiate(playerPrefab);
        newPlayer.SetActive(false);
        Player playerScript = newPlayer.GetComponent<Player>();

        players.Add((playerScript, joystickID));

        playerScript.playerIndex = players.Count; // Keep player index for UI purposes
    }

    public int GetJoystickID(int playerIndex)
    {
        return players[playerIndex - 1].joystickID; // Return joystick ID for this player
    }
    */

    public void LoadLevel(GameObject level)
    {
        uiCam.gameObject.SetActive(false);
        levelManager.LoadLevel(level);
        ChangeState(new StateLevel());
    }
}
