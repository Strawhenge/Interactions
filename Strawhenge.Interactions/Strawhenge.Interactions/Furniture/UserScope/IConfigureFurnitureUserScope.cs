namespace Strawhenge.Interactions.Furniture
{
    public interface IConfigureFurnitureUserScope
    {
        void Set<T>(T service) where T : class;
    }
}