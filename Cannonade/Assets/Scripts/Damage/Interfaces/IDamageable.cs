namespace Damage.Interfaces
{
    public interface IDamageable<in T> where T : struct
    {
        public void Damage(T damage);
    }
}