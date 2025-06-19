using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public enum PlayerType { Player1, Player2 }
    public PlayerType playerType;

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
    }
}
