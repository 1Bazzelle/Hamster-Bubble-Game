using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private bool showGizmos;

    [SerializeField] private float startPos;
    [SerializeField] private float endPos;

    [SerializeField] private List<Transform> playerStartPos;

    [SerializeField] private BoxCollider finishLine;
    public Sprite levelImage;

    private bool started;
    private bool countdownOver;

    private float initialCooldown;
    private float startCountdown;
    void OnEnable()
    {
        started = false;
        countdownOver = false;
        startCountdown = 0.5f;
        initialCooldown = 0.5f;

        // Spawn Players at their positions and lock their movement
        for (int i = 0; i < GameManager.Instance.players.Count; i++)
        {
            GameManager.Instance.players[i].player.transform.position = playerStartPos[i].position;
            GameManager.Instance.players[i].player.gameObject.SetActive(true);
        }
        GameManager.Instance.LockPlayerMovement();
    }


    void Update()
    {
        if (initialCooldown > 0)
        {
            initialCooldown -= Time.deltaTime;
            return;
        }
        if (!started)
        {
            if (Input.anyKeyDown)
            {
                Debug.Log("any key pressed");
                StartLevel();
            }
            return;
        }

        if(startCountdown < 0 && !countdownOver)
        {
            Debug.Log("GOOOOOOOOOOO");
            countdownOver = true;

            UIManager.Instance.countdown = -1;

            GameManager.Instance.UnlockPlayerMovement();
        }
        if (startCountdown > 0)
        {
            startCountdown -= Time.deltaTime;
            UIManager.Instance.countdown = startCountdown;
        }
       
    }

    private void StartLevel()
    {
        started = true;
        startCountdown = 3;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Gizmos.color = new Color(0, 1, 0, 0.25f);
        Gizmos.DrawCube(new Vector3(startPos, 0, 0), new Vector3(1, 20, 10));

        Gizmos.color = new Color(1, 0, 0, 0.25f);
        Gizmos.DrawCube(new Vector3(endPos, 0, 0), new Vector3(1, 20, 10));
    }
}

//