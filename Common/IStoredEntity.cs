namespace Cachy.Common
{
    public interface IStoredEntity : IEntity
    {
        public int Revision { get; set; }
        public byte[] Data { get; init; }
        public bool Active { get; init; }
        public IStoredEntity CopyAndDeactivate();
    }
}