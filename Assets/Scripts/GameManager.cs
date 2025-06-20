using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI Score;

    private void Start()
    {
        Score.text = "0 : 0";
    }

    public void UpdateScore(int score1player, int score2player)
    {
        Score.text = $"{score1player} : {score2player}";
    }
    public void Exit()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
