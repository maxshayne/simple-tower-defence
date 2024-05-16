namespace TowerDefence.Pool
{
    public interface IPool<T>
    {
        T Take();
        void Return(T item);
    }
}