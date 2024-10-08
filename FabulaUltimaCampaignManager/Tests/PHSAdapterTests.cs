using FabulaUltimaGMTool.Adaptors;
using FirstProject.Npc;
using GdUnit4;
using static GdUnit4.Assertions;

namespace FabulaUltimaGMTool.Tests
{
    [TestSuite]
    public class PHSAdapterTests
    {
        [TestCase]
        public void ToDataFormat_Armor_Success()
        {
            // Arrange

            var armor = new NpcEquipment
            {
                Name = "Vorpal Plate",
                Cost = 3928,
                Quality = "Snicker Snacks",
                IsMartial = true,
                Category = new NpcEquipmentCategory
                {
                    IsArmor = true
                },
                Modifiers = new NpcEquipmentModifiers
                {
                    DefenseModifier = 0,
                    MagicDefenseModifier = 1,
                    InitiativeModifier = -1,
                }
            };
            // Act

            var armorData = PHSAdapter.ToDataFormat(armor);

            // Assert
            AssertThat(armorData.name).IsEqual(armor.Name);
            AssertThat(armorData.cost).IsEqual(armor.Cost);
            AssertThat(armorData.quality).IsEqual(armor.Quality);
            AssertThat(armorData.martial).IsEqual(armor.IsMartial);
            AssertThat((string) armorData.type).IsNull();
            AssertThat((string)armorData.accuracy).IsNull();
            AssertThat((string)armorData.damage).IsNull();
            AssertThat((string)armorData.handedness).IsNull();
            AssertThat((string)armorData.range).IsNull();
            AssertThat((bool)armorData.basic).IsTrue();
        }
    }
}
