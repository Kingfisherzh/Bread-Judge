using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum State_Enum
{
    talk_normal,
    select_item,
    story,
    animation
}

public class FiniteStateMachineManager : MonoBehaviour
{
    public IState currenState;
    private Dictionary<State_Enum, IState> states = new Dictionary<State_Enum, IState>();

    public TextReader textReader;

    public Dictionary<string, TextReader.textData> textDic;     // 原文
    public Dictionary<string, TextReader.choiceData> choiceDic; // 选项
    public ItemManager itemManager;

    public GameObject choice1;
    public GameObject choice2;
    public GameObject choice3;

    public GameObject story1;
    public GameObject story2;
    public GameObject story3;
    public GameObject story4;
    public Dictionary<string, GameObject> storyDic = new Dictionary<string, GameObject>();

    public GameObject group1;
    public GameObject group2;
    public GameObject group3;
    public GameObject group4;

    public GameObject fatherImageHolder;
    public GameObject godImageHolder;
    public GameObject breadImageHolder;

    public GameObject fatherDialogBox;
    public GameObject godDialogBox;
    public GameObject breadDialogBox;
    public GameObject fatherSinnerDialogBox;
    public GameObject godDaughterDialogBox;

    // 图片立绘
    public GameObject croissant_normal;
    public GameObject croissant_smail;
    public GameObject croissant_cerious;
    public GameObject baguette_normal;
    public GameObject baguette_flurried;
    public GameObject baguette_veryflurried;
    public GameObject ciabatta_normal;
    public GameObject ciabatta_closeeye;
    public GameObject ciabatta_engry;
    public GameObject god_00;
    public GameObject god_normal;
    public GameObject god_smail;
    public GameObject god_hope;
    public GameObject father_00;
    public GameObject father_look;
    public GameObject father_normal;
    public Dictionary<string, GameObject> imageDic = new Dictionary<string, GameObject>();

    public Button ExitGameButton;
    public GameObject ItemSquare;
    public GameObject stageSpotLight;

    public GameObject sceneChange1;
    public GameObject sceneChange2;
    public GameObject sceneChange3;
    public GameObject sceneChange4;

    public string next;
    public string great_next;
    public GameObject return_next_game_object;
    public GameObject remain_selectable_item_num;
    public GameObject great_next_game_object;

    public List<string> choices = new List<string>();
    public int choice_num;
    public bool item_selectable;
    public int scene_round;

    public bool isDialogClickable;   // 是否为对话中，是则允许点击
    public bool isClickingOnChoiceEnabled;    // 是否允许点击选项，是则允许点击。待挂载在按钮上
    public bool ifDirectToStoryMode;
    public bool ifDirectToAnimationMode;

    public bool ifChoice1Clicked;
    public bool ifChoice2Clicked;
    public bool ifChoice3Clicked;

    public string latestStory;

    private void Awake()
    {
        AddImagesToImageDic();
        AddStoriesToStoryDic();
        DisableDialogBoxes();

        next = "1000";
        great_next = "";
        choice_num = -1;
        scene_round = 1;

        isDialogClickable = false;
        isClickingOnChoiceEnabled = false;
        ifDirectToStoryMode = false;
        ifDirectToAnimationMode = false;

        ifChoice1Clicked = false;
        ifChoice2Clicked = false;
        ifChoice3Clicked = false;

        choice1.SetActive(false);
        choice2.SetActive(false);
        choice3.SetActive(false);

        story1.SetActive(false);
        story2.SetActive(false);
        story3.SetActive(false);
        story4.SetActive(false);

        sceneChange1.SetActive(false);
        sceneChange2.SetActive(false);
        sceneChange3.SetActive(false);
        sceneChange4.SetActive(false);

        ItemSquare.SetActive(false);
        stageSpotLight.SetActive(false);

        //初始图
        fatherImageHolder = father_00;
        godImageHolder = god_00;
        breadImageHolder = croissant_normal;

        // 从txt存入数据
        textDic = textReader.LoadTextData();
        choiceDic = textReader.LoadChoiceData();

        states.Add(State_Enum.talk_normal, new TalkNormal(this));
        states.Add(State_Enum.select_item, new SelectItem(this));
        states.Add(State_Enum.animation, new Animation(this));
        states.Add(State_Enum.story, new Story(this));

        itemManager.setAllSelectableItemsInactive();
        Debug.Log("OnAwake");
    }

