using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public GameObject story1;
    public GameObject story2;
    public GameObject story3;
    public GameObject story4;

    public GameObject g1t1_book; 
    public GameObject g1t2_face;
    public GameObject g1t3_tear;
    public GameObject g2t1_bag;
    public GameObject g2t2_cookies;
    public GameObject g2t3_cry;
    public GameObject g3t1_card;
    public GameObject g3t2_smoke;
    public GameObject g3t3_snack;
    public GameObject g4t1_star;
    public GameObject g4t2_bread;
    public GameObject g4t3_sky;

    public void TurnOnStoryGroup(int round)
    {
        switch (round)
        {
            case 1:
                story1.SetActive(true);
                break;
            case 2:
                story2.SetActive(true);
                break;
            case 3:
                story3.SetActive(true);
                break;
            case 4:
                story4.SetActive(true);
                break;
        }
    }

    public void TurnOffAllStoryGroups()
    {
        story1.SetActive(false);
        story2.SetActive(false);
        story3.SetActive(false);
        story4.SetActive(false);
    }

    //public IEnumerator TurnOnStory1Animation() {
    //    int count = story1.transform.GetChildCount();
    //    for (int i = 0; i < count; i++)
    //    {
    //        story1.transform.GetChild(i).gameObject.SetActive(true);

    //        SpriteRenderer spiritRenderer = story1.transform.GetChild(i).GetComponent<SpriteRenderer>();
    //        var color = spiritRenderer.color;
    //        color.a = Mathf.PingPong(Time.time / 5f, 1);
    //        spiritRenderer.color = color;
    //    }
    //    yield return new WaitForSeconds(0.8f);
    //}
}
