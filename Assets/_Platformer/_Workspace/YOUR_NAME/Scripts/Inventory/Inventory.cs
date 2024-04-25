using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Serializable]
    public class ItemSlot
    {
        public Item item;
        public int amount;

        public bool IsFull => amount >= item.MaxAmount;
    }
    
    [SerializeField] private List<ItemSlot> items;
    [SerializeField] private int capacity = 5;
    
    public delegate void OnItemCollected(Item item, int amount);
    public event OnItemCollected onItemCollected;
    
    public bool CanPickup(Item item)
    {
        return items.Find(x => x.item.Equals(item) && !x.IsFull) != null 
               || items.Count + 1 <= capacity;
    }
    
    public int PickUp(Item item, int amount)
    {
        if (!item || amount == 0) return 0; 
        
        var startAmount = amount;
        var stored = items.FirstOrDefault(x => x.item.Equals(item) && !x.IsFull);
        
        if (stored != null)
        {
            var oldAmount = stored.amount;
            
            stored.amount += amount;
            var delta = stored.amount - oldAmount;
            
            amount -= delta;
            
            onItemCollected?.Invoke(item, delta);
        }

        while (amount > 0)
        {
            if (!CanPickup(item)) break;
            
            var delta = Math.Min(amount, item.MaxAmount);
            items.Add(new ItemSlot
            {
                item = item,
                amount = delta
            });
            
            amount -= delta;
            
            onItemCollected?.Invoke(item, delta);
        }

        return startAmount - amount;
    }
    
    public Item Get(string id)
    {
        return items.Select(x => x.item).First(x => x.Id == id);
    }
    
    public IList<Item> GetAll()
    {
        return items.Select(x => x.item).ToList();
    } 
    
    public IList<Item> GetAll(string id)
    {
        return items.Select(x => x.item).Where(x => x.Id == id).ToList();
    }
}