    public void SwitchToOtherState(State_Enum type)
    {
        ItemSquare.SetActive(true);
        currenState = states[type];
        currenState.OnEnter();
    }

    public void SwitchBackToTalkState()
    {
        if (currenState != null)
        {
            currenState.OnExit();
        }

        if (currenState == states[State_Enum.story] && latestStory.Length > 0)
        {
            StartCoroutine(PlayStoryAnimation(storyDic[latestStory], 1));
            Debug.Log("ExitStoryAnimation");
            //storyDic[latestStory].SetActive(false);
        }

        currenState = states[State_Enum.talk_normal];
        isDialogClickable = true;

        isClickingOnChoiceEnabled = false;        
        ifDirectToStoryMode = false;    //关闭故事模式识别
        ifDirectToAnimationMode = false;    // 关闭动画模式识别
        storyDic[latestStory].SetActive(false);
        Debug.Log("SwitchBackToTalkState");
    }

    public void readCurrentRowOtherInfo(string id)
    {
        TextReader.textData textRow = textDic[id];
        next = textRow.next;

        if (next.Length > 0)
        {
            // 判断是否进入故事模式
            Debug.Log("textRow.story" + textRow.story);
            if (textRow.story.Length > 0)
            {
                latestStory = textRow.story;
                ifDirectToStoryMode = true;
                StartCoroutine(PlayStoryAnimation(storyDic[textRow.story], 0));
            }
            // 判断是否进入动画模式
            else if (textRow.animation.Length > 0)
            {
                ifDirectToAnimationMode = true;
                Debug.Log("textRow.animation: " + textRow.animation);
                switch (textRow.animation)
                {
                    case "scene":
                        switch (scene_round)
                        {
                            case 1:
                                Debug.Log("SceneChanging");
                                StartCoroutine(ReadyForSceneChange(sceneChange1));
                                break;
                            case 2:
                                StartCoroutine(ReadyForSceneChange(sceneChange2));
                                break;
                            case 3:
                                StartCoroutine(ReadyForSceneChange(sceneChange3));
                                break;
                            case 4:
                                StartCoroutine(ReadyForSceneChange(sceneChange4));
                                break;
                        }
                        scene_round += 1;
                        break;
                    case "crossiant-enter":
                        StartCoroutine(PlayCrossiantEnterAnimation());
                        break;
                    case "baguette-enter":
                        StartCoroutine(PlayBaguetteEnterAnimation());
                        break;
                    case "ciabatta-enter":
                        StartCoroutine(PlayCiabattaEnterAnimation());
                        break;
                    case "guilty":
                        StartCoroutine(PlayGuiltyAnimation());
                        break;
                    case "not_guilty":
                        StartCoroutine(PlayNotGuiltyAnimation());
                        break;
                    case "father_enter":
                        StartCoroutine(PlayFatherMoveAnimation());
                        break;
                    case "bread_jump":
                        StartCoroutine(PlayBreadJumpAnimation());
                        break;
                }
                ifDirectToAnimationMode = false;
            }
        }
        else if (next.Length == 0)
        {
            // 遇到了一个分支点，承接对话选项或者道具选择
            if (textRow.great_next.Length > 0)
            {
                great_next = textRow.great_next;
                great_next_game_object.GetComponent<Text>().text = great_next;
            }
            // 进入对话选项
            if (textRow.choices.Count > 0)
            {
                choices = textRow.choices;
                choice_num = textRow.choice_num;
            }
            //接下来要选东西
            else if (textRow.item_selectable == true)
            {
                item_selectable = textRow.item_selectable;
            }
        }

        Debug.Log("readCurrentRowOtherInfo");
    }

