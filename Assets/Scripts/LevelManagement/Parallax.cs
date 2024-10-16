using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float parallax;

    private float startPosX;
    private float startPosY;
    private float startPosZ;

    private void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        startPosZ = transform.position.z;
    }

    private void Update()
    {
        float distX = (transform.position.x * (1 - parallax));
        float distY = (transform.position.y * (1 - parallax));

        transform.position = new Vector3(startPosX+distX, startPosY+distY, startPosZ);
    }
}
