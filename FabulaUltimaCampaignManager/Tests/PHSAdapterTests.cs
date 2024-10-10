using FabulaUltimaGMTool.Adaptors;
using FirstProject.Npc;
using GdUnit4;
using System;
using System.Reflection;
using static GdUnit4.Assertions;

namespace FabulaUltimaGMTool.Tests
{
    [TestSuite]
    public class PHSAdapterTests
    {
        [TestCase(false, "DEX")]
        [TestCase(true, "")]
        public void ToDataFormat_Armor_Success(bool defOverrides, string expectedDefenseDice)
        {
            // Arrange
            const int EXPECTED_NUMBER_PROPERTIES = 10;
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
                    DefenseOverrides = defOverrides,
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
            AssertThat(armorData.initiative).IsEqual(armor.Modifiers.InitiativeModifier);
            AssertThat(armorData.defenseDice).IsEqual(expectedDefenseDice);
            AssertThat(armorData.defenseConstant).IsEqual(armor.Modifiers.DefenseModifier);
            AssertThat(armorData.mDefenseDice).IsEqual("INS");
            AssertThat(armorData.mDefenseConstant).IsEqual(armor.Modifiers.MagicDefenseModifier);
            AssertThat((bool)armorData.basic).IsTrue();
            Assert(armorData.GetType(), EXPECTED_NUMBER_PROPERTIES);            
        }

        
        [TestCase]
        public void ToDataFormat_Shield_Success()
        {
            // Arrange
            const int EXPECTED_NUMBER_PROPERTIES = 18;
            var shield = new NpcEquipment
            {
                Name = "Vorpal Shield",
                Cost = 6456,
                Quality = "Nizzles and Nax",
                IsMartial = true,
                Category = new NpcEquipmentCategory
                {
                    IsArmor = true,
                    EquipmentCategory = new FabulaUltimaNpc.EquipmentCategory
                    {
                        Name = "Shield",
                        IsArmor = true,
                    }
                },
                Modifiers = new NpcEquipmentModifiers
                {
                    DefenseModifier = 2,
                    DefenseOverrides = false,
                    MagicDefenseModifier = 5,
                    InitiativeModifier = 0,
                }
            };
            // Act

            var shieldData = PHSAdapter.ToDataFormat(shield);

            // Assert
            AssertThat(shieldData.name).IsEqual(shield.Name);
            AssertThat(shieldData.type).IsEqual(shield.Category.Name);
            AssertThat(shieldData.cost).IsEqual(shield.Cost);
            AssertThat(shieldData.accuracy).IsEqual(string.Empty);
            AssertThat(shieldData.category).IsEqual(string.Empty);
            AssertThat(shieldData.accuracyConstant).IsEqual(0);
            AssertThat(shieldData.damageConstant).IsEqual(0);
            AssertThat(shieldData.damage).IsEqual(string.Empty);
            AssertThat(shieldData.handedness).IsEqual(string.Empty);
            AssertThat(shieldData.range).IsEqual(string.Empty);
            AssertThat(shieldData.dice1).IsEqual(string.Empty);
            AssertThat(shieldData.dice2).IsEqual(string.Empty);
            AssertThat(shieldData.quality).IsEqual(shield.Quality);
            AssertThat(shieldData.martial).IsEqual(shield.IsMartial);
            AssertThat(shieldData.initiative).IsEqual(shield.Modifiers.InitiativeModifier);
            AssertThat((int)shieldData.defense).IsEqual(shield.Modifiers.DefenseModifier);
            AssertThat((int)shieldData.mDefense).IsEqual(shield.Modifiers.MagicDefenseModifier);
            AssertThat((bool)shieldData.basic).IsTrue();
            Assert(shieldData.GetType(), EXPECTED_NUMBER_PROPERTIES);
        }

        private void Assert(Type anonObjType, int expectedPropertyCount)
        {
            PropertyInfo[] anonObjProperties = anonObjType.GetProperties();
            AssertThat(anonObjProperties.Length).IsEqual(expectedPropertyCount);
        }
    }
}
