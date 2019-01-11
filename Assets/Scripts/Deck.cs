using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck
{
    public GameObject panel;
    private Queue<Card> deck;
    private bool reloading;
    private Color defaultColor;

    public Card basic;
    public Card power;
    
    public Deck(GameObject panel)
    {
        this.panel = panel;
        deck = new Queue<Card>();
        reloading = false;
        defaultColor = new Color(1, 1, 1, 100f / 255f);
    }

    public IEnumerator Flash() {
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
            deck.Enqueue(basic);
        deck.Enqueue(power);
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

    public void Add (Card card) {
        deck.Enqueue(card);
    }

    public Card Pop() {
        Card c = deck.Dequeue();
        deck.Enqueue(c);
        return c;
    }
    
}
