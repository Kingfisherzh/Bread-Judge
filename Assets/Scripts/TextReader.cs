using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text;
using System;

public class TextReader: MonoBehaviour
{ 
    public class textData{
        public int ch_count;
        public int en_count;
        public string id;
        public string character;
        public string original;
        public string translation;
        public string great_next;
        public string next;
        public List<string> choices = new List<string>();
        public int choice_num;
        public string animation;
        public string story;
        public bool item_selectable;  // TRUE, 1, 2 3
        public string father_image;
        public string god_image;
        public string bread_image;
    }

    public class choiceData
    {
        public int ch_count;
        public int en_count;
        public string id;
        public string original;
        public string translation;
        public string next;
    }

    public Dictionary<string, textData> textDic = new Dictionary<string, textData>();
    public Dictionary<string, choiceData> choiceDic = new Dictionary<string, choiceData>();


    public Dictionary<string, choiceData> LoadChoiceData()
    {
        TextAsset textLines = Resources.Load("choiceData") as TextAsset;
        string[] lines = textLines.text.Split('\n');
        for (int i = 1; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split('\t');
            choiceData line = new choiceData();

            line.ch_count = int.Parse(parts[0]);
            line.id = parts[2];
            line.original = parts[3];
            line.translation = parts[4];
            line.next = parts[5];
            choiceDic.Add(line.id, line);
        }

        return choiceDic;
    }

    public Dictionary<string, textData> LoadTextData()
    {
        TextAsset textLines = Resources.Load("textData") as TextAsset;
        string[] lines = textLines.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split('\t');
            if (parts[2] != "")
            {
                textData line = new textData();
                line.ch_count = int.Parse(parts[0]);
                line.id = parts[2];
                line.character = parts[3];
                line.original = parts[4];
                line.translation = parts[5];
                line.great_next = parts[6];
                line.next = parts[7];

                if (parts[8] != "")
                {
                    string[] choices = parts[8].Split('/');
                    for (int j = 0; j < choices.Length; j++)
                    {
                        if (choices[j].Length == 4)
                        {
                            line.choices.Add(choices[j]);
                        }
                    }
                }
                if (parts[9] != "")
                {
                    line.choice_num = int.Parse(parts[9]);
                }

                line.animation = parts[10];
                line.story = parts[11];
                if (parts[12].Length > 0)
                {
                    line.item_selectable = true;
                }
                else
                {
                    line.item_selectable = false;
                }
                line.father_image = parts[13];
                line.god_image = parts[14];
                line.bread_image = parts[15];

                textDic.Add(line.id, line);
            } 
        }

        //Debug.Log(textDic["1000"].next);
        //Debug.Log(textDic["1000"].great_next);
        //Debug.Log(textDic["1000"].choice_num);

        //Debug.Log(textDic["1000"].choices);

        //int a = textDic["1000"].choices.Count;  // choices用count来判断是否有
        //Debug.Log("ifEmpty: " + (a == 0));

        //string b = textDic["1000"].great_next;  //别的string都用length是否为0来判断有无值
        //Debug.Log("if0" + (b.Length == 0));

        //Debug.Log("1001 == 1001: " + textDic["1000"].next.Equals("1001"));
        return textDic;
    }
}
