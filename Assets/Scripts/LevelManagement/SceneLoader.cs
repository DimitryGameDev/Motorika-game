using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private int levelID;
    public void LoadScene()
    {
        SceneManager.LoadScene(levelID);
    }
}
