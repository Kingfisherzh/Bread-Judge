using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


[RequireComponent(typeof(EventTrigger))]
public class ButtonEventHandler : MonoBehaviour
{
    public GameObject return_next_game_object;
    public GameObject remain_selectable_item_num_game_object;

    void Start()
    {
        Button btn = this.GetComponent<Button>();
        EventTrigger trigger = btn.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entryPointerDown = new EventTrigger.Entry();
        EventTrigger.Entry entryPointerEnter = new EventTrigger.Entry();
        EventTrigger.Entry entryPointerExit = new EventTrigger.Entry();

        entryPointerDown.eventID = EventTriggerType.PointerClick;
        entryPointerEnter.eventID = EventTriggerType.PointerEnter;
        entryPointerExit.eventID = EventTriggerType.PointerExit;

        entryPointerDown.callback = new EventTrigger.TriggerEvent();
        entryPointerEnter.callback = new EventTrigger.TriggerEvent();
        entryPointerExit.callback = new EventTrigger.TriggerEvent();

        entryPointerDown.callback.AddListener(OnClick);
        entryPointerEnter.callback.AddListener(OnMouseEnter);
        entryPointerExit.callback.AddListener(OnMouseExit);

        trigger.triggers.Add(entryPointerDown);
        trigger.triggers.Add(entryPointerEnter);
        trigger.triggers.Add(entryPointerExit);

    }

    public void OnClick(BaseEventData pointData)
    {
        Debug.Log(this.name + " clicked");
        transform.GetChild(3).gameObject.SetActive(true);    // set used icon
        transform.GetChild(2).gameObject.SetActive(false);   // unset chosen icon
        transform.GetComponent<Button>().enabled = false;      //禁掉该button
        transform.GetComponent<EventTrigger>().enabled = false;

        remain_selectable_item_num_game_object.GetComponent<Text>().text =                      // 将remain_selectable的物体的值减1，这个物体会被allstates与FSM读取
            (int.Parse(remain_selectable_item_num_game_object.GetComponent<Text>().text) - 1)
            .ToString();
        string return_next = transform.GetChild(4).gameObject.GetComponent<Text>().text;    // 获取挂在底下的下一个id的text
        Debug.Log("return_next: " + return_next);
        GameObject.Find("return_next").GetComponent<Text>().text = return_next;
    }

    public void OnMouseEnter(BaseEventData pointData)
    {
        transform.GetChild(2).gameObject.SetActive(true);
    }

    public void OnMouseExit(BaseEventData pointData)
    {
        transform.GetChild(2).gameObject.SetActive(false);
    }
}
