using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject UpgradePanel;
    public void StartGame()
    {
        SceneManager.LoadScene("Stage_Scene"); // ➜ remplace "GameScene" par le nom de ta scène de jeu
    }

    public void OpenShop()
    {
        UpgradePanel.SetActive(true);
        // ou change de scène si tu veux une boutique séparée
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll(); // ⚠️ Attention ça efface TOUT
        Debug.Log("Progression réinitialisée !");
    }

    public void BackMenu()
    {
        UpgradePanel?.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
