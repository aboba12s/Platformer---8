public interface IInteractive
{
    int Priority { get; }
    void Interact(Interactor instigator);
    bool CanInteractWith(Interactor instigator);
}