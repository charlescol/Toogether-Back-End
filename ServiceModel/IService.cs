using AppModel.Storage;

namespace ServiceModel
{
    public interface IService<TId, TItem>
    {
        public void Save(ReferencedItem<TId, TItem> item);
        public ReferencedItem<TId, TItem> GetByID(TId id);
    }
}
