using FabulaUltimaDatabase;
using FabulaUltimaDatabase.Models;
using FabulaUltimaDataImporter.IO;

namespace FabulaUltimaDataImporter.Processor
{
    public class AttackWorkflow : IWorkflow
    {
        private readonly Instance _database;
        private readonly UserIOWrapper _userIOWrapper;        

        public AttackWorkflow(Instance db, UserIOWrapper userIOWrapper)
        {
            _database = db;
            _userIOWrapper = userIOWrapper;
        }
        public string GetName => throw new NotImplementedException();

        public WorkFlowKind Kind => WorkFlowKind.ATTACK;

        public BasicAttackEntry? InitialAttack { private get; set; }

        public IEnumerable<IWorkflow> Run()
        {
            BasicAttackEntry currentAttack;
            if (InitialAttack != null)
            {
                _userIOWrapper.WriteLine($"Creating Attack {InitialAttack.Name}");
                currentAttack = InitialAttack;
            }
            else
            {
                _userIOWrapper.WriteLine("Creating new attack");
                var attackName = _userIOWrapper.GetValidString("Attack");
                currentAttack = new BasicAttackEntry
                {
                    Id = Guid.NewGuid(),
                    Name = attackName,
                };
            }

            currentAttack.Attribute1 = _userIOWrapper.GetAttribute(1);
            currentAttack.Attribute2 = _userIOWrapper.GetAttribute(2);
            currentAttack.AttackMod = 0; // set by default

            currentAttack.DamageMod = 5; // set by default
            
            currentAttack.DamageType = GetDamageType();
            currentAttack.IsRanged = _userIOWrapper.GetBoolean($"Is this attack ranged?", "yes", "no");

            _database.CreateBasicAttack(currentAttack);
            _userIOWrapper.WriteLine("Attack created");
            yield break;
        }

        private Guid? GetDamageType()
        {
            var damageTypes = _database.GetDamageTypes().ToDictionary(dt => dt.Name.ToLowerInvariant(), dt => dt.Id);
            (bool verified, string error) OnlyAllowDamageTypes(string arg)
            {
                var isGoodValue = true;
                var errorMessage = string.Empty;
                if (!damageTypes.ContainsKey(arg.ToLowerInvariant()))
                {
                    errorMessage = "Please choose a valid value";
                    isGoodValue = false;
                }
                return (isGoodValue, errorMessage);
            };
            _userIOWrapper.WriteLine($"Choose a damage type ({string.Join(", ", damageTypes.Keys)})");
            var name = _userIOWrapper.GetValidString("Damage Type: ", additionalVerification: OnlyAllowDamageTypes);
            return damageTypes[name.ToLowerInvariant()];
        }
    
    }
}