    public void DisableDialogBoxes()
    {
        breadDialogBox.SetActive(false);
        godDialogBox.SetActive(false);
        fatherDialogBox.SetActive(false);
        fatherSinnerDialogBox.SetActive(false);
        godDaughterDialogBox.SetActive(false);
    }

    public void ClearItemGroupIfRoundOver(string id)
    {
        switch (id)
        {
            case "1111":
                itemManager.clearItemGroup1();
                break;
            case "1177":
                itemManager.clearItemGroup2();
                break;
            case "1256":
                itemManager.clearItemGroup3();
                break;
        }
    }

    public void DisplayEndGameButton()
    {
        ItemSquare.SetActive(false);
        group1.SetActive(false);
        group2.SetActive(false);
        group3.SetActive(false);
        group4.SetActive(false);
        StartCoroutine(CloseDialog());
        godDialogBox.SetActive(false);
        ExitGameButton.gameObject.SetActive(true);
    }

    public void DisplayNextDialog(Dictionary<string, TextReader.textData> localTextDic, string id)
    {
        if (id != "1325")
        {
            ClearItemGroupIfRoundOver(id);
            DisableDialogBoxes();
            displayDialogText(localTextDic[id].character, localTextDic[id].translation);  //展示说话人的对话

            disableImages();
            displayImage(localTextDic[id].father_image);     //用文件名作为key，调取gameobject
            displayImage(localTextDic[id].god_image);
            displayImage(localTextDic[id].bread_image);

            //next = "";    //清空next句子，等待readCurrentRowOtherInfo里读取新的信息
            isDialogClickable = true;
            readCurrentRowOtherInfo(id);

            Debug.Log("DisplayNextDialog");
        }
        else
        {
            DisplayEndGameButton();
        }
    }

    public void DisplayNextChoices()
    {
        Debug.Log("DisplayNextChoices");
        if (choices.Count == 1)
        {
            choice1.SetActive(true);
            choice1.GetComponentInChildren<Text>().text = choiceDic[choices[0]].translation;
        }
        else if (choices.Count == 2)
        {
            choice1.SetActive(true);
            choice2.SetActive(true);
            choice1.GetComponentInChildren<Text>().text = choiceDic[choices[0]].translation;
            choice2.GetComponentInChildren<Text>().text = choiceDic[choices[1]].translation;
        }
        else if (choices.Count == 3)
        {
            choice1.SetActive(true);
            choice2.SetActive(true);
            choice3.SetActive(true);
            choice1.GetComponentInChildren<Text>().text = choiceDic[choices[0]].translation;
            choice2.GetComponentInChildren<Text>().text = choiceDic[choices[1]].translation;
            choice3.GetComponentInChildren<Text>().text = choiceDic[choices[2]].translation;
        }
    }

    public void ClearChoicesIfNoRemainedChoices()
    {
        Debug.Log("ClearChoicesIfNoRemainedChoices");
        if (choice_num == 0)
        {
            choices.Clear();
            Debug.Log("ClearChoicesSuccess");
        }
    }

    public void deleteClickedChoiceAndGiveNextValue(int index)
    {
        choice_num -= 1;     //可选选项数目减一
        Debug.Log("updated choice_num " + choice_num + " next " + choices[index - 1]);

        string clickedChoiceButtonId = choices[index - 1];  // choices中存储的都是id，可以获取当前被点击的按钮的选项id
        next = choiceDic[clickedChoiceButtonId].next;   // 找到对应next，是对话id
        choices.RemoveAt(index - 1);

        Debug.Log(next.Equals("1002"));

        ClearChoicesIfNoRemainedChoices();
        Debug.Log("DisableNextChoices");
        DisableNextChoices();
    }

    public void DisableNextChoices()
    {
        choice1.SetActive(false);
        choice2.SetActive(false);
        choice3.SetActive(false);
    }


