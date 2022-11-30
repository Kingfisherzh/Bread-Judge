using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public List<GameObject> items = new List<GameObject>();
    public GameObject g1t1;
    public GameObject g1t2;
    public GameObject g1t3;
    public GameObject g2t1;
    public GameObject g2t2;
    public GameObject g2t3;
    public GameObject g3t1;
    public GameObject g3t2;
    public GameObject g3t3;
    public GameObject g4t1;
    public GameObject g4t2;
    public GameObject g4t3;
    public GameObject itemGroup1;
    public GameObject itemGroup2;
    public GameObject itemGroup3;
    public GameObject itemGroup4;

    public void setAllSelectableItemsInactive()
    {
        items.Add(g1t1);
        items.Add(g1t2);
        items.Add(g1t3);
        items.Add(g2t1);
        items.Add(g2t2);
        items.Add(g2t3);
        items.Add(g3t1);
        items.Add(g3t2);
        items.Add(g3t3);
        items.Add(g4t1);
        items.Add(g4t2);
        items.Add(g4t3);

        //初始全部不启用
        for (int i = 0; i < items.Count; i++)
        {
            items[i].SetActive(false);
        }
    }

    public void setFirstHalfGroupActive()
    {
        Debug.Log("firsthalf");
        g1t1.SetActive(true);
        g1t2.SetActive(true);
        g1t3.SetActive(true);
        g1t1.GetComponent<ButtonEventHandler>().enabled = true;
        g1t2.GetComponent<ButtonEventHandler>().enabled = false;
        g1t3.GetComponent<ButtonEventHandler>().enabled = false;
    }

    public void setSecondHalfGroupActive()
    {
        Debug.Log("secondhalf");
        g1t1.GetComponent<ButtonEventHandler>().enabled = false;
        g1t2.GetComponent<ButtonEventHandler>().enabled = true;
        g1t3.GetComponent<ButtonEventHandler>().enabled = true;
    }

    public void setSecondGroupActive()
    {
        g2t1.SetActive(true);
        g2t2.SetActive(true);
        g2t3.SetActive(true);
        g2t1.GetComponent<ButtonEventHandler>().enabled = true;
        g2t2.GetComponent<ButtonEventHandler>().enabled = true;
        g2t3.GetComponent<ButtonEventHandler>().enabled = true;
    }

    public void setRemainedSecondGroupActive()
    {
        g2t1.GetComponent<ButtonEventHandler>().enabled = true;
        g2t2.GetComponent<ButtonEventHandler>().enabled = true;
        g2t3.GetComponent<ButtonEventHandler>().enabled = true;
    }

    public void setThirdGroupActive()
    {
        g3t1.SetActive(true);
        g3t2.SetActive(true);
        g3t3.SetActive(true);
        g3t1.GetComponent<ButtonEventHandler>().enabled = true;
        g3t2.GetComponent<ButtonEventHandler>().enabled = true;
        g3t3.GetComponent<ButtonEventHandler>().enabled = true;
    }

    public void setRemainedThirdGroupActive()
    {
        g3t1.GetComponent<ButtonEventHandler>().enabled = true;
        g3t2.GetComponent<ButtonEventHandler>().enabled = true;
        g3t3.GetComponent<ButtonEventHandler>().enabled = true;
    }

    public void setFourthGroupActive()
    {
        g4t1.SetActive(true);
        g4t2.SetActive(true);
        g4t3.SetActive(true);
        g4t1.GetComponent<ButtonEventHandler>().enabled = true;
        g4t2.GetComponent<ButtonEventHandler>().enabled = true;
        g4t3.GetComponent<ButtonEventHandler>().enabled = true;
    }

    public void setRemainedFourthGroupActive()
    {
        g4t1.GetComponent<ButtonEventHandler>().enabled = true;
        g4t2.GetComponent<ButtonEventHandler>().enabled = true;
        g4t3.GetComponent<ButtonEventHandler>().enabled = true;
    }


    public void setFirstGroupInactive()
    {
        g1t1.GetComponent<ButtonEventHandler>().enabled = false;
        g1t2.GetComponent<ButtonEventHandler>().enabled = false;
        g1t3.GetComponent<ButtonEventHandler>().enabled = false;
    }

    public void setSecondGroupInactive()
    {
        g2t1.GetComponent<ButtonEventHandler>().enabled = false;
        g2t2.GetComponent<ButtonEventHandler>().enabled = false;
        g2t3.GetComponent<ButtonEventHandler>().enabled = false;
    }

    public void setThirdGroupInactive()
    {
        g3t1.GetComponent<ButtonEventHandler>().enabled = false;
        g3t2.GetComponent<ButtonEventHandler>().enabled = false;
        g3t3.GetComponent<ButtonEventHandler>().enabled = false;
    }

    public void setFourthGroupInactive()
    {
        g4t1.GetComponent<ButtonEventHandler>().enabled = false;
        g4t2.GetComponent<ButtonEventHandler>().enabled = false;
        g4t3.GetComponent<ButtonEventHandler>().enabled = false;
    }

    public void clearItemGroup1()
    {
        itemGroup1.SetActive(false);
    }

    public void clearItemGroup2()
    {
        itemGroup2.SetActive(false);
    }

    public void clearItemGroup3()
    {
        itemGroup3.SetActive(false);
    }

    public void clearItemGroup4()
    {
        itemGroup4.SetActive(false);
    }
}

