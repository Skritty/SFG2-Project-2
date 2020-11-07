using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Deck<T>
{
    List<T> cards = new List<T>();

    public Action Changed = delegate { };
    public Action Emptied = delegate { };
    public Action<T> CardAdded = delegate { };
    public Action<T> CardRemoved = delegate { };

    public int Count => cards.Count;
    public T TopItem => cards[cards.Count - 1];
    public T BottomItem => cards[0];
    public bool IsEmpty => cards.Count == 0;
    public int LastIndex
    {
        get
        {
            if (cards.Count == 0) return 0;
            else return cards.Count - 1;
        }
    }

    public int GetIndexFromPosition(DeckPosition position)
    {
        int index = 0;
        switch (position)
        {
            case DeckPosition.Middle:
                index = UnityEngine.Random.Range(0, LastIndex);
                break;
            case DeckPosition.Top:
                index = LastIndex;
                break;
        }
        return index;
    }

    public void Add(T card, DeckPosition position = DeckPosition.Top)
    {
        if (card == null) return;
        int index = GetIndexFromPosition(position);
        if (index == LastIndex) cards.Add(card);
        else cards.Insert(index, card);
        Changed.Invoke();
        CardAdded?.Invoke(card);
    }

    public void Add(List<T> cards, DeckPosition position = DeckPosition.Top)
    {
        foreach(T card in cards)
        {
            Add(card, position);
        }
    }

    public T Draw(DeckPosition position = DeckPosition.Top)
    {
        if (IsEmpty) return default;
        int index = GetIndexFromPosition(position);
        T card = cards[index];
        Remove(index);
        return card;
    }

    public void Remove(int index)
    {
        if (IsEmpty || !IndexWithinRange(index)) return;
        T removed = cards[index];
        cards.RemoveAt(index);
        CardRemoved.Invoke(removed);
        Changed.Invoke();
        if (Count == 0) Emptied.Invoke();
    }

    private bool IndexWithinRange(int index)
    {
        if (index >= Count || index < 0) return false;
        return true;
    }

    public T GetCard(int index)
    {
        if (!IndexWithinRange(index)) return default;
        if (cards[index] != null) return cards[index];
        else return default;
    }

    public void Shuffle()
    {
        for(int i = 0; i < Count - 1; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i+1, Count);
            T randomCard = cards[randomIndex];
            cards[randomIndex] = cards[i];
            cards[i] = randomCard;
        }
    }
}