    public void displayDialogText(string character, string translation)
    {
        switch (character)
        {
            case "神":
                godDialogBox.SetActive(true);
                godDialogBox.GetComponentInChildren<Text>().text = translation;
                break;
            case "主角":
                fatherDialogBox.SetActive(true);
                fatherDialogBox.GetComponentInChildren<Text>().text = translation;
                break;
            case "主角罪人":
                fatherDialogBox.SetActive(false);
                fatherSinnerDialogBox.SetActive(true);
                fatherSinnerDialogBox.GetComponentInChildren<Text>().text = translation;
                break;

            case "神女儿":
                godDialogBox.SetActive(false);
                godDaughterDialogBox.SetActive(true);
                godDaughterDialogBox.GetComponentInChildren<Text>().text = translation;
                break;
            default:
                breadDialogBox.SetActive(true);
                breadDialogBox.GetComponentInChildren<Text>().text = translation;
                break;
        }
    }

    public void disableImages()
    {
        godImageHolder.SetActive(false);
        fatherImageHolder.SetActive(false);
        breadImageHolder.SetActive(false);
    }

    public void displayImage(string imgName)
    {
        string character = imgName.Split('_')[0];

        if (character == "god")
        {
            imageDic[imgName].SetActive(true);
            godImageHolder = imageDic[imgName];
        }
        else if (character == "father")
        {
            Debug.Log("father Image Name: " + imgName);
            imageDic[imgName].SetActive(true);
            fatherImageHolder = imageDic[imgName];
        }
        else if (character == "ciabatta" || character == "croissant" || character == "baguette")
        {
            imageDic[imgName].SetActive(true);
            breadImageHolder = imageDic[imgName];
        }
    }



    void Start()
    {
        //SwitchToOtherState(State_Enum.select_item);
        SwitchBackToTalkState();
        Debug.Log("Start");
    }

    void Update()
    {
        // 别的模式的onUpdate也要继续
        currenState.OnUpdate();
        string return_next = return_next_game_object.GetComponentInChildren<Text>().text;
        Debug.Log("great_next: " + great_next);
        Debug.Log("return_next: " + return_next);

        //其他模式回到普通对话模式
        if (return_next.Length > 0)
        {
            Debug.Log("Return Next is not Null " + return_next);
            return_next_game_object.GetComponentInChildren<Text>().text = "";     //重置回空字符串
            SwitchBackToTalkState();                                    //重置回普通说话模式
            DisplayNextDialog(textDic, return_next);
        }
        // 普通对话模式中，某个选项的后续对话结束/道具对话结束
        else if (next.Length == 0)
        {
            Debug.Log("Next is Null");

            //动画模式
            if (ifDirectToAnimationMode)
            {
                SwitchToOtherState(State_Enum.animation);
            }
            // 选项对话
            //if (choice_num > 0 && isDialogClickable == false)
            else if (choice_num > 0)
                {
                DisplayNextChoices();
                next = "";  //清空next句子，等待点击返回的next
            }
            //道具模式
            else if (item_selectable)
            {
                int updated_remained_num = int.Parse(remain_selectable_item_num.GetComponent<Text>().text); //获取最新可选道具数量
                if (updated_remained_num > 0) 
                {
                    SwitchToOtherState(State_Enum.select_item);
                }
                else if (updated_remained_num == 0)     //下一次循环会进入great_next
                {
                    remain_selectable_item_num.GetComponent<Text>().text = "3"; //重置为3
                    item_selectable = false;
                }
            }
            //结束选项对话模式 或者 选中道具后对话全部结束
            else if (great_next.Length > 0)
            {
                string temp_great_next = great_next;
                great_next = "";
                DisplayNextDialog(textDic, temp_great_next);
                item_selectable = false;    // 禁止进入选择模式，需要读取到相关数据才会重赋true
            }
        }
        //故事探索模式
        else if (ifDirectToStoryMode)
        {
            if (currenState != states[State_Enum.story])
            {
                great_next_game_object.GetComponent<Text>().text = next;
                SwitchToOtherState(State_Enum.story);
            }
        }
        // 普通对话模式继续
        else if (next.Length > 0 && isDialogClickable == false && ifDirectToStoryMode == false && ifDirectToAnimationMode == false)
        {
            DisableNextChoices();   //关闭对话可选选项
            ClearChoicesIfNoRemainedChoices();  //如果可选选项数目为0，则清空已有的choices列表

            Debug.Log("Next is not Null: " + next);
            DisplayNextDialog(textDic, next);

        }

        Debug.Log("next " + next + " isDialogClickable: " + isDialogClickable);
    }


