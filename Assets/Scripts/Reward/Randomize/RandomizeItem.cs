
using UnityEngine;

public class RandomizeItem
{
    public RandomizeItem(int maxChance, Object item)
    {
        this._maxChance = maxChance;
        this.Item = item;

        this.Chance = this._maxChance;
    }
    private int _maxChance;

    public Object Item;
    public int Chance;
    public int MaxChance { get => _maxChance; set => _maxChance = value; }

}
