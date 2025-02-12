namespace PetFamily.Domain.Shared;

public interface ISoftDeletable
{
    public bool IsDeleted { get; }
    public void Activate();
    public void Deactivate();
}