    public void AddImagesToImageDic()
    {
        imageDic.Add("croissant_normal", croissant_normal);
        imageDic.Add("croissant_smail", croissant_smail);
        imageDic.Add("croissant_cerious", croissant_cerious);
        imageDic.Add("baguette_normal", baguette_normal);
        imageDic.Add("baguette_flurried", baguette_flurried);
        imageDic.Add("baguette_veryflurried", baguette_veryflurried);
        imageDic.Add("ciabatta_normal", ciabatta_normal);
        imageDic.Add("ciabatta_closeeye", ciabatta_closeeye);
        imageDic.Add("ciabatta_engry", ciabatta_engry);
        imageDic.Add("father_normal", father_normal);
        imageDic.Add("father_00", father_00);
        imageDic.Add("father_look", father_look);
        imageDic.Add("god_00", god_00);
        imageDic.Add("god_smail", god_smail);
        imageDic.Add("god_normal", god_normal);
        imageDic.Add("god_hope", god_hope);


        croissant_normal.SetActive(false);
        croissant_smail.SetActive(false);
        croissant_cerious.SetActive(false);
        baguette_normal.SetActive(false);
        baguette_flurried.SetActive(false);
        baguette_veryflurried.SetActive(false);
        ciabatta_normal.SetActive(false);
        ciabatta_closeeye.SetActive(false);
        ciabatta_engry.SetActive(false);
        father_normal.SetActive(false);
        father_look.SetActive(false);
        father_00.SetActive(false);
        god_smail.SetActive(false);
        god_normal.SetActive(false);
        god_hope.SetActive(false);
        god_00.SetActive(false);
    }

    public IEnumerator StopToPlayAnimation(int count)
    {
        yield return StartCoroutine(WaitForAnimation(count));
        
    }

    public IEnumerator WaitForAnimation(int count)
    {
        if (count == 1)
        {
            breadImageHolder.transform.position = new Vector3(0f, 9.0f, 20f);
            breadImageHolder.transform.Translate(new Vector3(0, -0.1f * Time.deltaTime, 0));
        }
        yield return null;

    }


    private IEnumerator PlayBreadJumpAnimation()
    {
        float time = 0.8f;
        Vector3 startingPos = godImageHolder.transform.position;
        Vector3 finalPos = new Vector3(-11.3100004f, 0.949999988f, 20f);
        float elapsedTime = 0;
        yield return StartCoroutine(SmoothLerp(time, startingPos, finalPos, elapsedTime, godImageHolder));

        godImageHolder = god_smail;
        godImageHolder.SetActive(true);
        time = 2.5f;
        startingPos = new Vector3(-11f, -1.7700001f, 10f);
        finalPos = new Vector3(-3.3499999f, -1.7700001f, 10f);
        yield return StartCoroutine(SmoothLerp(time, startingPos, finalPos, elapsedTime, godImageHolder));

        yield return null;
    }

    private IEnumerator PlayFatherMoveAnimation()
    {
        float time = 0.3f;
        Vector3 startingPos = fatherImageHolder.transform.position;
        Vector3 finalPos = startingPos;
        finalPos.y = -8.0f;
        float elapsedTime = 0;
        StartCoroutine(SmoothLerp(time, startingPos, finalPos, elapsedTime, fatherImageHolder));

        time = 0.7f;
        fatherImageHolder = father_normal;
        fatherImageHolder.SetActive(true);
        startingPos = new Vector3(0f, 9.0f, 10f);
        finalPos = new Vector3(0f, -0.3f, 10f);
        StartCoroutine(SmoothLerp(time, startingPos, finalPos, elapsedTime, fatherImageHolder));

        Debug.Log("fatherImageHolder");
        yield return null;
    }

