using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ItemManager;

public interface IState
{
    void OnEnter();
    void OnUpdate();
    void OnExit();
    void getState();
}


public class TalkNormal : IState
{
    private FiniteStateMachineManager manager;


    public TalkNormal(FiniteStateMachineManager manager)
    {
        this.manager = manager;
    }
    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
    }

    public void getState()
    {
        Debug.Log("Talk");
    }

}

public class SelectItem : IState
{
    private FiniteStateMachineManager FSMmanager;
    public int round = 1;   // max 4
    public int remain_selectable_item_num;
    public string return_next;
    public ItemManager itemManager;

    public GameObject group1;
    public GameObject group2;
    public GameObject group3;
    public GameObject group4;

    public bool readyForExit = false;

    public SelectItem(FiniteStateMachineManager manager)
    {
        this.FSMmanager = manager;
        itemManager = GameObject.Find("ScriptController").GetComponent<ItemManager>();
        group1 = GameObject.Find("group1");
        group2 = GameObject.Find("group2");
        group3 = GameObject.Find("group3");
        group4 = GameObject.Find("group4");
    }

    public void OnEnter()
    {
        remain_selectable_item_num = GetRemainedItemNum();  //获取最新的剩余可选数
        if (round == 1)
        {
           if (remain_selectable_item_num == 3) itemManager.setFirstHalfGroupActive();
            else itemManager.setSecondHalfGroupActive();
        }
        else if (round == 2)
        {
            if (remain_selectable_item_num == 3) itemManager.setSecondGroupActive();
            else itemManager.setRemainedSecondGroupActive();
        }
        else if (round == 3)
        {
            if (remain_selectable_item_num == 3) itemManager.setThirdGroupActive();
            else itemManager.setRemainedThirdGroupActive();
        }
        else if (round == 4)
        {
            if (remain_selectable_item_num == 3) itemManager.setFourthGroupActive();
            else itemManager.setRemainedFourthGroupActive();
        }
    }

    public int GetRemainedItemNum()
    {
        return int.Parse(GameObject.Find("remain_selectable_item_num").GetComponent<Text>().text);
    }

    public void OnExit()
    {
        if (round == 1)
        {
            itemManager.setFirstGroupInactive();
        }
        else if (round == 2)
        {
            itemManager.setSecondGroupInactive();
        }
        else if (round == 3)
        {
            itemManager.setThirdGroupInactive();
        }
        else if (round == 4)
        {
            itemManager.setFourthGroupInactive();
        }

        //三个道具都选完了
        if (remain_selectable_item_num == 0)
        {
            round += 1;
        }
    }

    public void OnUpdate()
    {
        int latestRemainedNum = GetRemainedItemNum();
        if (remain_selectable_item_num != latestRemainedNum)
        {
            Debug.Log(latestRemainedNum);
            remain_selectable_item_num = latestRemainedNum;
        }
    }

    public void getState(){ Debug.Log("SelectItem"); }

}

public class Story : IState
{
    public int round;
    public StoryManager storyManager;
    private FiniteStateMachineManager manager;

    public Story(FiniteStateMachineManager manager)
    {
        this.manager = manager;
        round = 1;
        storyManager = GameObject.Find("ScriptController").GetComponent<StoryManager>();
    }
    public void OnEnter()
    {
        storyManager.TurnOnStoryGroup(round);
        Debug.Log("StoryModeEnter");
    }

    public void OnExit()
    {
        Debug.Log("StoryModeExit");
        round += 1;
        storyManager.TurnOffAllStoryGroups();
        GameObject.Find("great_next").GetComponent<Text>().text = "";   //清空挂载的great_next
        GameObject.Find("selected_item_num_in_story").GetComponent<Text>().text = "0";  //story中已选的数目变为0
    }

    public void OnUpdate()
    {
        string selected_item_num_in_story = GameObject.Find("selected_item_num_in_story").GetComponent<Text>().text;
        if (selected_item_num_in_story == "3")
        {
            GameObject.Find("return_next").GetComponent<Text>().text = GameObject.Find("great_next").GetComponent<Text>().text; //赋值给return_next，这样FSM会读取到return_next更新了
        }
    }

    public void getState()
    {
        Debug.Log("Story");
    }

    public string getNext()
    {
        return "null";
    }
}

public class Animation : IState
{
    private FiniteStateMachineManager manager;
    public Animation(FiniteStateMachineManager manager)
    {
        this.manager = manager;
    }
    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
    }

    public void getState()
    {
        Debug.Log("Animation");
    }

    public string getNext()
    {
        return "null";
    }
}
