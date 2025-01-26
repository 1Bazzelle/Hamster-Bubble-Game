using UnityEngine;

public class Turbine : MonoBehaviour
{
    [SerializeField] private bool clockwise;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private Transform hub;
    private Vector3 initOffset;
    void OnEnable()
    {
        initOffset = hub.position - transform.position;
    }

    void Update()
    {
        hub.position = transform.position + initOffset;

        if(clockwise)
            hub.localRotation = Quaternion.Euler(0, 0, hub.localRotation.eulerAngles.z + rotationSpeed);
        if(!clockwise)
            hub.localRotation = Quaternion.Euler(0, 0, hub.localRotation.eulerAngles.z - rotationSpeed);
    }
}