    private IEnumerator PlayGuiltyAnimation()
    {
        float time = 0.5f;
        Vector3 startingPos = breadImageHolder.transform.position;
        Vector3 finalPos = new Vector3(0f, -8.0f, 20f);
        float elapsedTime = 0;
        yield return StartCoroutine(SmoothLerp(time, startingPos, finalPos, elapsedTime, breadImageHolder));

        Color lightColor = stageSpotLight.GetComponent<SpriteRenderer>().color;
        yield return StartCoroutine(PlayLightChangeAnimation(1.0f, 1f, 0f, lightColor, 0f, stageSpotLight));
        stageSpotLight.SetActive(false);

        yield return null;
    }

    private IEnumerator PlayNotGuiltyAnimation()
    {
        float time = 2.0f;
        Vector3 startingPos = breadImageHolder.transform.position;
        float y = breadImageHolder.transform.position.y;
        Vector3 finalPos = new Vector3(11f, -1.1f, 20f);
        finalPos.y = y;
        float elapsedTime = 0;
        StartCoroutine(SmoothLerp(time, startingPos, finalPos, elapsedTime, breadImageHolder));


        Color lightColor = stageSpotLight.GetComponent<SpriteRenderer>().color;
        yield return StartCoroutine(PlayLightChangeAnimation(1.0f, 1f, 0f, lightColor, 0f, stageSpotLight));
        stageSpotLight.SetActive(false);
        yield return null;

    }

    private IEnumerator PlayCrossiantEnterAnimation()
    {
        breadImageHolder.SetActive(true);
        breadImageHolder = croissant_normal;
        float time = 2.0f;
        Vector3 startingPos = new Vector3(-11f, -1.1f, 20f);
        Vector3 finalPos = new Vector3(0f, -1.1f, 20f);
        float elapsedTime = 0;
        yield return StartCoroutine(SmoothLerp(time, startingPos, finalPos, elapsedTime, breadImageHolder));

        stageSpotLight.SetActive(true);
        Color lightColor = stageSpotLight.GetComponent<SpriteRenderer>().color;
        yield return StartCoroutine(PlayLightChangeAnimation(1.0f, 0f, 1f, lightColor, 0f, stageSpotLight));
        yield return null;
    }

    private IEnumerator PlayBaguetteEnterAnimation()
    {
        breadImageHolder = baguette_normal;
        breadImageHolder.SetActive(true);
        float time = 2.0f;
        Vector3 startingPos = new Vector3(-11f, -1.23f, 10f);
        Vector3 finalPos = new Vector3(0f, -1.23f, 10f);
        float elapsedTime = 0;
        yield return StartCoroutine(SmoothLerp(time, startingPos, finalPos, elapsedTime, breadImageHolder));

        stageSpotLight.SetActive(true);
        Color lightColor = stageSpotLight.GetComponent<SpriteRenderer>().color;
        yield return StartCoroutine(PlayLightChangeAnimation(1.0f, 0f, 1f, lightColor, 0f, stageSpotLight));
        yield return null;
    }

    private IEnumerator PlayCiabattaEnterAnimation()
    {
        Debug.Log("PlayCiabattaEnterAnimation");
        breadImageHolder = ciabatta_closeeye;
        breadImageHolder.SetActive(true);
        float time = 2.0f;
        Vector3 startingPos = new Vector3(-11f, -0.93f, 10f);
        Vector3 finalPos = new Vector3(0f, -0.93f, 10f);
        float elapsedTime = 0;
        yield return StartCoroutine(SmoothLerp(time, startingPos, finalPos, elapsedTime, breadImageHolder));

        stageSpotLight.SetActive(true);
        Color lightColor = stageSpotLight.GetComponent<SpriteRenderer>().color;
        yield return StartCoroutine(PlayLightChangeAnimation(1.0f, 0f, 1f, lightColor, 0f, stageSpotLight));
        yield return null;
    }

