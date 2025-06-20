using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public enum PlayerType { Player1, Player2, Bot }
    public PlayerType playerType;
    public Transform ball; // ������ �� ���

    [Header("��������� ����")]
    public float reactionDelay = 0.3f; // �������� �������
    public float predictionOffset = 0.5f; // ������������ �������� ����

    private float lastMoveTime;

    void Update()
    {
        // ��������� ����� ��� ������ 1 (W/S)
        if (playerType == PlayerType.Player1)
        {
            if (Input.GetKeyDown(KeyCode.W) && transform.position.y < 4)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + 1);
            }
            else if (Input.GetKeyDown(KeyCode.S) && transform.position.y > -4)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - 1);
            }
        }
        // ��������� ����� ��� ������ 2 (�������)
        else if (playerType == PlayerType.Player2)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && transform.position.y < 4)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + 1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && transform.position.y > -4)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - 1);
            }
        }
        else if(playerType == PlayerType.Bot)
        {
            if (Time.time - lastMoveTime < reactionDelay) return;

            // ������������� ������� Y-������� ����
            float predictedY = ball.position.y + (ball.GetComponent<Rigidbody2D>().linearVelocity.y * predictionOffset);

            // ���� ��� �������� � ������� ����
            bool ballComing = (playerType == PlayerType.Bot && ball.GetComponent<Rigidbody2D>().linearVelocity.x > 0) ||
                             (playerType == PlayerType.Player1 && ball.GetComponent<Rigidbody2D>().linearVelocity.x < 0);

            if (ballComing)
            {
                // ������������� �� ������������� ������� ����
                if (predictedY > transform.position.y + 0.1f && transform.position.y < 4)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y + 1);
                    lastMoveTime = Time.time;
                }
                else if (predictedY < transform.position.y - 0.1f && transform.position.y > -4)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y - 1);
                    lastMoveTime = Time.time;
                }
            }
            else
            {
                // ������������ � �����, ���� ��� �� ����� � ����
                if (transform.position.y > 0.1f)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y - 1);
                    lastMoveTime = Time.time;
                }
                else if (transform.position.y < -0.1f)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y + 1);
                    lastMoveTime = Time.time;
                }
            }
        }
    }
}
