using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private bool showGizmos;

    [SerializeField] private float startPos;
    [SerializeField] private float endPos;

    private bool started;

    private float startCountdown = 3;
    void OnEnable()
    {
        started = false;
    }


    void Update()
    {
        if (!started)
        {
            if (Input.anyKey) StartLevel();
            return;
        }

        if(startCountdown < 0)
        {
            GameManager.Instance.UnlockPlayerMovement();
        }
        startCountdown -= Time.deltaTime;
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
