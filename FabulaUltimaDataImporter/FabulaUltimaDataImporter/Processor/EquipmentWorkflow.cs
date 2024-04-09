using FabulaUltimaDatabase;
using FabulaUltimaDatabase.Models;
using FabulaUltimaDataImporter.IO;

namespace FabulaUltimaDataImporter.Processor
{
    internal class EquipmentWorkflow : IWorkflow
    {
        private readonly Instance _database;
        private readonly UserIOWrapper _userIOWrapper;

        public EquipmentWorkflow(Instance db, UserIOWrapper userIOWrapper)
        {
            _database = db;
            _userIOWrapper = userIOWrapper;
        }
        public string GetName => nameof(EquipmentWorkflow);

        public WorkFlowKind Kind => WorkFlowKind.EQUIPMENT;

        public EquipmentEntry? InitialEquipment { private get; set; }

        public IEnumerable<IWorkflow> Run()
        {
            EquipmentEntry currentEquipment;
            if (InitialEquipment != null)
            {
                _userIOWrapper.WriteLine($"Creating item {InitialEquipment.Name}");
                currentEquipment = InitialEquipment;
            }
            else
            {
                _userIOWrapper.WriteLine("Creating new item");
                var itemName = _userIOWrapper.GetValidString("item name");
                currentEquipment = new EquipmentEntry
                {
                    Id = Guid.NewGuid(),
                    Name = itemName,
                };
            }

            currentEquipment.CategoryId = GetEquipmentCategory(out bool isWeapon, out var isArmor, out var categoryName);
            currentEquipment.Cost = (int) _userIOWrapper.GetUnsignedInt("Cost");            

            currentEquipment.IsMartial = _userIOWrapper.GetBoolean($"Is this {categoryName} Martial?", "yes", "no");
            if (isWeapon)
            {
                _userIOWrapper.WriteLine($"Adding {categoryName} attributes");
                currentEquipment.Attribute1 = _userIOWrapper.GetAttribute(1, true);
                currentEquipment.Attribute2 = _userIOWrapper.GetAttribute(2, true);                
                currentEquipment.AttackMod = (int?)_userIOWrapper.GetUnsignedInt("Attack Modifier", 0);
                currentEquipment.DamageMod = (int?)_userIOWrapper.GetUnsignedInt("Damage Modifier", 0);
                currentEquipment.DamageType = GetDamageType();                
                currentEquipment.NumHands = (int?) _userIOWrapper.GetUnsignedInt("number of hands", 1, 2);
            }
            
            if(isArmor)
            {
                var doesOverride = _userIOWrapper.GetBoolean($"Will this {categoryName} override Def?", "yes", "no");
                if(doesOverride == true)
                {
                    currentEquipment.DefenseOverride = (int?)_userIOWrapper.GetUnsignedInt("Defense Override", 0);
                }
                else
                {
                    currentEquipment.DefenseModification = (int?)_userIOWrapper.GetUnsignedInt("Defense Modifier", 0);
                }
                currentEquipment.MagicDefenceModification = (int?)_userIOWrapper.GetUnsignedInt("M.Def Modifier", 0);
                currentEquipment.InitiativeModification = (int?)_userIOWrapper.GetUnsignedInt("Initiative Modifier", 0);
            }

            var quality = _userIOWrapper.GetValidString("Quality", true);
            currentEquipment.Quality = string.IsNullOrWhiteSpace(quality) ? "No Quality" : quality;

            _database.CreateEquipment(currentEquipment);
            _userIOWrapper.WriteLine("Item created");
            yield break;
        }

        private Guid? GetEquipmentCategory(out bool isWeapon, out bool isArmor, out string categoryName)
        {
            var equipmentCategories = _database.GetEquipmentCategories().ToDictionary(c => c.Name, c => c);
            (bool verified, string error) OnlyAllowEquipmentCategories(string arg)
            {   
                var isGoodValue = true;
                var errorMessage = string.Empty;
                if (!equipmentCategories.ContainsKey(arg))
                {
                    errorMessage = "Please choose a valid value";
                    isGoodValue = false;
                }
                return (isGoodValue, errorMessage);
            };
            _userIOWrapper.WriteLine($"Choose a item type ({string.Join(", ", equipmentCategories.Keys)})");
            var name = _userIOWrapper.GetValidString("Category", additionalVerification: OnlyAllowEquipmentCategories);
            isWeapon = equipmentCategories[name].IsWeapon;
            isArmor = equipmentCategories[name].IsArmor;
            categoryName = equipmentCategories[name].Name;
            return equipmentCategories[name].Id;
        }

        private Guid? GetDamageType()
        {
            var damageTypes = _database.GetDamageTypes().ToDictionary(dt => dt.Name, dt => dt.Id);
            (bool verified, string error) OnlyAllowDamageTypes(string arg)
            {
                if (string.IsNullOrWhiteSpace(arg)) return (true, string.Empty);
                var isGoodValue = true;
                var errorMessage = string.Empty;
                if (!damageTypes.ContainsKey(arg))
                {
                    errorMessage = "Please choose a valid value";
                    isGoodValue = false;
                }
                return (isGoodValue, errorMessage);
            };
            _userIOWrapper.WriteLine($"Choose a damage type ({string.Join(", ", damageTypes.Keys)})");
            var name = _userIOWrapper.GetValidString("DamageType", additionalVerification: OnlyAllowDamageTypes, allowEmpty: true);
            return damageTypes[name];
        }
    }
}
