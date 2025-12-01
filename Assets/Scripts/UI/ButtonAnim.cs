using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLogic : MonoBehaviour
{
    [SerializeField] private string targetScene;

    public void Exit()
    {
        Application.Quit();
    }

    public void ChangeScene()
    {
        if (!string.IsNullOrEmpty(targetScene))
        {
            EnemyLimitUI.GamePaused = false;
            SceneManager.LoadScene(targetScene);
        }
    }
}
