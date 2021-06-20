using Core.Items;

namespace Unity.Scenes.Shared.Status
{
    public interface IItemView
    {
        void UpdateModel(IItem item);
    }
}