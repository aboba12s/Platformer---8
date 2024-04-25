using System;
using UnityEngine;
using YOUR_NAME;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class Item : ScriptableObject
{
    [Guid]
    [SerializeField] private string id;
        
    [Header("Item")]
    [SerializeField] private string title;
    [SerializeField] private Sprite sprite;
    [SerializeField, Min(0)] private int maxAmount = 10;
    
    [Header("Info")]
    [TextArea(3, 10)]
    [SerializeField] private string description;
    
    public string Id => id;
    public string Title => title;
    public Sprite Sprite => sprite;
    
    public string Description => description;

    public int MaxAmount => maxAmount;
    
    #region Equality check
    
    protected bool Equals(Item other)
    {
        return id == other.id;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Item)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), id);
    }
    
    #endregion
}