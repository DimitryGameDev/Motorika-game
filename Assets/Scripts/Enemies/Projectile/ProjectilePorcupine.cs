using UnityEngine;

public class ProjectilePorcupine : MonoBehaviour
{
    [SerializeField] private float lifetime;        
    [SerializeField] protected int damage;         

    private float speed;                            

    private float timer;                            // ������ ��� ������������ ������� ����� �������
    private Vector3 direction;                      // ����������� �������� �������
    private Destructable parent;                    // ������, ������� �������� ������ (��������)

    // ��������� ����� ��� ��������� �������� �������
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection;
        // ������������� ��������� ����������� �������
        transform.forward = direction;
    }

    private void Update()
    {
        RaycastHit hit;

        // ����� ���� �� ������ �����
        float stepLength = Time.deltaTime * speed;
        Vector3 step = direction * stepLength;

        // ��������� ����������� ���� ��� �������
        Debug.DrawRay(transform.position, direction * stepLength, Color.green);

        // �������� ������������ � �������� �� ���� �������� �������
        if (Physics.Raycast(transform.position, direction, out hit, stepLength))
        {
            OnHit(hit);
            OnProjectileLifeEnd(hit.collider, hit.point);
        }

        // ���������� ������� ������� �����
        timer += Time.deltaTime;

        // ����������� ������� ����� ��������� ������� �����
        if (timer > lifetime)
            Destroy(gameObject);

        // ����������� ������� � ��������� �����������
        transform.position += step;
    }

    // ����� ��������� ��������� ������� � ������
    protected virtual void OnHit(RaycastHit hit)
    {
        var destructible = hit.collider.transform.root.GetComponent<Destructable>();

        // ��������� ����� �������, ���� �� ����� ��������� Destructable � �� �������� "���������" �������
        if (destructible != null && destructible != parent)
        {
            destructible.ApplyDamage(damage);
        }
    }

    // ����� ��������� ����� ����� ������� (��������, ��� ������������)
    private void OnProjectileLifeEnd(Collider collider, Vector3 pos)
    {
        Destroy(gameObject);
    }

    // ����� ��������� ������������� ������� (���������), ������� �������� ������
    public void SetParentShooter(Destructable parent)
    {
        this.parent = parent;
    }
}
