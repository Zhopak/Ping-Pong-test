using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActions : MonoBehaviour
{
    public void OpenPVP()
    {
        SceneManager.LoadScene(3);
    }

    public void OpenBOT()
    {
        SceneManager.LoadScene(2);
    }

    public void OpenWALL()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
