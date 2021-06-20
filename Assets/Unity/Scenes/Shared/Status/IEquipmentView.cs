using Core.EquipMap;

namespace Unity.Scenes.Shared.Status
{
    public interface IEquipmentView
    {
        void UpdateModel(IEquipMap equipMap);
    }
}