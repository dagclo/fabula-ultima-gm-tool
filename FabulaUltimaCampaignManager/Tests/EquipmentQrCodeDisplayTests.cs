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
                Category = new NpcEquipmentCategory
                {
                    IsArmor = true
                }
            };
            // Act

            var armorData = PHSAdapter.ToDataFormat(armor);

            //// Assert
            //AssertThat(armorData.name).IsEqual(armor.Name);
            //AssertThat(armorData.type).IsNull();
            //AssertThat(armorData.basic).IsTrue();
        }
    }
}
