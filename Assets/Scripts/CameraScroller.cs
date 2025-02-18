using UnityEngine;

public class CameraScroller : MonoBehaviour
{
    [SerializeField] private Vector3 offset;

    void OnEnable()
    {
        
    }

    void Update()
    {
        /* Logic for following first player
        Player first = GetFirstPlayer();
        transform.position = new Vector3 (first.transform.position.x + offset.x, offset.y, offset.z);
        // */

        // /* Logic for following the average position of all player, but with the applied offset
        transform.position = new Vector3(GetAveragePlayerX() + offset.x, offset.y, offset.z);
        // */

    }

    private Player GetFirstPlayer()
    {
        if (GameManager.Instance.players.Count == 0) return null;

        Player first = GameManager.Instance.players[0].player;

        for(int i = 0; i < GameManager.Instance.players.Count; i++)
        {
            if (GameManager.Instance.players[i].player.transform.position.x > first.transform.position.x) first = GameManager.Instance.players[i].player;
        }
        return first;
    }
    private float GetAveragePlayerX()
    {
        if (GameManager.Instance == null || GameManager.Instance.players.Count == 0) return 0;

        float allPos = 0;
        for (int i = 0; i < GameManager.Instance.players.Count; i++)
        {
            allPos += GameManager.Instance.players[i].player.transform.position.x;
        }
        return allPos / GameManager.Instance.players.Count;
    }
}

//