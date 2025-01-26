using System.Runtime.CompilerServices;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
                if (player.dashCharges < player.GetMoveData().maxDashCharges)
                {
                    player.AddCharge();
                }
                PopSelf();
            }
        }
    }

    private void PopSelf()
    {
        Destroy(gameObject);
    }
}
