using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Element : MonoBehaviour {

    // Si c'est l'élement de départ
    public bool playerStart;
    // Si un virus est présent dans l'evennement
    private bool containsVirus;
    // Si l'élement est repéré
    private bool isDetected;
    //Si l'élement est exploré
    private bool isExplored;
    // Id de l'élement pour l'identifier
    private int id;

    private bool isAnalyzed;
    private bool isFortified;
    private bool isFaked;

    public string title;

    // halo
    private Behaviour halo;
    private CircleCollider2D hitbox;

    private SpriteRenderer sprite;

    //Game variables
    private int warningLevel;
    private int fortifiedLevel;
    private int fakeLevel;

    // liens
    private List<Link> links;
    private Link[] publicLinks;
    private List<Element> neighboors;
    private GameObject refLink;
    private int count;
    
    // liste des nodes sur l'élement
    private List<Transform> nodeList;

    // Dépendance pour ordre de start
    private static Element singleton = null;

    public bool PlayerStart
    {
        get
        {
            return playerStart;
        }

        set
        {
            playerStart = value;
        }
    }

    public bool ContainsVirus
    {
        get
        {
            return containsVirus;
        }

        set
        {
            containsVirus = value;
        }
    }

    public bool IsDetected
    {
        get
        {
            return isDetected;
        }

        set
        {
            isDetected = value;
        }
    }

    public bool IsExplored
    {
        get
        {
            return isExplored;
        }

        set
        {
            isExplored = value;
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

    public Virus Virus
    {
        get
        {
            return this.GetComponentInChildren<Virus>();
        }
    }

    public bool IsAnalyzed
    {
        get
        {
            return isAnalyzed;
        }

        set
        {
            isAnalyzed = value;
        }
    }

    public bool IsFortified
    {
        get
        {
            return isFortified;
        }

        set
        {
            isFortified = value;
        }
    }

    public bool IsFaked
    {
        get
        {
            return isFaked;
        }

        set
        {
            isFaked = value;
        }
    }

    public string Title
    {
        get
        {
            return title;
        }

        set
        {
            title = value;
        }
    }

    public int WarningLevel
    {
        get
        {
            return warningLevel;
        }

        set
        {
            warningLevel = value;
        }
    }

    // Use this for initialization
    void Start () {
        sprite = this.gameObject.GetComponent<SpriteRenderer>();
        links = new List<Link>();
        nodeList = new List<Transform>();

        IsDetected = false;
        IsExplored = false;
        ContainsVirus = false;
        IsAnalyzed = false;
        IsFortified = false;
        IsFaked = false;

        fortifiedLevel = 0;
        fakeLevel = 0;

        // Get le Halo
        halo = (Behaviour)GetComponent("Halo");
        hitbox = GetComponent<CircleCollider2D>();

        // Get les liens
        refLink = GameObject.FindWithTag("Links");
        publicLinks = GameObject.FindWithTag("Links").GetComponentsInChildren<Link>();
        count = publicLinks.Length;

        // Check si c'est l'élement de départ
        if (PlayerStart)
        {
            halo.enabled=true;
            ContainsVirus = true;
            IsDetected = true;
            IsExplored = true;
            IsAnalyzed = true;
        }

        // Analyse des liens pour trouver ceux qui concernent l'élément
        for (int i = 0; i < count; i++)
        {
            if (publicLinks[i].isLinked(this))
            {
                links.Add(publicLinks[i]);
               // neighboors.Add(publicLinks[i].getNeighboor(this));
            }
        }

        // si l'élement n'est pas detecté on desactive le render, sinon on l'active et on render les liens
        if (!IsDetected) sprite.enabled = false;
        else
        {
            sprite.enabled = true;
            renderLinks();
        }

        // nodes
        float spriteSize = sprite.bounds.size.x;

        // node top
        GameObject node1 = new GameObject();
        node1.name = "node1";
        Vector3 temp = new Vector3(0, spriteSize / 2, 0);
        node1.transform.position = this.transform.position;
        node1.transform.position += temp;
        node1.transform.parent = this.transform;
        nodeList.Add(node1.transform);

        // node haut droit
        GameObject node2 = new GameObject();
        node2.name = "node2";
        temp.Set(spriteSize / 2, spriteSize / 4, 0);
        node2.transform.position = this.transform.position;
        node2.transform.position += temp;
        node2.transform.parent = this.transform;
        nodeList.Add(node2.transform);

        // node  bas droit
        GameObject node3 = new GameObject();
        node3.name = "node3";
        temp.Set(spriteSize / 2, -spriteSize / 4, 0);
        node3.transform.position = this.transform.position;
        node3.transform.position += temp;
        node3.transform.parent = this.transform;
        nodeList.Add(node3.transform);

        // node bas
        GameObject node4 = new GameObject();
        node4.name = "node4";
        temp.Set(0, -spriteSize / 2, 0);
        node4.transform.position = this.transform.position;
        node4.transform.position += temp;
        node4.transform.parent = this.transform;
        nodeList.Add(node4.transform);

        // node  bas gauche
        GameObject node5 = new GameObject();
        node5.name = "node5";
        temp.Set(-spriteSize / 2, -spriteSize / 4, 0);
        node5.transform.position = this.transform.position;
        node5.transform.position += temp;
        node5.transform.parent = this.transform;
        nodeList.Add(node5.transform);

        // node haut droit
        GameObject node6 = new GameObject();
        node6.name = "node6";
        temp.Set(-spriteSize / 2, spriteSize / 4, 0);
        node6.transform.position = this.transform.position;
        node6.transform.position += temp;
        node6.transform.parent = this.transform;
        nodeList.Add(node6.transform);

        singleton = this;
        //GameManager.instance.HudManager.actionCanvas.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        /*if (Input.GetMouseButton(0))
        {
            Debug.Log("status:" + actionCanvas.activeSelf);
            actionCanvas.SetActive(false);
        }*/
	}

    void OnMouseDown()
    {
        if(IsDetected) // si l'élement est bien découvert
        {
            GameManager.instance.HudManager.actionCanvas.gameObject.SetActive(true);
            GameManager.instance.setSelectedElement(this);
            //actionCanvas.Find("TitreElement").GetComponent<Text>().text=titre;
        }
    }

    public bool IsReady()
    {
        return (null != singleton);
    }

    public List<Transform> getListNode()
    {
        return nodeList;
    }

    public void setContainsVirus(bool b)
    {
        ContainsVirus = b;
        halo.enabled = b;
    }

    public bool compare(Element e)
    {
        if (Id == e.Id) return true;
        else return false;
    }

    public void renderLinks()
    {
        for (int i = 0; i < links.Count; i++)
        {
            links[i].getNeighboor(this).detect();
            links[i].renderLink();
        }
    }

    public int isNeighboorOfSelectedVirus()
    {
        int r = -1;
        for (int i = 0; i < links.Count; i++)
        {
            if (links[i].getNeighboor(this).compare(GameManager.instance.getSelectedVirus().Element))  r = i;
            //if (neighboors[i].compare(player.getSelectedElement()))     r = i;
        }
        return r;
    }

    public void detect()
    {
        IsDetected = true;
        sprite.enabled = true; // ERREUR ICI A REVOIR PLUS TARD
    }

    public void explore()
    {
        int i = this.isNeighboorOfSelectedVirus();
        if (i != -1)
        {
            IsExplored = true;
            GameManager.instance.getSelectedVirusElement().setContainsVirus(false);
            setContainsVirus(true);
            renderLinks();
        }
    }

    public void analyse()
    {
        IsAnalyzed = true;
    }

    public void scan()
    {
        IsExplored = true;
        renderLinks();
    }

    public void fort(int level)
    {
        IsFortified = true;
        fortifiedLevel = level;
    }

    public void fake(int level)
    {
        IsFaked = true;
        fakeLevel = level;
    }
}
