using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck
{
    public GameObject up_panel;
    public GameObject left_panel;
    public GameObject right_panel;
    public GameObject down_panel;

    public Queue<Card> up_deck;
    public Queue<Card> left_deck;
    public Queue<Card> right_deck;
    public Queue<Card> down_deck;


    public class Card {
        public string name;
        public int damage;
        public float reload;
    }
    
    public static Card Basic_up {
        get {
            Card c = new Card();
            c.name = "Basic Shot";
            c.damage = 10;
            c.reload = .25f;
            return c;
        }
    }
    public static Card Power_up {
        get {
            Card c = new Card();
            c.name = "Power Shot";
            c.damage = 20;
            c.reload = .25f;
            return c;
        }
    }

    // Start is called before the first frame update
    public Deck()
    {
        up_panel = GameObject.Find("UpCard_Panel");
        left_panel = GameObject.Find("LeftCard_Panel");
        down_panel = GameObject.Find("DownCard_Panel");
        right_panel = GameObject.Find("RightCard_Panel");

        up_deck = new Queue<Card>();
        down_deck = new Queue<Card>();
        left_deck = new Queue<Card>();
        right_deck = new Queue<Card>();
        Debug.Log("started");
    }

    public void init_basic_decks() {
        for (int i = 0; i < 4; i++)
            up_deck.Enqueue(Basic_up);
        up_deck.Enqueue(Power_up);
        Update_Panel(up_deck, up_panel);
        down_deck.Enqueue(Basic_up);
        Update_Panel(down_deck, down_panel);
        left_deck.Enqueue(Basic_up);
        Update_Panel(left_deck, left_panel);
        right_deck.Enqueue(Basic_up);
        Update_Panel(right_deck, right_panel);
    }

    public static void Update_Panel (Queue<Card> q, GameObject panel) {
        if (panel == null) {
            Debug.Log("fail");
        }
        Card top = q.Peek();
        Text[] texts = panel.GetComponentsInChildren<Text>();
        foreach (Text t in texts) {
            switch (t.name) {
                case "Name_Text":
                    t.text = top.name;
                    break;
                case "Reload_Text":
                    t.text = "Rld: " + String.Format("{0:.00}", top.reload);
                    break;
                case "Damage_Text":
                    t.text = "Dmg: " + top.damage;
                    break;
                default:
                    Debug.Log(t.name);
                    break;
            }
        }
    }

    public static Card pop(Queue<Card> q) {
        Card c = q.Dequeue();
        q.Enqueue(c);
        return c;
    }

    // Update is called once per frame
}
