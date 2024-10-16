using UnityEngine;

public enum State
{
    rightUp,
    leftUp,
    rightDown,
    leftDown,
    attack
}

public class NewBeetle : Enemy
{
    [SerializeField] private Transform platform;

    private Vector3 rightUpPosition;
    private Vector3 rightDownPosition;
    private Vector3 leftUpPosition;
    private Vector3 leftDownPosition;

    private State state;
    private Destructible beetleDestructible;
    
    private void Start()
    {
        beetleDestructible = GetComponent<Destructible>();

        if (platform != null)
        {
            rightUpPosition = new Vector3(platform.transform.position.x, platform.transform.position.y + platform.localScale.y / 2 + transform.localScale.y / 2, platform.transform.position.z + platform.localScale.z / 2 + transform.localScale.z / 2);
            rightDownPosition = new Vector3(platform.transform.position.x, platform.transform.position.y - platform.localScale.y / 2 - transform.localScale.y / 2, platform.transform.position.z + platform.localScale.z / 2 + transform.localScale.z / 2);
            leftUpPosition = new Vector3(platform.transform.position.x, platform.transform.position.y + platform.localScale.y / 2 + transform.localScale.y / 2, platform.transform.position.z - platform.localScale.z / 2 - transform.localScale.z / 2);
            leftDownPosition = new Vector3(platform.transform.position.x, platform.transform.position.y - platform.localScale.y / 2 - transform.localScale.y / 2, platform.transform.position.z - platform.localScale.z / 2 - transform.localScale.z / 2);

            transform.position = rightUpPosition;
            state = State.rightUp;
        }
    }

    private void Update()
    {
        MoveToNextCorner();
    }

    private void MoveToNextCorner()
    {
        switch (state)
        {
            case State.rightUp:
                MoveTowards(leftUpPosition);
                
                if (IsAtPosition(leftUpPosition))
                {
                    Rotation(-1);
                    state = State.leftUp;
                }
                break;
            case State.leftUp:
                MoveTowards(leftDownPosition);

                if (IsAtPosition(leftDownPosition))
                {
                    Rotation(-1);
                    state = State.leftDown;
                }
                break;
            case State.leftDown:
                MoveTowards(rightDownPosition);

                if (IsAtPosition(rightDownPosition))
                {
                    Rotation(-1);
                    state = State.rightDown;
                }
                break;
            case State.rightDown:
                MoveTowards(rightUpPosition);

                if (IsAtPosition(rightUpPosition))
                {
                    Rotation(-1);
                    state = State.rightUp;
                }
                break;

            case State.attack:
                Attack();
                break;
        }
    }

    private void MoveTowards(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, MoveSpeed * Time.deltaTime);
    }

    private void Rotation(float sing)
    {
        transform.Rotate(Vector3.right * 90f *sing, Space.Self);
    }

    private bool IsAtPosition(Vector3 target)
    {
        return Vector3.Distance(transform.position, target) < 0.1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.root.GetComponent<Player>() != null)
        {
            var player = collision.transform.root.GetComponent<Player>();
            var playerDestructible = collision.transform.root.GetComponent<Destructible>();

            if (playerDestructible != null)
            {
                if (!player.IsGround() || player.isSlide || player.transform.position.y >= transform.position.y)
                {
                    beetleDestructible.ApplyDamage(beetleDestructible.HitPoints);
                }
                else
                    playerDestructible.ApplyDamage(Damage);
            }

            state = State.attack;
        }
    }

    private void Attack()
    {
        //TODO Add sound
    }
}