using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class StoryButtonManager : MonoBehaviour
{
    public Texture2D cursorTexture1;
    public Texture2D cursorTexture2;
    public CursorMode cursorMode = CursorMode.Auto;
    public GameObject selected_item;
    public GameObject selected_item_num_in_story;

    void Start()
    {
        Image image = GetComponent<Image>();
        Button btn = this.GetComponent<Button>();
        image.alphaHitTestMinimumThreshold = 0.1f;

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
        if (this.GetComponent<Button>().interactable == true)
        {
            selected_item.SetActive(true);      //启用，看得到button
            selected_item.transform.GetComponent<ButtonEventHandler>().enabled = false;     //story模式中不触发鼠标点击和浮动事件
            selected_item_num_in_story.GetComponent<Text>().text = (int.Parse(selected_item_num_in_story.GetComponent<Text>().text) + 1).ToString();
            this.GetComponent<Button>().interactable = false;
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }
    }

    public void OnMouseEnter(BaseEventData pointData) {

        if (this.GetComponent<Button>().interactable == true)
        {
            Cursor.SetCursor(cursorTexture1, Vector2.zero, cursorMode);
        }
        else
        {
            Cursor.SetCursor(cursorTexture2, Vector2.zero, cursorMode);
        }
    }

    public void OnMouseExit(BaseEventData pointData)
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}