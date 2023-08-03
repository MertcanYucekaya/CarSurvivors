using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;



public class FristSceneSc : MonoBehaviour
{
    [SerializeField] private SceneAsset[] gameScenes;
    private void Start()
    {
        if (!PlayerPrefs.HasKey("currentLevel"))
        {
            PlayerPrefs.SetInt("currentLevel", 0);
        }
        SceneManager.LoadScene(gameScenes[PlayerPrefs.GetInt("currentLevel")].name);
    }
}
