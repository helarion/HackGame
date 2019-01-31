using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    // instance du gamemanager en singleton
    public static GameManager instance;

    // HudManager
    private HudManager hudManager;

    // constantes de gameplay

        //int constants permettant d'identifier les types de virus
        private const int basic = 0;
        private const int analyse = 1;
        private const int scan = 2;
        private const int fort = 3;
        private const int fake = 4;
        
        // niveau max constant d'un virus
        private const int maxLevel = 5;
        
        // numéro max de tours dans un niveau (à voir si on ne le met pas plutot 
        // adapté au niveau en question...
        public const int maxTurn = 10;

        // prix des virus
        public const int levelUpPrice = 500; // upgrade un virus
        public const int newVirusPrice = 1000; // acheter un nouveau virus
        public const int fortPrice = 250; // acheter la fonction fort
        public const int scanPrice = 150; // acheter la fonction scan
        public const int fakePrice = 125; // acheter la fonction leurre
        public const int analysePrice = 100; // acheter la fonction analyse
        public const int warningGain = 25; // JE SAIS PLUS CE QUE C'EST !!!
        private double currentVirusPrice; // prix actuel d'un virus selon le nombre de virus présents dans le niveau
        public double modifierLevelUpPrice = 1.5; // modificateur d'incrémentation du prix du level up de virus
        public double modifierVirusPrice = 2; // modificateur d'incrémentation du prix d'un virus

    // variables de gameplay

    // à placer dans le virus puisque dépendant de l'anvancement individuel de ceux ci


    private int currentTurn; // tour actuel
    private int currentWarning; // JE SAIS PLUS CE QUE C'EST !!!
    public double resources; // ressources actuelles du joueur
    private int detection; // detection actuelle du joueur

    // éléments classes
    public List<Virus> virusList; // liste des virus dans le niveau
    private Virus selectedVirus; // virus selectionné actuellement
    private Element selectedElement; // élement selectionné actuellement

    public Element SelectedElement
    {
        get
        {
            return selectedElement;
        }

        set
        {
            selectedElement = value;
        }
    }

    public int CurrentWarning
    {
        get
        {
            return currentWarning;
        }

        set
        {
            currentWarning = value;
        }
    }

    public int CurrentTurn
    {
        get
        {
            return currentTurn;
        }

        set
        {
            currentTurn = value;
        }
    }

    public HudManager HudManager
    {
        get
        {
            return hudManager;
        }
    }

    // Use this for initialization
    void Awake()
    {
        //on check si l'instance existe déja
        if(instance==null)
        {
            // si non : on l'attribue à celle en cours
            instance = this;
        }
        else if(instance!= this)
        {
            // trouver le HudManager
            instance.hudManager = FindObjectOfType<HudManager>();
            // s'il en existe déja une, on détruit cet objet car on en a besoin que d'un seul
            Destroy(gameObject);
        }

        // ne pas détruire lorsqu'on change de scène
        DontDestroyOnLoad(gameObject);

        // trouver le HudManager
        hudManager = FindObjectOfType<HudManager>();

        currentVirusPrice = newVirusPrice;
        CurrentWarning = 0;
        CurrentTurn = 1;

        GameObject[] tempList = GameObject.FindGameObjectsWithTag("Virus");
        for (int i = 0; i < tempList.Length; i++)
        {
            virusList.Add(tempList[i].GetComponent<Virus>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        HudManager.buttonUpgrade.gameObject.SetActive(false);
        enableVirusButton(basic, false);
        HudManager.textWarning.gameObject.SetActive(false);
        HudManager.textOutOfActions.gameObject.SetActive(false);
        HudManager.textOutOfExploration.gameObject.SetActive(false);
        // si aucun virus n'est séléctionné mais qu'un élément l'est
        if (selectedVirus == null && SelectedElement != null)
        {
            // on désactive le bouton d'exploration
            HudManager.buttonExplore.gameObject.SetActive(false);
            if (SelectedElement.ContainsVirus) // si l'élement selectionné contient un virus
            {
                HudManager.buttonSelect.gameObject.SetActive(true); // on active le bouton select virus
            }
            else HudManager.buttonSelect.gameObject.SetActive(false);
        }
        // si un virus est selectionné et qu'un élément est séléctionné
        else if (selectedVirus != null && SelectedElement != null)
        {
            // on désactive le bouton select
            HudManager.buttonSelect.gameObject.SetActive(false);
            HudManager.buttonExplore.gameObject.SetActive(false);
            // on vérifie si l'élément selectionné contient un virus
            if (SelectedElement.ContainsVirus)
            {
                // on désactive le bouton d'exploration
                HudManager.buttonExplore.gameObject.SetActive(false);
                // si ce dernier n'est pas le même que celui séléctionné on réactive le bouton select
                if (!SelectedElement.Virus.compare(selectedVirus))
                {
                    HudManager.buttonSelect.gameObject.SetActive(true);
                }
                else
                {
                    HudManager.buttonUpgrade.gameObject.SetActive(true);
                }
            }
            // si l'élement selectionné ne contiens pas de virus, on réactive le bouton d'exploration
            else if (SelectedElement.isNeighboorOfSelectedVirus() != -1)
            {
                if (!selectedVirus.OutOfActions)
                {
                    if (!selectedVirus.HasExplored) HudManager.buttonExplore.gameObject.SetActive(true);
                    else HudManager.textOutOfExploration.gameObject.SetActive(true);
                    if (!SelectedElement.IsAnalyzed && selectedVirus.Type == analyse)
                    {
                        enableVirusButton(selectedVirus.Type, true);
                    }
                    if (!SelectedElement.IsExplored && selectedVirus.Type == scan)
                    {
                        enableVirusButton(selectedVirus.Type, true);
                    }
                    if (!SelectedElement.IsFortified && selectedVirus.Type == fort && selectedVirus.Level >= 1)
                    {
                        enableVirusButton(selectedVirus.Type, true);
                    }
                    if (!SelectedElement.IsFaked && selectedVirus.Type == fake)
                    {
                        enableVirusButton(selectedVirus.Type, true);
                    }
                }
                else HudManager.textOutOfActions.gameObject.SetActive(true);
            }
        }

        if (HudManager.upgradeCanvas.gameObject.activeSelf)
        {
            HudManager.textCurrentLevel.GetComponentInChildren<TextMeshPro>().text = selectedVirus.Level.ToString();
            if (selectedVirus.Type == basic && selectedVirus.Level >= 1)
            {
                HudManager.fonctionBuy.gameObject.SetActive(true);
                HudManager.textCurrentFunction.gameObject.SetActive(false);
            }
            else
            {
                HudManager.fonctionBuy.gameObject.SetActive(false);
                HudManager.textCurrentFunction.gameObject.SetActive(true);
                HudManager.textCurrentFunction.GetComponent<TextMeshPro>().text = selectedVirus.StringType;
            }
        }

        if (SelectedElement != null)
        {
            HudManager.textTitle.GetComponent<TextMeshPro>().text = "test";//SelectedElement.Title;
            if (SelectedElement.IsAnalyzed)
            {
                HudManager.textWarningLevel.GetComponent<TextMeshPro>().text = SelectedElement.WarningLevel.ToString();
                HudManager.textWarning.gameObject.SetActive(true);
            }

        }

        HudManager.textResources.GetComponent<TextMeshPro>().text = "test";// resources.ToString();
    }

    public void enableVirusButton(int type, bool b)
    {
        if (type == basic)
        {
            HudManager.buttonAnalyse.gameObject.SetActive(false);
            HudManager.buttonScan.gameObject.SetActive(false);
            HudManager.buttonFake.gameObject.SetActive(false);
            HudManager.buttonFort.gameObject.SetActive(false);
        }
        else if (type == analyse)
        {
            HudManager.buttonAnalyse.gameObject.SetActive(b);
            HudManager.buttonScan.gameObject.SetActive(false);
            HudManager.buttonFake.gameObject.SetActive(false);
            HudManager.buttonFort.gameObject.SetActive(false);
        }
        else if (type == scan)
        {
            HudManager.buttonAnalyse.gameObject.SetActive(false);
            HudManager.buttonScan.gameObject.SetActive(b);
            HudManager.buttonFake.gameObject.SetActive(false);
            HudManager.buttonFort.gameObject.SetActive(false);
        }
        else if (type == fake)
        {
            HudManager.buttonAnalyse.gameObject.SetActive(false);
            HudManager.buttonScan.gameObject.SetActive(false);
            HudManager.buttonFake.gameObject.SetActive(b);
            HudManager.buttonFort.gameObject.SetActive(false);
        }
        if (type == fort)
        {
            HudManager.buttonAnalyse.gameObject.SetActive(false);
            HudManager.buttonScan.gameObject.SetActive(false);
            HudManager.buttonFake.gameObject.SetActive(false);
            HudManager.buttonFort.gameObject.SetActive(b);
        }
    }

    public void setSelectedElement(Element e)
    {
        SelectedElement = e;
    }

    public Element getSelectedElement()
    {
        return SelectedElement;
    }

    public Virus getSelectedVirus()
    {
        return selectedVirus;
    }

    public Element getSelectedVirusElement()
    {
        return selectedVirus.Element;
    }

    public void exploreSelectedElement()
    {
        if (SelectedElement.isNeighboorOfSelectedVirus() != -1)
        {
            // on lance l'exploration de l'élement
            SelectedElement.explore();
            // on définit le nouveau parent du virus qui a changé d'élement
            selectedVirus.transform.parent = SelectedElement.transform;
            // reset la position du virus pour le déplacer à l'origine de son nouveau parent
            //selectedVirus.GetComponentInParent<Transform>.transform.translate(0);
            // on change le nouvel élement du virus
            selectedVirus.setElement(SelectedElement);
            selectedVirus.updatePosition();
            selectedVirus.setHasExplored(true);
        }
    }

    public void analyseSelectedElement()
    {
        SelectedElement.analyse();
        selectedVirus.setCountAnalyse(selectedVirus.CountAnalyse + 1);
        if (selectedVirus.CountAnalyse >= selectedVirus.Level) selectedVirus.setOutOfAction(true);
    }

    public void scanSelectedElement()
    {
        SelectedElement.scan();
    }

    public void fortSelectedElement()
    {
        SelectedElement.fort(selectedVirus.Level);
    }

    public void fakeSelectedElement()
    {
        SelectedElement.fake(selectedVirus.Level);
    }

    public void Nextlevel()
    {
        Debug.Log("test");
        SceneManager.LoadScene(SceneManager.sceneCount + 1);
    }

    public void SelectVirus()
    {
        if (SelectedElement.ContainsVirus)
        {
            selectedVirus = SelectedElement.Virus;
        }
    }

    public void openUpgrades()
    {
        HudManager.upgradeCanvas.gameObject.SetActive(true);
    }

    public void levelUpgrade()
    {
        if (resources >= selectedVirus.CurrentLevelUpPrice)
        {
            selectedVirus.setLevel(selectedVirus.Level + 1);
            resources -= selectedVirus.CurrentLevelUpPrice;
            selectedVirus.CurrentLevelUpPrice *= modifierLevelUpPrice;
            HudManager.textCurrentLevelCost.GetComponent<TextMeshPro>().text = selectedVirus.CurrentLevelUpPrice.ToString();
            if (selectedVirus.Level >= maxLevel) HudManager.buttonLevel.gameObject.SetActive(false);
        }
    }

    public void chooseTypeScan()
    {
        if (resources >= scanPrice)
        {
            selectedVirus.setType(scan);
            resources -= scanPrice;
        }
    }

    public void chooseTypeFake()
    {
        if (resources >= fakePrice)
        {
            selectedVirus.setType(fake);
            resources -= fakePrice;
        }
    }

    public void chooseTypeFort()
    {
        if (resources >= fortPrice)
        {
            selectedVirus.setType(fort);
            resources -= fortPrice;
        }
    }

    public void chooseTypeAnalyse()
    {
        if (resources >= analysePrice)
        {
            selectedVirus.setType(analyse);
            resources -= analysePrice;
        }
    }

    public void endTurn()
    {
        if (CurrentTurn + 1 <= maxTurn)
        {
            CurrentTurn++;
            HudManager.textCurrentTurn.GetComponent<TextMeshPro>().text = CurrentTurn.ToString();
            for (int i = 0; i < virusList.Count; i++)
            {
                virusList[i].setOutOfAction(false);
                virusList[i].setHasExplored(false);
            }
        }
        else lose();
    }

    public void lose()
    {

    }

    public void win()
    {

    }
}