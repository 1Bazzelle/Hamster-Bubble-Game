using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GameState curState;

    public LevelCollection levelCollection;

    private LevelManager levelManager;

    public List<(Player player, int joystickID)> players = new();

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private List<Material> playerMaterials;

    [SerializeField] private Camera uiCam;

    public (bool hasWinner, Player winner) game;

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
            players[i].player.LockMovement();
        }
    }
    public void ToggleSinglePlayerMovement(Player player, bool state)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].player == player)
            {
                if (!state) player.LockMovement();
                if (state) player.UnlockMovement();
            }
        }
    }
    public void UnlockPlayerMovement()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].player.UnlockMovement();
        }
    }

    public void OnPlayerFinish(Player player)
    {
        player.hasFinished = true;
        ToggleSinglePlayerMovement(player, false);

        bool first = true;

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].player == player) continue;

            if (players[i].player.hasFinished == true)
            {
                first = false;
                break;
            }
        }

        if (first)
        {
            game.winner = player;
            game.hasWinner = true;
        }

        for (int i = 0; i < players.Count; i++)
        {
            if (!players[i].player.hasFinished)
                return;
        }

        StartCoroutine(QueueGameFinish());
    }

    private void ResetPlayers()
    {
        foreach ((Player player, int joystickID) playerData in players)
        {
            if (playerData.player != null)
            {
                Destroy(playerData.player.gameObject);
            }
        }

        players.Clear();
    }
    public void AddPlayer(int joystickID)
    {
        GameObject newPlayer = Instantiate(playerPrefab);
        newPlayer.SetActive(false);
        Player playerScript = newPlayer.GetComponent<Player>();


        List<Material> availableMaterials = new List<Material>(playerMaterials);

        foreach ((Player player, int joystickID) player in players)
        {
            Material usedMaterial = player.player.GetMaterial();
            if (availableMaterials.Contains(usedMaterial))
            {
                availableMaterials.Remove(usedMaterial);
            }
        }

        if (availableMaterials.Count > 0)
        {
            Material material = availableMaterials[Random.Range(0, availableMaterials.Count)];
            playerScript.ChangeHampterMaterial(material);
        }
        else Debug.Log("Not enough materials");

        players.Add((playerScript, joystickID));

        playerScript.playerIndex = joystickID;
    }
    public Player GetPlayerByJoystick(int joystickID)
    {
        foreach((Player player, int joystick) player in players)
        {
            if (player.joystick == joystickID) return player.player;
        }
        Debug.LogWarning("Didnt find player with that Joystick");
        return null;
    }

    public int GetJoystickID(int playerIndex)
    {
        return players[playerIndex - 1].joystickID;
    }

    public void LoadLevel(GameObject level)
    {
        uiCam.gameObject.SetActive(false);
        levelManager.LoadLevel(level);
        ChangeState(new StateLevel());
    }


    private IEnumerator QueueGameFinish()
    {
        yield return new WaitForSeconds(3f);
        uiCam.gameObject.SetActive(true);
        levelManager.UnloadLevel();
        ResetPlayers();
        game.hasWinner = false;
        ChangeState(new StateMenu());
    }
}
