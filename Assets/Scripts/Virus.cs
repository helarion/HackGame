using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : MonoBehaviour {

    public Element position;

    // variables virus
    private int id; // identifiant du virus en nombre entier
    private int level; // niveau actuel du virus
    private int type; // type du virus selon les identifiants entiers
    private string stringType; // nom en String correspondant à l'entier du type du virus
    private bool outOfActions; // Booleen indiquant si le virus est à court d'action ou non
    private bool hasExplored; // Booleen indiquant si le virus a exploré ce tour-ci ou non
    private int countAnalyse; // Compteur du nombre de fois où le virus a analysé ce tour-ci
    private double currentLevelUpPrice; // prix actuel du level up du virus selon son niveau actuel
   // private int countLevelUp; 

    // Constantes identifiants de types de virus
    private const int basic = 0;
    private const int analyse = 1;
    private const int scan = 2;
    private const int fort = 3;
    private const int fake = 4;

    public double CurrentLevelUpPrice
    {
        get
        {
            return CurrentLevelUpPrice1;
        }

        set
        {
            CurrentLevelUpPrice1 = value;
        }
    }

    public int Id
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }

    public int Level
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
        }
    }

    public int Type
    {
        get
        {
            return type;
        }

        set
        {
            type = value;
        }
    }

    public string StringType
    {
        get
        {
            return stringType;
        }

        set
        {
            stringType = value;
        }
    }

    public bool OutOfActions
    {
        get
        {
            return outOfActions;
        }

        set
        {
            outOfActions = value;
        }
    }

    public bool HasExplored
    {
        get
        {
            return hasExplored;
        }

        set
        {
            hasExplored = value;
        }
    }

    public int CountAnalyse
    {
        get
        {
            return countAnalyse;
        }

        set
        {
            countAnalyse = value;
        }
    }

    public double CurrentLevelUpPrice1
    {
        get
        {
            return currentLevelUpPrice;
        }

        set
        {
            currentLevelUpPrice = value;
        }
    }

    public Element Element
    {
        get
        {
            return position;
        }
    }

    // Use this for initialization
    void Start () {
        Level = 0;
        Type = basic;
        setStringType();
        OutOfActions = false;
        HasExplored = false;
        CountAnalyse = 0;
        CurrentLevelUpPrice = GameManager.levelUpPrice;
        position = GetComponentInParent<Element>();
        //countLevelUp = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    /*
    public Element getElement()
    {
        return position;
    }

    public int getType()
    {
        return type;
    }

    public int getLevel()
    {
        return level;
    }

    public int getId()
    {
        return id;
    }

    public string getStringType()
    {
        return stringType;
    }

    public int getCountAnalyse()
    {
        return countAnalyse;
    }

    public bool getOutOfAction()
    {
        return outOfActions;
    }

    public bool getHasExplored()
    {
        return hasExplored;
    }*/

    public bool compare(Virus v)
    {
        if (Id == v.Id)
        {
            return true;
        }

        return false;
    }

    public void setElement(Element e)
    {
        position = e;
    }

    public void setCountAnalyse(int c)
    {
        CountAnalyse = c;
    }

    public void setOutOfAction(bool b)
    {
        OutOfActions = b;
    }

    public void setHasExplored(bool b)
    {
        HasExplored = b;
    }

    public void setLevel(int l)
    {
        Level = l;
    }

    public void setType(int t)
    {
        Type = t;
        setStringType();
    }

    public void setStringType()
    {
        if (Type == scan) StringType = "Scanner";
        else if (Type == fort) StringType = "Renforcer";
        else if (Type == fake) StringType = "Leurre";
        else if (Type == analyse) StringType = "Analiseur";
    }

    public void updatePosition()
    {
        this.GetComponentInParent<Transform>().transform.localPosition = Vector3.zero;
    }
}
