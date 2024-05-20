using FabulaUltimaDatabase;
using FabulaUltimaDatabase.Models;
using FabulaUltimaDataImporter.IO;
using FabulaUltimaSkillLibrary;
using FabulaUltimaSkillLibrary.Models;
using System;
using System.Net.Mail;

namespace FabulaUltimaDataImporter.Processor
{
    internal class BeastWorkflow : IWorkflow
    {
        private readonly Instance _database;
        private readonly UserIOWrapper _userIOWrapper;
        private readonly IWorkflowFactory _workflowFactory;

        public string GetName => "BeastWorkflow";

        public WorkFlowKind Kind => WorkFlowKind.BEAST;

        public BeastWorkflow(Instance db, UserIOWrapper userIOWrapper, IWorkflowFactory workflowFactory)
        {
            _database = db;
            _userIOWrapper = userIOWrapper;
            _workflowFactory = workflowFactory;
        }
        public IEnumerable<IWorkflow> Run()
        {
            _userIOWrapper.WriteLine("Enter Beast details");
            var name = _userIOWrapper.GetValidString("Name");
            // todo: verify no clones based on exact name match (case insensitive)

            var description = _userIOWrapper.GetValidString("Description");

            var level = _userIOWrapper.GetUnsignedInt("Level", min: 5, max: 60);

            var traits = _userIOWrapper.GetValidString("Traits", true);

            var species = GetSpecies();

            var dex = GetDiceSize("Dexterity");
            var ins = GetDiceSize("Insight");
            var mgt = GetDiceSize("Might");
            var wlp = GetDiceSize("WillPower");
            var filename = GetImageFile();
                        
            var resistances = _database.GetDamageTypes()
                .Where(t => t.Name != "No Damage") // exclude for obvious reasons
                .Select(d =>
                {
                    var affinity = GetAffinity(d.Name);
                    return new Resistance(d, affinity);
                })
                .ToList();

            var speciesCanHaveEquipment = CanHaveEquipment(species);
            var skillInput = GetPrintedStats(speciesCanHaveEquipment);
            
            ICollection<EquipmentEntry> equipment;
            if(speciesCanHaveEquipment)
            {
                equipment = GetEquipment();
                foreach (var item in equipment.Where(a => !a.CategoryId.HasValue))
                {
                    yield return _workflowFactory.GenerateWorkflow(WorkFlowKind.EQUIPMENT, item);
                }
            }
            else
            {
                equipment = new List<EquipmentEntry>();
            }
            
            var attacks = GetAttacks(speciesCanHaveEquipment);
            foreach(var attack in attacks.Where(a => !a.Id.HasValue))
            {
                attack.Id = Guid.NewGuid();               
                yield return _workflowFactory.GenerateWorkflow(WorkFlowKind.ATTACK, attack);
            }

            var spells = GetSpells();
            foreach (var spell in spells.Where(s => !s.Id.HasValue))
            {
                spell.Id = Guid.NewGuid();
                yield return _workflowFactory.GenerateWorkflow(WorkFlowKind.SPELL, spell);
            }

            var actions = GetActions();
            foreach(var action in actions.Where(a => !a.Id.HasValue))
            {
                action.Id = Guid.NewGuid();
                yield return _workflowFactory.GenerateWorkflow(WorkFlowKind.ACTION, action);
            }

            var entry = new BeastiaryEntry()
            {
                Name = name,
                Description = description,
                Level = level,
                Traits = traits,
                Species = species,
                Insight = ins,
                Dexterity = dex,
                Might = mgt,
                WillPower = wlp,
                ImageFile = filename
            };

            var id = _database.CreateBeast(entry, resistances, attacks, spells, equipment, actions);

            skillInput.BeastId = id;

            yield return _workflowFactory.GenerateWorkflow(WorkFlowKind.SKILL, skillInput);
        }

        private ICollection<ActionEntry> GetActions()
        {
            var actions = _database.GetActions().ToDictionary(a => a.Name, a => a);
            _userIOWrapper.WriteLine("Configure Spells");

            var result = new List<ActionEntry>();
            if (!_userIOWrapper.WillUserContinue("Does this beast have other actions?"))
            {
                return result;
            }

            while (true)
            {
                var actionName = _userIOWrapper.GetValidString("Action Name");
                if (actions.ContainsKey(actionName))
                {
                    _userIOWrapper.WriteLine("Found Action");
                    result.Add(actions[actionName]);
                }
                else
                {
                    _userIOWrapper.WriteLine("didn't find Action. Marking for later creation");
                    result.Add(new ActionEntry { Name = actionName });
                }
                if (!_userIOWrapper.WillUserContinue("Another Action?"))
                {
                    break;
                }
            }
            return result;
        }

