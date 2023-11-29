using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    private int rows = 4;
    private int cols = 4;
    private int totalCards;
    private int matches = 0;
    private int matchesNeeded = 5;
    private int cardW = 100;
    private int cardH = 100;
    private ArrayList aCard;
    private ArrayList aGrid;
    private ArrayList aCardsFlipped;
    private bool playerCanClick;
    private bool playerHasWon = false;

    // Start is called before the first frame update
    void Start()
    {
        playerCanClick = true;
        aCard = new ArrayList();
        aGrid = new ArrayList();
        aCardsFlipped = new ArrayList();
        BuildDeck();
        for (int i = 0; i < rows; i++)
        {
            aGrid.Add(new ArrayList());
            for (int j = 0; j < cols; j++)
            {
                ((ArrayList)aGrid[i]).Add(new Card());
                int someNum = UnityEngine.Random.Range(0,aCard.Count);
                ((ArrayList)aGrid[i])[j] = aCard[someNum];
                aCard.RemoveAt(someNum);
            }
        }
        
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        GUILayout.BeginHorizontal();
        BuildGrid();
        if(playerHasWon) BuildWinPrompt();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void BuildGrid()
    {
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        for (int i = 0; i < rows; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            for (int j = 0; j < cols; j++)
            {
                Card card = (Card)((ArrayList)aGrid[i])[j];
                string img;

                if(card.isMatched)
                {
                    img = "blank";
                }
                else
                {
                    if(card.isFaceUp)
                    {
                        img = card.img;
                    }
                    else
                    {
                        img = "wrench";
                    }
                }
                GUI.enabled = !card.isMatched;
                if (GUILayout.Button(Resources.Load<Texture>(img), GUILayout.Width(cardW)))
                {
                    if(playerCanClick)
                    {
                        FlipCardFaceUp(card);
                    }
                    Debug.Log(card.img);
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
    }

    private void BuildDeck()
    {
        int totalRobots = 4;
        Card card;
        int id = 0;

        for(int i = 0; i < totalRobots; i++)
        {
            ArrayList aRobotParts = new ArrayList();
            aRobotParts.Add("Head");
            aRobotParts.Add("Arm");
            aRobotParts.Add("Leg");
            for(int j = 0; j < 2; j++)
            {
                int someNum = UnityEngine.Random.Range(0, aRobotParts.Count);
                string theMissingPart = (string)aRobotParts[someNum];
                
                aRobotParts.RemoveAt(someNum);

                card = new Card("robot" + (i+1)+ "Missing" + theMissingPart, id);
                aCard.Add(card);

                card = new Card("robot" + (i+1) + theMissingPart, id);
                aCard.Add(card);
                id++;
            }
        }
    }

    private void FlipCardFaceUp(Card card)
    {
        card.isFaceUp = true;

        if(aCardsFlipped.IndexOf(card) < 0)
        {
            aCardsFlipped.Add(card);
            if(aCardsFlipped.Count == 2)
            {
                playerCanClick = false;

                System.Threading.Thread.Sleep(1000);

                if(((Card)aCardsFlipped[0]).id == ((Card)aCardsFlipped[1]).id)
                {
                    // Match!
                    ((Card)aCardsFlipped[0]).isMatched = true;
                    ((Card)aCardsFlipped[1]).isMatched = true;

                    matches ++;
                    if(matches >= matchesNeeded)
                    {
                        playerHasWon = true;
                    }
                }
                else
                {
                    ((Card)aCardsFlipped[0]).isFaceUp = false;
                    ((Card)aCardsFlipped[1]).isFaceUp = false;
                }
                aCardsFlipped = new ArrayList();
                playerCanClick = true;
            }
        }
    }

    private void BuildWinPrompt()
    {
        int winPromptW = 100;
        int winPromptH = 90;

        float halfScreenW = Screen.width/2;
        float halfScreenH = Screen.height/2;
        int halfPromptW = winPromptW/2;
        int halfPromptH = winPromptH/2;
        GUI.BeginGroup(new Rect(halfScreenW-halfPromptW,halfScreenH-halfPromptH, winPromptW, winPromptH));
            GUI.Box(new Rect(0,0,winPromptW,winPromptH),"A Winner is You!!");
            if(GUI.Button(new Rect(10,40,80,20),"Play Again"))
            {
                Application.LoadLevel("Title");
            }
        GUI.EndGroup();
    }
}

class Card : System.Object
{
    public bool isFaceUp = false;
    public bool isMatched = false;
    public string img;
    public int id;

    public Card()
    {
        img = "robot";
    }

    public Card(string name, int id){
        img = name; 
        this.id = id;
    }
}
