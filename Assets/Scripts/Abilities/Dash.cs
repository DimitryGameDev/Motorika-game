using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{


    [SerializeField] private float dashForce = 10f; // Сила рывка
    private KeyCode dashKey = KeyCode.D; // Клавиша для рывка

    private Rigidbody rb;
    private bool isGrounded;
    [SerializeField] private float raycastDistanceForward = 1.5f; // Raycast distance from player to value;
    [SerializeField] private float raycastDistanceDown = 1.5f; // Raycast distance from player to value;
    [SerializeField] private float rayPositionTop = 0.5f; // Start position Ray on Top 
    [SerializeField] private float rayPositionBot = 0.5f; // Start position Ray on Bottom 
    private Vector3 raycastDownPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckGround();

        if (!isGrounded && Input.GetKeyDown(dashKey))
        {
            PerformDash();
        }
    }

    private void CheckGround()
    {
        raycastDownPosition = new(transform.position.x, transform.position.y + rayPositionBot, transform.position.z);

        isGrounded = Physics.Raycast(raycastDownPosition, -transform.up, out _, raycastDistanceDown);
    }

    private void PerformDash()
    {
        // Применение силы рывка по оси Z
        if (isGrounded) return;
        Vector3 dashDirection = transform.forward * dashForce;
        rb.AddForce(dashDirection, ForceMode.Impulse);
    }
}
