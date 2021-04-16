using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public int selectedCharacter = 0;
    public GameObject[] characters;

    public void SelectCharacter(string nome)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i].name == nome) {
                selectedCharacter = i;
                PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
                Debug.Log("Personagem passado:" + characters[i].name);
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

}
