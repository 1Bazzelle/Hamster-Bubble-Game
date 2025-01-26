using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class ILevel : IScreen
{
    private struct PlayerProfile
    {
        public Player playerScript;

        public VisualElement root;

        public bool active;

        private List<VisualElement> bubbles;
        public PlayerProfile(VisualElement rot)
        {
            playerScript = null;
            active = false;

            root = rot;

            bubbles = new();

            VisualElement bubbleContainer = root.Q<VisualElement>("BubbleContainer");

            if (bubbleContainer != null)
            {
                foreach (VisualElement child in bubbleContainer.Children())
                {
                    if (child.name == "Bubble")
                    {
                        bubbles.Add(child);
                    }
                }
            }
            else
            {
                Debug.LogWarning("BubbleContainer not found in the provided root element.");
            }
        }
        public void UpdateBubbleCount()
        {
            int bubbleNum = playerScript.dashCharges;

            Debug.Log("new batch");
            for (int i = 0; i < bubbles.Count; i++)
            {
                if(i < bubbleNum)
                {
                    Debug.Log("Added Bubble");
                    bubbles[i].Q<VisualElement>("Active").style.display = DisplayStyle.Flex;
                    bubbles[i].Q<VisualElement>("Inactive").style.display = DisplayStyle.None;
                }
                else
                {
                    Debug.Log("Added Empty");
                    bubbles[i].Q<VisualElement>("Active").style.display = DisplayStyle.None;
                    bubbles[i].Q<VisualElement>("Inactive").style.display = DisplayStyle.Flex;
                }
            }
        }

        public void HidePlayerProfile()
        {
            root.style.display = DisplayStyle.None;
        }
        public void ShowPlayerProfile()
        {
            root.style.display = DisplayStyle.Flex;
        }
    }
    private VisualElement root;
    private VisualElement screenElements;

    private bool countdownOver = false;
    private float goTextTimer = 3;

    private List<PlayerProfile> players;

    private int playerCount;
    public void Initialize(VisualTreeAsset tree, VisualElement root)
    {
        screenElements = tree.Instantiate();
        root.Add(screenElements);

        this.root = root;

        root.Q<Label>("CountdownLabel").style.display = DisplayStyle.None;
        countdownOver = false;
        goTextTimer = 3;

        players = new()
        {
            new PlayerProfile(root.Q<VisualElement>("Player1")),
            new PlayerProfile(root.Q<VisualElement>("Player2")),
            new PlayerProfile(root.Q<VisualElement>("Player3")),
            new PlayerProfile(root.Q<VisualElement>("Player4"))
        };

        playerCount = GameManager.Instance.players.Count;

        UpdatePlayerProfiles();

        root.Q<Label>("WinnerLabel").style.display = DisplayStyle.None;
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

        // CODE GETS HERE
        for (int i = 0; i < playerCount; i++)
        {
            if (players[i].active) players[i].UpdateBubbleCount();
        }

        if(GameManager.Instance.game.hasWinner)
        {
            root.Q<Label>("WinnerLabel").style.display = DisplayStyle.Flex;
            root.Q<Label>("WinnerLabel").text = "THE WINNER IS PLAYER " + GameManager.Instance.game.winner.playerIndex;
        }
    }

    private void UpdatePlayerProfiles()
    {
        for (int i = 0; i < GameManager.Instance.players.Count; i++)
        {
            if (i < GameManager.Instance.players.Count)
            {
                PlayerProfile temp = players[i];

                temp.active = true;
                temp.playerScript = GameManager.Instance.players[i].player;

                players[i].ShowPlayerProfile();
            }
            else
            {
                players[i].HidePlayerProfile();
            }
        }
    }
}
