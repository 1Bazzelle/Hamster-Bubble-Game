using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class IPlayerScreen : IScreen
{
    private VisualElement root;
    private VisualElement screenElements;

    private List<(VisualElement root, bool empty)> playerSlots;
    // Maybe not
    private List<int> assignedJoysticks = new List<int>();

    private float maxCountdown = 3;
    private float countdown = 3;

    public void Initialize(VisualTreeAsset tree, VisualElement root)
    {
        screenElements = tree.Instantiate();
        root.Add(screenElements);

        this.root = root;

        root.Q<Label>("CountdownLabel").style.display = DisplayStyle.None;

        playerSlots = new()
        {
            (root.Q<VisualElement>("PlayerSlot1"), true),
            (root.Q<VisualElement>("PlayerSlot2"), true),
            (root.Q<VisualElement>("PlayerSlot3"), true),
            (root.Q<VisualElement>("PlayerSlot4"), true)
        };
        for (int i = 0; i < playerSlots.Count; i++)
        {
            playerSlots[i].root.Q<VisualElement>("NotConnected").style.display = DisplayStyle.Flex;
            playerSlots[i].root.Q<VisualElement>("Connected").style.display = DisplayStyle.None;

            playerSlots[i].root.Q<VisualElement>("Ready").style.display = DisplayStyle.None;
            playerSlots[i].root.Q<VisualElement>("NotReady").style.display = DisplayStyle.Flex;
        }
    }

    public void Update()
    {
        CheckNewPlayer();

        UpdateReadyButton();

        CheckGameStart();
    }

    private void CheckNewPlayer()
    {
        for (int joystickID = 1; joystickID <= 4; joystickID++)
        {
            KeyCode buttonStart = (KeyCode)System.Enum.Parse(typeof(KeyCode), $"Joystick{joystickID}Button7");

            if (!assignedJoysticks.Contains(joystickID) && Input.GetKeyDown(buttonStart))
            {
                AddNewPlayer(joystickID);
                break;
            }
        }
    }
    private void AddNewPlayer(int joystickID)
    {
        for (int i = 0; i < playerSlots.Count; i++)
        {
            if (playerSlots[i].empty)
            {
                playerSlots[i].root.Q<VisualElement>("NotConnected").style.display = DisplayStyle.None;
                playerSlots[i].root.Q<VisualElement>("Connected").style.display = DisplayStyle.Flex;

                (VisualElement root, bool empty) temp = playerSlots[i];
                temp.empty = false;
                playerSlots[i] = temp;

                assignedJoysticks.Add(joystickID);

                Debug.Log($"Joystick {joystickID} assigned to Player Slot {i + 1}");
                break;
            }
        }

        GameManager.Instance.AddPlayer(joystickID);
    }

    private void UpdateReadyButton()
    {
        for (int i = 0; i < assignedJoysticks.Count; i++)
        {
            int joystickID = assignedJoysticks[i];
            KeyCode buttonA = (KeyCode)System.Enum.Parse(typeof(KeyCode), $"Joystick{joystickID}Button0");

            if (Input.GetKeyDown(buttonA))
            {
                SwitchReadyState(i + 1);
            }
        }
    }
    private void SwitchReadyState(int player)
    {
        if (playerSlots[player - 1].empty)
        {
            Debug.LogWarning("Controller hasn't connected yet");
            return;
        }

        if(playerSlots[player - 1].root.Q<VisualElement>("Ready").style.display != DisplayStyle.Flex)
        {
            playerSlots[player - 1].root.Q<VisualElement>("Ready").style.display = DisplayStyle.Flex;
            playerSlots[player - 1].root.Q<VisualElement>("NotReady").style.display = DisplayStyle.None;
            return;
        }
        if (playerSlots[player - 1].root.Q<VisualElement>("Ready").style.display == DisplayStyle.Flex)
        {
            playerSlots[player - 1].root.Q<VisualElement>("Ready").style.display = DisplayStyle.None;
            playerSlots[player - 1].root.Q<VisualElement>("NotReady").style.display = DisplayStyle.Flex;
            return;
        }
    }

    private void CheckGameStart()
    {
        if (GameManager.Instance.players.Count == 0) return;

        bool everyoneReady = true;
        for (int i = 0; i < GameManager.Instance.players.Count; i++)
        {
            // If a slot isn't empty AND the ready visualElement is deactivated
            if (!playerSlots[i].empty && playerSlots[i].root.Q<VisualElement>("Ready").style.display == DisplayStyle.None) everyoneReady = false;
        }

        if (everyoneReady)
        {
            root.Q<Label>("CountdownLabel").style.display = DisplayStyle.Flex;
            countdown -= Time.deltaTime;

            root.Q<Label>("CountdownLabel").text = "Starting in " + (int)(countdown + 1);

            if (countdown <= 0) UIManager.Instance.ChangeScreen(UIManager.ScreenID.LevelSelect);
        }
        if (!everyoneReady)
        {
            root.Q<Label>("CountdownLabel").style.display = DisplayStyle.None;
            countdown = maxCountdown;
        }
    }
}