        private bool CanHaveEquipment(Guid speciesId)
        {   
            var barredSpeciesId = Guid.Parse(KnownSkills.UseEquipment.OtherAttributes[SpeciesConstants.BLOCKED_SPECIES]);
            return speciesId != barredSpeciesId;
        }

        private SkillInputData GetPrintedStats(bool speciesCanHaveEquipment)
        {
            _userIOWrapper.WriteLine("Enter in Sheet Stats");
            var maxHp = (int) _userIOWrapper.GetUnsignedInt("Max HP");
            var maxMp = (int) _userIOWrapper.GetUnsignedInt("Max MP");
            var mDefVal = (int)_userIOWrapper.GetUnsignedInt("MDef mod", 0);
            var defVal = (int) _userIOWrapper.GetUnsignedInt("Def Value", 0);
            var isOverride = speciesCanHaveEquipment ? _userIOWrapper.GetBoolean("Is this an Override?", "yes", "no") : false;
            var initiative = (int) _userIOWrapper.GetUnsignedInt("Initiative");

            return new SkillInputData
            {
                DefMod = isOverride == true ? 0 : defVal,
                DefOverride = isOverride == true ? null : defVal,
                MDefMod = mDefVal,
                Init = initiative,
                MaxHP = maxHp,
                MaxMP = maxMp,                
            };
        }

        private ICollection<EquipmentEntry> GetEquipment()
        {
            var equipmentMap = _database.GetEquipment().ToDictionary(s => s.Name, s => s);
            
            _userIOWrapper.WriteLine("Configure Spells");

            var result = new List<EquipmentEntry>();

            if (!_userIOWrapper.WillUserContinue("Does this beast have equipment?"))
            {
                return result;
            }
            while (true)
            {
                var equipmentName = _userIOWrapper.GetValidString("Equipment");
                if (equipmentMap.ContainsKey(equipmentName))
                {
                    _userIOWrapper.WriteLine("Found Equipment");
                    var equipment = equipmentMap[equipmentName];
                    result.Add(equipment);                  
                }
                else
                {
                    _userIOWrapper.WriteLine("didn't find equipment. Marking for later creation");
                    result.Add(new EquipmentEntry { Id = Guid.NewGuid(), Name = equipmentName });
                }

                if (!_userIOWrapper.WillUserContinue("Another Piece of Equipment?"))
                {
                    break;
                }
            }
            return result;
        }

        private ICollection<SpellEntry> GetSpells()
        {
            var spells = _database.GetSpells().ToDictionary(s => s.Name, s => s);
            _userIOWrapper.WriteLine("Configure Spells");

            var result = new List<SpellEntry>();
            if (!_userIOWrapper.WillUserContinue("Does this beast have spells?"))
            {
                return result;
            }
            
            while (true)
            {
                var spellName = _userIOWrapper.GetValidString("Spell Name");
                if (spells.ContainsKey(spellName))
                {
                    _userIOWrapper.WriteLine("Found Spell");
                    result.Add(spells[spellName]);
                }
                else
                {
                    _userIOWrapper.WriteLine("didn't find Spell. Marking for later creation");
                    result.Add(new SpellEntry { Name = spellName });
                }
                if (!_userIOWrapper.WillUserContinue("Another Spell?"))
                {
                    break;
                }
            }
            return result;
        }

        private ICollection<BasicAttackEntry> GetAttacks(bool speciesCanHaveEquipment)
        {
            var attacks = _database.GetBasicAttacks().ToDictionary(a => a.Name, a => a);
            _userIOWrapper.WriteLine("Configure Attacks");

            var result = new List<BasicAttackEntry>();
            if (!speciesCanHaveEquipment && !_userIOWrapper.WillUserContinue("Does this npc have attacks?"))
            {
                return result;
            }
            while (true)
            {
                var attackName = _userIOWrapper.GetValidString("Attack Name");
                if (attacks.ContainsKey(attackName))
                {
                    _userIOWrapper.WriteLine("Found attack");
                    result.Add(attacks[attackName]);
                }
                else
                {
                    _userIOWrapper.WriteLine("didn't find attack. Marking for later creation");
                    result.Add(new BasicAttackEntry { Name = attackName });
                }
                if(! _userIOWrapper.WillUserContinue("Another Attack?")) 
                {
                    break;
                }
            }
            return result;
        }

