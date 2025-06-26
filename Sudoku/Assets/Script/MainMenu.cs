using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void loadScene(string name)
   {
        SceneManager.LoadScene(name);
   }
}
