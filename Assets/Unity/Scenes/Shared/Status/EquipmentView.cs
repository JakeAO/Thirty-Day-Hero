using Core.EquipMap;
using UnityEngine;

namespace Unity.Scenes.Shared.Status
{
    public class EquipmentView : MonoBehaviour, IEquipmentView
    {
        [SerializeField] private ItemView _weaponView;
        [SerializeField] private ItemView _armorView;
        [SerializeField] private ItemView _item1View;
        [SerializeField] private ItemView _item2View;

        public void UpdateModel(IEquipMap equipMap)
        {
            _weaponView.UpdateModel(equipMap[EquipmentSlot.Weapon]);
            _armorView.UpdateModel(equipMap[EquipmentSlot.Armor]);
            _item1View.UpdateModel(equipMap[EquipmentSlot.ItemA]);
            _item2View.UpdateModel(equipMap[EquipmentSlot.ItemB]);
        }
    }
}