        private Affinity GetAffinity(string damageType)
        {
            var affinityList = _database.GetAffinities();
            (bool verified, string error) VerifyAffinity(string affinityName)
            {

                var errors = new List<string>();
                if (!affinityList.Any(s => string.Equals(s.Name, affinityName, StringComparison.InvariantCultureIgnoreCase)) &&
                    !affinityList.Any(s => string.Equals(s.ShortName, affinityName, StringComparison.InvariantCultureIgnoreCase))
                    )
                    errors.Add("unknown affinity");
                // todo: add more 
                return (!errors.Any(), string.Join(',', errors));
            };
            _userIOWrapper.WriteLine($"Choose an affinity for {damageType} Damage ({string.Join(", ", affinityList.Select(s => $"{s.ShortName} - {s.Name}"))})");
            var affinity = _userIOWrapper.GetValidString("Affinity", allowEmpty: true, additionalVerification: VerifyAffinity);
            return affinityList.Single(s => string.Equals(s.Name, affinity, StringComparison.InvariantCultureIgnoreCase) || 
                        string.Equals(s.ShortName, affinity, StringComparison.InvariantCultureIgnoreCase));
        }

        private static readonly IReadOnlySet<string> _supportedImageFileExtensions = new HashSet<string>()
        {
            ".png",
            ".jpg",
            ".jpeg"
        };
        private string GetImageFile()
        {
            _userIOWrapper.WriteLine("Image File?");
            string RemoveQuotes(string text)
            {
                string cleanText = text;
                if (cleanText.StartsWith("\"")) cleanText = cleanText.Substring(1);
                if (cleanText.EndsWith("\"")) cleanText = cleanText.Substring(0, cleanText.Length - 1);
                return cleanText;
            };

            (bool verified, string error) VerifyFilePath(string filePath)
            {
                if (string.IsNullOrWhiteSpace(filePath)) return (true, ""); // we've allowed empty below, so we don't want to handle it here
                var pathToVerify = RemoveQuotes(filePath);
                var errors = new List<string>();
                if (!File.Exists(pathToVerify)) errors.Add("filepath does not exist");
                var fileExtension = Path.GetExtension(pathToVerify);
                if (!_supportedImageFileExtensions.Contains(fileExtension)) errors.Add("file extension not supported");
                // todo: add more 
                return (!errors.Any(), string.Join(',', errors));
            };

            var result = _userIOWrapper.GetValidString("FileName", allowEmpty: true, additionalVerification: VerifyFilePath);
            var cleanResult = RemoveQuotes(result);
            return cleanResult;
        }

        private Guid GetSpecies()
        {
            var speciesList = _database.GetSpecies();
            (bool verified, string error) VerifySpecies(string speciesName)
            {
                
                var errors = new List<string>();
                if (!speciesList.Any(s => string.Equals(s.Name, speciesName, StringComparison.InvariantCultureIgnoreCase)))
                    errors.Add("unknown species");
                // todo: add more 
                return (!errors.Any(), string.Join(',', errors));
            };
            _userIOWrapper.WriteLine($"Choose a species ({string.Join(", ", speciesList.Select(s => s.Name))})");
            var name = _userIOWrapper.GetValidString("Species", additionalVerification: VerifySpecies);
            return speciesList.Single(s => string.Equals(s.Name, name, StringComparison.InvariantCultureIgnoreCase)).Id;
        }
        
        private uint GetDiceSize(string attribute)
        {
            var dice = _database.GetDice();
            _userIOWrapper.WriteLine($"Choose Dice Size ({string.Join(", ", dice.Select(d => d.Sides))}) for {attribute}");
            return _userIOWrapper.GetUnsignedInt("Dice Size", allowedValues: dice.Select(d => (int?) d.Sides).ToHashSet());
        }
    }
}