    private IEnumerator SmoothLerp(float time, Vector3 startingPos, Vector3 finalPos, float elapsedTime, GameObject holder)
    {
        while (elapsedTime < time)
        {
            holder.transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator ReadyForSceneChange(GameObject scene)
    {
        scene.SetActive(true);
        Text text = scene.GetComponentInChildren<Text>();
        Color textColor = text.GetComponent<Text>().color;
        Color sceneColor = scene.GetComponent<Image>().color;
        float elapsedTime = 0f;
        float totalTime = 0.7f;
        float startAlpha = 0f;
        float endAlpha = 0.8f;
        yield return StartCoroutine(PlaySceneChangeAnimation(totalTime, startAlpha ,endAlpha , sceneColor, elapsedTime, scene, text, textColor));
        startAlpha = 0.8f;
        totalTime = 1.5f;
        yield return StartCoroutine(PlaySceneChangeAnimation(totalTime, startAlpha, endAlpha, sceneColor, elapsedTime, scene, text, textColor));
        startAlpha = 0.8f;
        endAlpha = 0f;
        totalTime = 1.0f;
        yield return StartCoroutine(PlaySceneChangeAnimation(totalTime, startAlpha, endAlpha, sceneColor, elapsedTime, scene, text, textColor));
        scene.SetActive(false);
        yield return null;
    }

    private IEnumerator PlaySceneChangeAnimation(float totalTime, float startAlpha, float endAlpha, Color sceneColor, float elapsedTime, GameObject scene, Text text, Color textColor)
    {
        while (elapsedTime < totalTime)
        {
            sceneColor.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / totalTime);
            textColor.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / totalTime);
            elapsedTime += Time.deltaTime;
            scene.GetComponent<Image>().color = sceneColor;
            text.GetComponent<Text>().color = textColor;
            yield return null;
        }
    }

    private IEnumerator PlayLightChangeAnimation(float totalTime, float startAlpha, float endAlpha, Color sceneColor, float elapsedTime, GameObject scene)
    {
        while (elapsedTime < totalTime)
        {
            sceneColor.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / totalTime);
            Debug.Log("sceneColor.a: " + sceneColor.a);
            elapsedTime += Time.deltaTime;
            scene.GetComponent<SpriteRenderer>().color = sceneColor;
            yield return null;
        }
    }

    private IEnumerator PlayStoryAnimation(GameObject parentStory, int startOrEnd)
    {
        parentStory.SetActive(true);
        List<GameObject> storyChildren = new List<GameObject>();
        for (int i=0; i< parentStory.transform.childCount; i++)
        {
            storyChildren.Add(parentStory.transform.GetChild(i).gameObject);
        }
        switch (startOrEnd)
        {
            case 0:
                yield return StartCoroutine(PlayEnterStoryAnimationForChildren(storyChildren));
                break;
            case 1:
                yield return StartCoroutine(PlayExitStoryAnimationForChildren(storyChildren));
                break;
        }
        yield return null;
    }

    private IEnumerator PlayEnterStoryAnimationForChildren(List<GameObject> storyChildren)
    {
        float totalTime = 2.0f;
        float startAlpha = 0f;
        float endAlpha = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < totalTime)
        {
            for (int i = 0; i < storyChildren.Count; i++)
            {
                Color color = storyChildren[i].GetComponent<Image>().color;
                color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / totalTime);
                elapsedTime += Time.deltaTime;
                storyChildren[i].GetComponent<Image>().color = color;
            }
            yield return null;
        }
    }

    private IEnumerator PlayExitStoryAnimationForChildren(List<GameObject> storyChildren)
    {
        float totalTime = 2.0f;
        float startAlpha = 1f;
        float endAlpha = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < totalTime)
        {
            for (int i = 0; i < storyChildren.Count; i++)
            {
                Color color = storyChildren[i].GetComponent<Image>().color;
                color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / totalTime);
                elapsedTime += Time.deltaTime;
                storyChildren[i].GetComponent<Image>().color = color;
            }
            yield return null;
        }
    }


    private IEnumerator CloseDialog()
    {
        yield return new WaitForSeconds(5);
    }


    public void AddStoriesToStoryDic()
    {
        storyDic.Add("story1", story1);
        storyDic.Add("story2", story2);
        storyDic.Add("story3", story3);
        storyDic.Add("story4", story4);
    }
}
