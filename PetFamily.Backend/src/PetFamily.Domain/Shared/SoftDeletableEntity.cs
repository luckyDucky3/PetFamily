using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared;

public abstract class SoftDeletableEntity<TId> : Entity<TId> where TId : IComparable<TId>
{
    protected SoftDeletableEntity(TId id) : base(id) { }
    protected bool IsDeleted { get; set; } = false;
    
    public DateTime DeletionDate { get; protected set; }

    public virtual void Activate()
    {
        if (IsDeleted)
            IsDeleted = false;
    }

    public virtual void Deactivate()
    {
        if (!IsDeleted)
        {
            IsDeleted = true;
            DeletionDate = DateTime.UtcNow;
        }
    }
}