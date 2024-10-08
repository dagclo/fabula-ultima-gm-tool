using FirstProject.Beastiary;
using FirstProject.Npc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulaUltimaGMTool.Adaptors
{
    public static class PHSAdapter
    {
        private static string ToEnglish(int num)
        {
            switch (num)
            {
                case 1: return "One";
                case 2: return "Two";
                default:
                    throw new NotImplementedException();
            }
        }

        public static dynamic ToDataFormat(NpcEquipment npcEquipment)
        {
            string type;
            bool checkHandedNess = false;
            string rangeValue = null;
            int? defaultModValue = null;
            string emptyStringDefault = null;
            int? emptyIntDefault = null;
            if (npcEquipment.Category.IsWeapon)
            {
                type = "Weapon";
                checkHandedNess = true;
                rangeValue = npcEquipment.BasicAttack.IsRanged ? "Ranged" : "Melee";
                defaultModValue = 0;
            }
            else if (npcEquipment.Category.Name == "Shield")
            {
                type = "Shield";
                emptyStringDefault = string.Empty;
                rangeValue = emptyStringDefault;
                emptyIntDefault = 0;
            }
            else if (npcEquipment.Category.IsArmor)
            {
                type = null;
            }
            else
            {
                type = "Accessory";
            }


            var equipment = new
            {
                name = npcEquipment.Name,
                cost = npcEquipment.Cost,
                quality = npcEquipment.Quality,
                type = type,
                accuracy = npcEquipment.BasicAttack != null ? $"\u3010{npcEquipment.BasicAttack.Attribute1?.ShortenAttribute()} + {npcEquipment.BasicAttack.Attribute2?.ShortenAttribute()}\u3011" : emptyStringDefault,
                damage = npcEquipment.BasicAttack != null ? $"\u3010HR + {npcEquipment.BasicAttack.DamageMod}\u3011{npcEquipment.BasicAttack.DamageType.Name}" : emptyStringDefault,
                handedness = checkHandedNess ? $"{ToEnglish(npcEquipment.NumHands)} Handed" : emptyStringDefault,
                range = rangeValue,
                martial = npcEquipment.IsMartial,
                category = npcEquipment.Category.IsWeapon ? npcEquipment.Category.Name : null,
                defense = npcEquipment.Category.IsWeapon ? (npcEquipment.Modifiers?.DefenseModifier ?? defaultModValue) : null,
                mDefense = npcEquipment.Category.IsWeapon ? (npcEquipment.Modifiers?.MagicDefenseModifier ?? defaultModValue) : null,
                dice1 = npcEquipment.BasicAttack?.Attribute1?.ShortenAttribute() ?? emptyStringDefault,
                dice2 = npcEquipment.BasicAttack?.Attribute2?.ShortenAttribute() ?? emptyStringDefault,
                accuracyConstant = npcEquipment.BasicAttack?.AttackMod ?? emptyIntDefault,
                damageConstant = npcEquipment.BasicAttack?.DamageMod ?? emptyIntDefault,
                basic = true,
                defenseDice = npcEquipment.Category.IsArmor && !npcEquipment.Modifiers.DefenseOverrides ? "DEX" : null,
                defenseConstant = npcEquipment.Category.IsArmor ? npcEquipment.Modifiers.DefenseModifier : (int?)null,
                mDefenseDice = npcEquipment.Category.IsArmor ? "INS" : null,
                mDefenseConstant = npcEquipment.Category.IsArmor ? npcEquipment.Modifiers.DefenseModifier : (int?)null,
                initiative = npcEquipment.Category.IsArmor ? npcEquipment.Modifiers.InitiativeModifier : (int?)null,
            };

            return equipment;
        }
    }
}
