using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData; 
    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }
    void GenerateData()
    {
        talkData.Add(1, new string[] { "안녕 짱구야", "액션가면이야 !!", "선물을 줄게 받을래?", "[E] 받기, [Esc] 안받기" });
        talkData.Add(2, new string[] { "안녕 짱구야", "토페마야 !!", "선물을 줄게 받을래?", "[E] 받기, [Esc] 안받기" });
        talkData.Add(3, new string[] { "안녕 짱구야", "건담로봇이야 !!", "선물을 줄게 받을래?", "[E] 받기, [Esc] 안받기" });
        talkData.Add(4, new string[] { "안녕 짱구야", "부리부리대마왕이야 !!", "선물을 줄게 받을래?", "[E] 받기, [Esc] 안받기" });
        talkData.Add(5, new string[] { "안녕 짱구야", "비룡이야 !!", "선물을 줄게 받을래?", "[E] 받기, [Esc] 안받기" });
    }
    public string GetTalk(int id, int talkText)
    {
        if(talkText >= talkData[id].Length)
            return null;
        else
            return talkData[id][talkText];
    }
}