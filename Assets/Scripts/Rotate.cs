using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Vector3 direction;

    private void Update()
    {
        transform.Rotate(direction, rotateSpeed * Time.deltaTime);
    }
}