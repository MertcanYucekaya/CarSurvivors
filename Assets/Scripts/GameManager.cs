using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor.SearchService;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    private bool isGame;
    [Header("CoinElements")]
    [SerializeField] private int nextLevelForCoin;
    [SerializeField] private int nextLevelForCoinPlus;
    [SerializeField] private Image coinFilledImage;
    private int coinAmount;
    [Header("VaveElements")]
    [SerializeField] private TextMeshProUGUI currentVaveText;
    private int currentVave;
    [Header("LevelElements")]
    [SerializeField] private TextMeshProUGUI currentLevelText;
    [Header("Skiils")]
    [SerializeField] private GameObject skillsPanel;
    [SerializeField] private SkillsSc skillsSc;
    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private int duration;
    private int remainingDuration;
    [Header("WinAndLose")]
    [SerializeField] private GameObject gameInfoPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI loseText;
   
    [Header("References")]
    [SerializeField] private CarController carController;
    [SerializeField] private GameObject playerCar;
    [SerializeField] private CanvasEnverionmentSc canvasEnverionmentSc;
    [SerializeField] private SceneAsset[] gameScenes;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private GameObject tutorialPanel;
    private void Start()
    {
        isGame = true;
        currentLevelText.text =(PlayerPrefs.GetInt("currentLevel")+1).ToString();
        currentVave = 1;
        coinAmount = 0;
        currentVaveText.text =currentVave.ToString();
    }
    public void startGame()
    {
        tutorialPanel.SetActive(false);
        being(duration);
        carController.carControlForStart();
        enemySpawner.enemySpawnerForStart();
    }
    public void setCoin()
    {
        coinAmount++;
        coinFilledImage.fillAmount = Mathf.InverseLerp(0, nextLevelForCoin, coinAmount);
        if (coinAmount >= nextLevelForCoin)
        {
            Time.timeScale = 0;
            coinAmount = 0;
            nextLevelForCoin += nextLevelForCoinPlus;
            coinFilledImage.fillAmount = 0;
            skillsPanel.SetActive(true);
            skillsSc.skillEnabledMethod();
        }
    }

    public void setCurrentVave()
    {
        currentVave++;
        currentVaveText.text = currentVave.ToString();
    }
   
    #region Win
    public void winMethod()
    {
        gameInfoPanel.SetActive(false);
        skillsPanel.SetActive(false);
        carController.stopCar();
        Time.timeScale = .4f;
        nextLevelForCoin = int.MaxValue;
        Invoke("winPanelsSet", 1.5f);
    }
    void winPanelsSet()
    {
        canvasEnverionmentSc.winPanelAnimation();
        winPanel.SetActive(true);
        winText.text = "LEVEL " + (PlayerPrefs.GetInt("currentLevel")+1);
    }
    public void setCurrentLevel()
    {
        if (PlayerPrefs.GetInt("currentLevel") >= gameScenes.Length)
        {
            PlayerPrefs.SetInt("currentLevel", 0);
        }
        else
        {
            PlayerPrefs.SetInt("currentLevel", PlayerPrefs.GetInt("currentLevel"));
        }
        SceneManager.LoadScene(gameScenes[PlayerPrefs.GetInt("currentLevel")].name);
    }
    #endregion


    #region GameOver
    public void gameOverMethod()
    {
        gameInfoPanel.SetActive(false);
        skillsPanel.SetActive(false);
        Time.timeScale = .4f;
        nextLevelForCoin = int.MaxValue;
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            g.GetComponent<EnemyCar>().gameOverForEnemey();
        }
        Destroy(playerCar);
        Invoke("OverPanelsSet", 1.5f);
    }

    void OverPanelsSet()
    {
        canvasEnverionmentSc.gameOverPanelAnimation();
        gameOverPanel.SetActive(true);
        loseText.text = "LEVEL " + (PlayerPrefs.GetInt("currentLevel")+1);
    }
    public void tryAgainButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion


    #region Timer
    private void being(int Second)
    {
        remainingDuration = Second;
        StartCoroutine(updateTimer());
    }

    private IEnumerator updateTimer()
    {
        while (remainingDuration >= 0)
        {
            timerText.text = $"{remainingDuration / 60:00}:{remainingDuration % 60:00}";
            remainingDuration--;
            yield return new WaitForSeconds(1f);
            yield return null;
        }
        OnEnd();
    }

    private void OnEnd()
    {
        if (isGame)
        {
            gameOverMethod();
        } 
    }
    #endregion
}
