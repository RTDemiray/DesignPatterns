namespace BaseProject.Composite
{
    public interface IComponent
    {
        public int Id { get; }
        public string Name { get; }
        int Count();
        string Display();
    }
}