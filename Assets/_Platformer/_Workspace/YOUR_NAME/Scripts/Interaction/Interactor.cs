using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Interactor : MonoBehaviour
{
    private List<IInteractive> _interactiveObjects = new();

    private void OnTriggerEnter2D(Collider2D other)
    {
        var interactive = other.GetComponent<IInteractive>();
        if (interactive != null) _interactiveObjects.Add(interactive);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var interactive = other.GetComponent<IInteractive>();
        if (interactive != null) _interactiveObjects.Remove(interactive);
    }
    
    private void Sort()
    {
        if (!HasInteractions()) return;
        _interactiveObjects = _interactiveObjects.OrderBy(x => x.Priority).ToList();
    }
    
    public void Interact()
    {
        Sort();
        var interactive = _interactiveObjects.LastOrDefault(x => x.CanInteractWith(this));
        interactive?.Interact(this);
    }

    public bool HasInteractions()
    {
        return _interactiveObjects.Count(x => x.CanInteractWith(this)) > 0;
    }
}
