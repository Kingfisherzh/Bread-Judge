using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    List<int> a = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        a.Add(1);
        a.Add(2);
        a.Clear();
        Debug.Log("aaaaaa" + a.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
