namespace Inverse.Domain.Model
{
    public abstract class Entity<T>
    {
        public T Id { get; set; }

        protected Entity()
        {
            Id = default;
        }

        public override bool Equals(object obj)
        {
            return obj is Entity<T> entity && Id.Equals(entity.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() * 554;
        }
    }
}
