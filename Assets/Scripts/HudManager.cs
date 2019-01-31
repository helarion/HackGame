using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudManager : MonoBehaviour {

    public Canvas mainCanvas;
    public Canvas actionCanvas;
    public Canvas upgradeCanvas;
    public Button buttonSelect;
    public Button buttonExplore;
    public Button buttonScan;
    public Button buttonAnalyse;
    public Button buttonFort;
    public Button buttonFake;
    
    public GameObject textResources;
    public GameObject textWarning;
    public GameObject textWarningLevel;
    public Button buttonUpgrade;
    public Button buttonLevel;
    public GameObject textCurrentLevel;
    public GameObject textCurrentLevelCost;
    public GameObject textTitle;
    public GameObject fonctionBuy;
    public GameObject textCurrentFunction;
    public GameObject textScanPrice;
    public GameObject textFortPrice;
    public GameObject textFakePrice;
    public GameObject textAnalysePrice;
    public GameObject textCurrentTurn;
    public GameObject textMaxTurn;
    public GameObject textOutOfActions;
    public GameObject textOutOfExploration;
    public GameObject textCurrentWarning;

    private GameObject test;

    public Button nextNiveauButton;

    // Use this for initialization
    void Start () {
        ResetHud();
    }

    public void ResetHud()
    {
        actionCanvas.gameObject.SetActive(false);
        buttonExplore.gameObject.SetActive(false);
        buttonSelect.gameObject.SetActive(false);
        buttonFort.gameObject.SetActive(false);
        buttonScan.gameObject.SetActive(false);
        buttonFake.gameObject.SetActive(false);
        buttonAnalyse.gameObject.SetActive(false);
        textWarning.gameObject.SetActive(false);
        buttonUpgrade.gameObject.SetActive(false);
        upgradeCanvas.gameObject.SetActive(false);
        textCurrentFunction.gameObject.SetActive(false);
        textOutOfActions.gameObject.SetActive(false);
        textOutOfExploration.gameObject.SetActive(false);

        textCurrentLevelCost.GetComponent<TextMeshPro>().text = GameManager.levelUpPrice.ToString();
        textAnalysePrice.GetComponent<TextMeshPro>().text = GameManager.analysePrice.ToString();
        textFortPrice.GetComponent<TextMeshPro>().text = GameManager.fortPrice.ToString();
        textScanPrice.GetComponent<TextMeshPro>().text = GameManager.scanPrice.ToString();
        textFakePrice.GetComponent<TextMeshPro>().text = GameManager.fakePrice.ToString();

        actionCanvas.transform.localScale *= 2;
        upgradeCanvas.transform.localScale *= 2;

        textCurrentWarning.GetComponent<TextMeshPro>().text = GameManager.instance.CurrentWarning.ToString();
        textCurrentTurn.GetComponent<TextMeshPro>().text = GameManager.instance.CurrentTurn.ToString();
        textMaxTurn.GetComponent<TextMeshPro>().text = GameManager.maxTurn.ToString();
    }

    public void closeActionCanvas()
    {
        actionCanvas.gameObject.SetActive(false);
    }

    public void closeUpgradeCanvas()
    {
        upgradeCanvas.gameObject.SetActive(false);
    }

}
