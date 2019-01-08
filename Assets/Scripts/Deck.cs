using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck
{
    private GameObject panel;
    private Queue<Card> deck;
    private bool reloading;
    private Color defaultColor;

    public class Card {
        public string name;
        public int damage;
        public float reload;
    }
    
    public static Card Basic {
        get {
            Card c = new Card();
            c.name = "Basic Shot";
            c.damage = 10;
            c.reload = .75f;
            return c;
        }
    }
    public static Card Power {
        get {
            Card c = new Card();
            c.name = "Power Shot";
            c.damage = 20;
            c.reload = 1f;
            return c;
        }
    }
    
    public Deck(GameObject panel)
    {
        this.panel = panel;
        deck = new Queue<Card>();
        reloading = false;
        defaultColor = panel.GetComponent<Image>().color;
    }

    public IEnumerator flash() {
        Image image = panel.GetComponent<Image>();
        image.color = new Color(0, 255, 0, 255);
        yield return new WaitForSeconds(.1f);
        image.color = defaultColor;
    }

    public void SetReloading(bool reloading) {
        this.reloading = reloading;
    }

    public bool IsReloading() {
        return this.reloading;
    }

    public IEnumerator Reload(float time) {
        yield return new WaitForSeconds(time);
        reloading = false;
    }

    public void InitBasicDeck() {
        for (int i = 0; i < 4; i++)
            deck.Enqueue(Basic);
        deck.Enqueue(Power);
        UpdatePanel();
    }

    public void UpdatePanel () {
        if (panel == null) {
            Debug.Log("fail");
        }
        Card top = deck.Peek();
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

    public Card Pop() {
        Card c = deck.Dequeue();
        deck.Enqueue(c);
        return c;
    }
    
}
