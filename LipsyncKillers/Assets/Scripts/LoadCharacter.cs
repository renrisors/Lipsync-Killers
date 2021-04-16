using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCharacter : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public GameObject[] animationsEnemyPrefabs;
    public Transform spawnPoint;
    public Transform enemySpawnPoint;
    void Start()
    {


        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
        GameObject prefabPlayer = characterPrefabs[selectedCharacter];
        GameObject prefabEnemy;
        if (selectedCharacter == 0)
        {
            prefabEnemy = animationsEnemyPrefabs[1];
        }
        else
        {
            prefabEnemy = animationsEnemyPrefabs[0];
        }
        GameObject player = Instantiate(prefabPlayer, spawnPoint.position, Quaternion.identity);
        GameObject enemy = Instantiate(prefabEnemy, enemySpawnPoint.position, Quaternion.identity);

        BGMusic.Instance.gameObject.GetComponent<AudioSource>().Stop();
    }
}
