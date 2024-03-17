using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string talkText;
    public Player1 player1;
    public Player2 player2;

    [SerializeField] private TalkManager talkManager;
    [SerializeField] private Image talkPanel;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    private Item npcItem;
    public int talkIndex;

    public TalkManager TalkManager
    {
        get => talkManager;
    }
    public Image TalkPanel
    {
        get => talkPanel;
    }
    private void Awake()
    {
        Instance = this;
        talkPanel.gameObject.SetActive(false);
    }
    public bool TalkEnd(int id, NPC npc)
    {
        talkText = talkManager.GetTalk(id, talkIndex);
        if (talkText == null)
        {
            talkIndex = 0;
            talkPanel.gameObject.SetActive(false);
            npcItem = Instantiate(npc.Item);
            npcItem.transform.position = npc.transform.position;
            player1.PlayerMove.IsMove = true;
            return true;
        }
        textMeshProUGUI.text = talkText;
        talkIndex++;
        return false;
    }
}