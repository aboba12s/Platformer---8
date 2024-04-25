using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public sealed class CollectableItem : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private int amount = 1;
    [Space]
    [SerializeField] private UnityEvent onPickedUp;
    
    private void OnValidate()
    {
        if (!item) return;
        
        GetComponent<SpriteRenderer>().sprite = item.Sprite;
        amount = Math.Clamp(amount, 0, item.MaxAmount);
    }

    public void PickUp(Inventory inventory)
    {
        if (!inventory) return;
        amount -= inventory.PickUp(item, amount);
        
        if (amount == 0)
        {
            onPickedUp?.Invoke();
            gameObject.SetActive(false);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        PickUp(other.GetComponent<Inventory>());
    }
}