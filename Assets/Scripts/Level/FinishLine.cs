using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private Level myLevel;
    void Start()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if ((Constants.LAYER_PLAYER.value & (1 << other.gameObject.layer)) != 0)
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player == null)
            {
                Debug.LogWarning("Player Script not found");
                return;
            }
            else
            {
                GameManager.Instance.OnPlayerFinish(player);
            }
        }
    }
}
