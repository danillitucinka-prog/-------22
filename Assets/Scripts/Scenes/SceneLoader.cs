using UnityEngine.SceneManagement;

namespace LoxQuest3D.Scenes
{
    public static class SceneLoader
    {
        public static void LoadMenu() => SceneManager.LoadScene(SceneIds.MainMenu);
        public static void LoadCity() => SceneManager.LoadScene(SceneIds.City);
    }
}

