using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour {

    public Element e1;
    public Element e2;

    private Color color;
    private LineRenderer LD;

    // listes temporaires pour y mettre les nodes des élements
    private Transform[] l1;
    private Transform[] l2;

    // nodes les plus proches
    private Transform n1;
    private Transform n2;

    // Use this for initialization
    void Start () {
        LD = GetComponent<LineRenderer>();
        LD.SetWidth(0.1f,0.1f);
        LD.useWorldSpace = true;
        color.a = 1;

        color.r = 0;
        color.g = 1;
        color.b = 0;

        LD.SetColors(color, color);
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void renderLink()
    {
        if (areNodesDetected())
        {
            findClosestNodes();
            LD.SetPosition(0, n1.position);
            LD.SetPosition(1, n2.position);
        }
    }

    public bool areNodesDetected()
    {
        if (e1.IsDetected && e2.IsDetected) return true;
        return false;
    }

    public Element getElement1()
    {
        return e1;
    }

    public Element getElement2()
    {
        return e2;
    }

    public bool isLinked(Element e)
    {
        if (e.compare(e1) || e.compare(e2)) return true;
        return false;
    }

    public Element getNeighboor(Element e)
    {
        if (e1.compare(e)) return e2;
        return e1;
    }

    void findClosestNodes()
    {
        l1 = e1.GetComponentsInChildren<Transform>();
        l2 = e2.GetComponentsInChildren<Transform>();
        int minI = 0;
        int minJ = 0;
        
        //Debug.Log("l1 size=" + l1.Length);
        //Debug.Log("l2 size=" + l2.Length);
        
        float min = 2000;

        for (int i=1; i<l1.Length; i++)
        {
            for(int j=1; j<l2.Length; j++)
            {
                if(Vector2.Distance(l1[i].position, l2[j].position)<min)
                {
                    minI = i;
                    minJ = j;
                    min = Vector2.Distance(l1[i].position, l2[j].position);
                }
            }
        }
        //Debug.Log("distancemin = " + min);
        n1 = l1[minI].transform;
        n2 = l2[minJ].transform;
    }
}
