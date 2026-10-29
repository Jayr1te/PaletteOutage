using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadScene(3);
    }

    public void optionsList()
    {
        SceneManager.LoadScene(1);
    }

    public void creditsList()
    {
        SceneManager.LoadScene(2);
    }

    public void backToMain()
    {
        SceneManager.LoadScene(0);
    }
}
