using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomize<T>
{
    private RandomizeItem[] _items;

    public Randomize(T[] items, int[] itemChances)
    {
        this._items = new RandomizeItem[items.Length];

        for (int i = 0; i < items.Length; i++)
        {
            _items[i] = new RandomizeItem(itemChances[i], items[i] as Object);
        }
    }

    public RandomizeItem Run()
    {
        // Check input
        if (_items.Length == 0)
        {
            return null;
        }

        int chanceTotal = 0;

        for (int i = 0; i < _items.Length; i++)
        {
            chanceTotal += _items[i].Chance;
        }


        int randomChance = Random.Range(0, chanceTotal) + 1;

        int accumulatedProbability = 0;

        for (int i = 0; i < _items.Length; i++)
        {
            accumulatedProbability += _items[i].Chance;

            if (randomChance <= accumulatedProbability)
            {
                ///Decrease item chance in range, if = min so change = max else change decrease
                _items[i].Chance--;
                // reset chance of the item
                _items[i].Chance = _items[i].Chance <= 0 ? _items[i].MaxChance : _items[i].Chance;

                return _items[i];
            }
        }

        return null;
    }

}
