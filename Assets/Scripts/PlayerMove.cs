using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public enum PlayerType { Player1, Player2, Bot }
    public PlayerType playerType;
    public Transform ball; // Ссылка на мяч

    [Header("Настройки бота")]
    public float reactionDelay = 0.3f; // Задержка реакции
    public float predictionOffset = 0.5f; // Предсказание движения мяча

    private float lastMoveTime;

    void Update()
    {
        // Обработка ввода для игрока 1 (W/S)
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
        // Обработка ввода для игрока 2 (Стрелки)
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

            // Предсказываем будущую Y-позицию мяча
            float predictedY = ball.position.y + (ball.GetComponent<Rigidbody2D>().linearVelocity.y * predictionOffset);

            // Если мяч движется в сторону бота
            bool ballComing = (playerType == PlayerType.Bot && ball.GetComponent<Rigidbody2D>().linearVelocity.x > 0) ||
                             (playerType == PlayerType.Player1 && ball.GetComponent<Rigidbody2D>().linearVelocity.x < 0);

            if (ballComing)
            {
                // Выравниваемся по предсказанной позиции мяча
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
                // Возвращаемся в центр, если мяч не летит к боту
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
