using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CanvasEnverionmentSc : MonoBehaviour
{

    [Header("GameOverEnv")]
    [SerializeField] private Transform gameOverPanel;
    [SerializeField] private float gameOverPanelAnimTime;
    [SerializeField] private Transform skullImage;
    [SerializeField] private float skullImageAnimTime;
    private Vector3 gameOverPanelStartScale;
    private Vector3 skullImageStartScale;

    [Header("WinEnv")]
    [SerializeField] private Transform winPanel;
    [SerializeField] private float winPanelAnimTime;
    [SerializeField] private Transform starImage;
    [SerializeField] private float starImageAnimTime;
    private Vector3 winPanelStartScale;
    private Vector3 starImageStartScale;


    void Start()
    {
        gameOverPanelStartScale = gameOverPanel.localScale;
        skullImageStartScale = skullImage.localScale;

        winPanelStartScale = winPanel.localScale;
        starImageStartScale = starImage.localScale;
        DOTween.Init();
    }
    public void gameOverPanelAnimation()
    {
        gameOverPanel.localScale = Vector3.zero;
        skullImage.localScale = Vector3.zero;
        gameOverPanel.DOScale(gameOverPanelStartScale, gameOverPanelAnimTime);
        skullImage.DOScale(skullImageStartScale, skullImageAnimTime);
    }
    public void winPanelAnimation()
    {
        winPanel.localScale = Vector3.zero;
        starImage.localScale = Vector3.zero;
        winPanel.DOScale(winPanelStartScale, winPanelAnimTime);
        starImage.DOScale(starImageStartScale, starImageAnimTime);
    }
}
