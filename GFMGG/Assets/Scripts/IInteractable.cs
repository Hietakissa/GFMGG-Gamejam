public interface IInteractable
{
    public virtual bool IsInteractable() => true;
    public virtual string GetInteractionText() => "Interact";

    public abstract void Interact();
}