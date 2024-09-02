using Dapper;
using FabulaUltimaDatabase.Configuration;
using FabulaUltimaDatabase.Models;
using FabulaUltimaNpc;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;

namespace FabulaUltimaDatabase
{
    public class Instance
    {
        private readonly DatabaseConfiguration _configuration;

        public Instance(DatabaseConfiguration configuration)
        {
            _configuration = configuration;
            _configuration.InitializeDatabase();            
        }


        public virtual Guid CreateBeast(
            BeastiaryEntry entry, 
            ICollection<Resistance> resistances, 
            ICollection<BasicAttackEntry> attacks, 
            ICollection<SpellEntry> spells,
            ICollection<EquipmentEntry> equipment,
            ICollection<ActionEntry> actions)
        {
            var templateId = Guid.NewGuid();
            var templateIdString = templateId.ToString();

            using (var connection = _configuration.GetConnection()) 
            { 
                connection.Open();
                connection.Execute(@"
                    INSERT INTO BeastTemplate (Id, Name, Description, Level, Traits, Species, Dexterity, Insight, Might, Willpower, ImageFile)
                    VALUES (@Id, @Name, @Description, @Level, @Traits, @Species, @Dexterity, @Insight, @Might, @Willpower, @ImageFile)
                ",
                new
                {
                    Id = templateIdString,
                    Name = entry.Name,
                    Description = entry.Description,
                    Level = entry.Level,
                    Traits = entry.Traits,
                    Species = entry.Species,
                    Dexterity = entry.Dexterity,
                    Insight = entry.Insight,
                    Might = entry.Might,
                    Willpower = entry.WillPower,                    
                    ImageFile = entry.ImageFile
                });

                connection.Execute(@"
                    INSERT INTO BeastResistance (BeastTemplateId, DamageTypeId, AffinityId)
                    VALUES (@BeastTemplateId, @DamageTypeId, @AffinityId)
                ",
                resistances.Select(r => new { BeastTemplateId = templateIdString, DamageTypeId = r.DamageTypeId, AffinityId = r.AffinityId })
                );

                connection.Execute(@"
                    INSERT INTO BeastAttack (BeastTemplateId, BasicAttackId)
                    VALUES (@BeastTemplateId, @BasicAttackId)
                ",
                 attacks.Select(a => new { BeastTemplateId = templateIdString, BasicAttackId = a.Id }));

                connection.Execute(@"
                    INSERT INTO BeastSpell (BeastTemplateId, SpellId)
                    VALUES (@BeastTemplateId, @SpellId)
                ",
                 spells.Select(a => new { BeastTemplateId = templateIdString, SpellId = a.Id }));

                connection.Execute(@"
                    INSERT INTO BeastEquipment (BeastTemplateId, EquipmentId)
                    VALUES (@BeastTemplateId, @EquipmentId)
                ",
                 equipment.Select(e => new { BeastTemplateId = templateIdString, EquipmentId = e.Id }));

                connection.Execute(@"
                    INSERT INTO BeastAction (BeastTemplateId, ActionId)
                    VALUES (@BeastTemplateId, @ActionId)
                ",
                 actions.Select(a => new { BeastTemplateId = templateIdString, ActionId = a.Id }));

                return templateId;
            }
        }

        public virtual IEnumerable<IBeastTemplate> GetBeasts()
        {
            var species = GetSpecies().ToDictionary(s => s.Id, s => new Species { Id = s.Id, Name = s.Name });
            var affinites = GetAffinities().ToDictionary(a => a.Id, a => a);
            var damageTypes = GetDamageTypes().ToDictionary(d => d.Id, d => d);
            var resistances = GetResistances()
                .GroupBy(r => r.BeastTemplateId, r => r)
                .ToDictionary(g => g.Key, g => g.ToDictionary(r => damageTypes[r.DamageTypeId].Name, r => new BeastResistance 
                { 
                    DamageType = damageTypes[r.DamageTypeId].Name, 
                    DamageTypeId = r.DamageTypeId,
                    Affinity = affinites[r.AffinityId].ShortName,
                    AffinityId = r.AffinityId,
                }));
            
            var attacks = GetBasicAttacks().ToDictionary(a => a.Id, a => a);
            var ownedAttacks = GetBeastAttacks()
                .GroupBy(ba => ba.BeastTemplateId.Value, ba => ba)
                .ToDictionary(g => g.Key, g => (IEnumerable<BeastAttack>) g);
            
            var spells = GetSpells().ToDictionary(s => s.Id, s => s);
            var ownedSpells = GetBeastSpells()
                .GroupBy(bs => bs.BeastTemplateId, bs => bs)
                .ToDictionary(g => g.Key, g => (IEnumerable<BeastSpell>)g);

            var equipment = GetEquipment().ToDictionary(e => e.Id, e => e);
            var equipmentCategories = GetEquipmentCategories().ToDictionary(ec => ec.Id, ec => ec);
            var ownedEquipment = GetBeastEquipment()
                .GroupBy(be => be.BeastTemplateId, be => be)
                .ToDictionary(g => g.Key, g => (IEnumerable<BeastEquipment>)g);

            var skills = GetSkills().ToDictionary(s => s.Id, s => s);
            var assignedSkills = GetBeastSkills()
                                    .GroupBy(bs => bs.BeastTemplateId, bs => bs)
                                    .ToDictionary(g => g.Key, g => (IEnumerable<BeastSkillEntry>) g);

            var actions = GetActions().ToDictionary(s => s.Id, s => s);
            var ownedActions = GetBeastActions()
                .GroupBy(bs => bs.BeastTemplateId, bs => bs)
                .ToDictionary(g => g.Key, g => (IEnumerable<BeastAction>)g);

            using (var connection = _configuration.GetConnection())
            {
                connection.Open();
                var beasts = connection.Query(@"
                    SELECT Id, Name, Description, Level, Traits, Species, Dexterity, Insight, Might, Willpower, ImageFile FROM BeastTemplate
                ");                

                return beasts.Select(b =>
                {
                    Guid id = Guid.Parse(b.Id);

                    var speciesType = species[Guid.Parse((string)b.Species)].ToSpeciesType();
                    var resistance = resistances.TryGetValue(id, out var resistanceMap) ? resistanceMap : throw new Exception($"beast {id}:{b.Name} has no resistances listed");

                    return new BeastTemplate( new BeastModel
                    {
                        Id = id,
                        Name = b.Name,
                        Description = b.Description,
                        Level = (int)b.Level,
                        Traits = b.Traits,
                        Species = speciesType,
                        Dexterity = ((int?)b.Dexterity).ToDie(),
                        Insight = ((int?)b.Insight).ToDie(),
                        Might = ((int?)b.Might).ToDie(),
                        WillPower = ((int?)b.Willpower).ToDie(),
                        ImageFile = b.ImageFile,
                        Resistances = resistance,
                        BasicAttacks = ExtractAttacks(id, attacks, ownedAttacks, damageTypes, skills, assignedSkills).ToArray(),
                        Spells = ExtractSpells(id, spells, ownedSpells).ToArray(),
                        Equipment = ExtractEquipment(id, equipment, ownedEquipment, damageTypes, equipmentCategories, skills, assignedSkills).ToArray(),
                        Skills = ExtractSkills(id, skills, assignedSkills).ToArray(),
                        Actions = ExtractActions(id, actions, ownedActions).ToArray(),
                    });
                }
                ).ToList();
            }
        }

        private IEnumerable<ActionTemplate> ExtractActions(Guid id, IDictionary<Guid?, ActionEntry> actionMap, IDictionary<Guid, IEnumerable<BeastAction>> ownedActions)
        {
            if (!ownedActions.ContainsKey(id))
            {
                return Enumerable.Empty<ActionTemplate>().ToArray();
            }

            return ownedActions[id]
               .Select(bs => actionMap[bs.ActionId])
               .Select(s =>
               new ActionTemplate
               {

                   Id = s.Id.Value,
                   Name = s.Name,
                   Effect = s.Effect
               });
        }

        private IEnumerable<BeastAction> GetBeastActions()
        {
            using (var connection = _configuration.GetConnection())
            {
                connection.Open();
                var beastAttacks = connection.Query(@"
                    SELECT BeastTemplateId, ActionId FROM BeastAction
                ");
                return beastAttacks
                    .Select(r =>
                        new BeastAction
                        {
                            BeastTemplateId = Guid.Parse(r.BeastTemplateId),
                            ActionId = Guid.Parse(r.ActionId)
                        })
                    .ToList();
            }
        }

        private IEnumerable<SkillTemplate> ExtractSkills(
            Guid id, 
            IDictionary<Guid, SkillTemplate> skillMap, 
            IDictionary<Guid, IEnumerable<BeastSkillEntry>> assignedSkills)
        {
            if(assignedSkills.TryGetValue(id, out var value))
            {
                return value.Select(bs => skillMap[bs.SkillId]);
            }

            return new SkillTemplate[0];
        }

        private IEnumerable<BeastSkillEntry> GetBeastSkills()
        {
            using var connection = _configuration.GetConnection();
            connection.Open();

            var selectSQL = @"SELECT BeastTemplateId, SkillId, BasicAttackId FROM BeastSkill";
            var results = connection.Query(selectSQL);

            return results.Select(r => new BeastSkillEntry
            {
                BeastTemplateId = Guid.Parse(r.BeastTemplateId),
                SkillId = Guid.Parse(r.SkillId),
                BasicAttackId = !string.IsNullOrWhiteSpace(r.BasicAttackId) ? Guid.Parse(r.BasicAttackId) : (Guid?) null,
            });
        }

        private IEnumerable<EquipmentTemplate> ExtractEquipment(
            Guid id, 
            IDictionary<Guid?, EquipmentEntry> equipmentMap, 
            IDictionary<Guid, IEnumerable<BeastEquipment>> ownedEquipment,
            IDictionary<Guid, DamageTypeEntry> damageTypes,
            IDictionary<Guid, EquipmentCategory> equipmentCategories,
            Dictionary<Guid, SkillTemplate> skillMap,
            IDictionary<Guid, IEnumerable<BeastSkillEntry>> assignedSkills)
        {

            if (!ownedEquipment.ContainsKey(id))
            {
                return Enumerable.Empty<EquipmentTemplate>().ToArray();
            }

            ICollection<BeastSkillEntry> specialAttacks;
            if (assignedSkills.TryGetValue(id, out var value))
            {
                specialAttacks = value.Where(e => e.BasicAttackId != null).ToArray();
            }
            else
            {
                specialAttacks = new BeastSkillEntry[0];
            }

            return ownedEquipment[id]
                .Select(e => equipmentMap.TryGetValue(e.EquipmentId, out var equipment) ? equipment : throw new Exception($"Beast {id} has unknown equipment {e.EquipmentId}"))
                .Select(e
                =>
                new EquipmentTemplate
                {
                    Id = e.Id,
                    Name = e.Name,
                    Category = equipmentCategories[e.CategoryId.Value],
                    IsMartial = e.IsMartial.Value,
                    Quality = e.Quality,
                    NumHands = e.NumHands,
                    Cost = e.Cost,
                    BasicAttack = equipmentCategories[e.CategoryId.Value].IsWeapon ? new BasicAttackTemplate()
                    {
                        Id = e.Id.Value,
                        Name = e.Name,
                        AccuracyMod = e.AttackMod.Value,
                        Attribute1 = e.Attribute1,
                        Attribute2 = e.Attribute2,
                        DamageMod = e.DamageMod.Value,
                        DamageType = damageTypes[e.DamageType.Value].ToDamageType(),
                        IsRanged = equipmentCategories[e.CategoryId.Value].IsRanged,
                        AttackSkills = specialAttacks.Where(s => s.BasicAttackId == e.Id.Value).Select(s => skillMap[s.SkillId]).ToArray(),
                        IsEquipmentAttack = true,
                    } : null,
                    StatsModifier = equipmentCategories[e.CategoryId.Value].IsArmor ?
                        new StatsModifications
                        {
                            InitiativeModifier = -e.InitiativeModification.Value,
                            MagicDefenseModifier = e.MagicDefenceModification.Value,
                            DefenseModifier = e.DefenseOverride ?? e.DefenseModification.Value,
                            DefenseOverrides = e.DefenseOverride.HasValue

                        } : null,
                });
        }

        private IEnumerable<BeastEquipment> GetBeastEquipment()
        {
            using (var connection = _configuration.GetConnection())
            {
                connection.Open();
                var beastEquipment = connection.Query(@"
                    SELECT BeastTemplateId, EquipmentId FROM BeastEquipment
                ");
                return beastEquipment
                    .Select(r =>
                        new BeastEquipment
                        {
                            BeastTemplateId = Guid.Parse(r.BeastTemplateId),
                            EquipmentId = Guid.Parse(r.EquipmentId)
                        })
                    .ToList();
            }
        }

        private IEnumerable<SpellTemplate> ExtractSpells(Guid id, IDictionary<Guid?, SpellEntry> spellMap, IDictionary<Guid, IEnumerable<BeastSpell>> ownedSpells)
        {
            if (!ownedSpells.ContainsKey(id))
            {
                return Enumerable.Empty<SpellTemplate>().ToArray();
            }

            return ownedSpells[id]
               .Select(bs => spellMap[bs.SpellId])
               .Select(s =>
               new SpellTemplate
               {
                   
                   Id = s.Id.Value,
                   Name = s.Name,
                   Attribute1 = s.Attribute1,
                   Attribute2 = s.Attribute2,                   
                   IsOffensive = s.IsOffensive.Value,
                   Duration = s.Duration,
                   Target = s.Target,
                   MagicPointCost = s.MagicPointCost,
                   Description = s.Description
               });
        }

        private IEnumerable<BeastSpell> GetBeastSpells()
        {
            using (var connection = _configuration.GetConnection())
            {
                connection.Open();
                var beastAttacks = connection.Query(@"
                    SELECT BeastTemplateId, SpellId FROM BeastSpell
                ");
                return beastAttacks
                    .Select(r =>
                        new BeastSpell
                        {
                            BeastTemplateId = Guid.Parse(r.BeastTemplateId),
                            SpellId = Guid.Parse(r.SpellId)
                        })
                    .ToList();
            }
        }

        private static IEnumerable<BasicAttackTemplate> ExtractAttacks(            
            Guid id, 
            IDictionary<Guid?, BasicAttackEntry> attackMap, 
            IDictionary<Guid, IEnumerable<BeastAttack>> ownedMap,
            IDictionary<Guid, DamageTypeEntry> damageTypes,
            IDictionary<Guid, SkillTemplate> skillMap,
            IDictionary<Guid, IEnumerable<BeastSkillEntry>> assignedSkills)
        {
            if (!ownedMap.ContainsKey(id))
            {
                return Enumerable.Empty<BasicAttackTemplate>().ToArray();
            }

            ICollection<BeastSkillEntry> specialAttacks;
            if(assignedSkills.TryGetValue(id, out var value))
            {
                specialAttacks = value.Where(e => e.BasicAttackId != null).ToArray();
            }
            else
            {
                specialAttacks = new BeastSkillEntry[0];
            }
           

            return ownedMap[id]
                .Select(ba => attackMap[ba.BasicAttackId])
                .Select(a =>
                new BasicAttackTemplate
                {
                    DamageType = damageTypes[a.DamageType.Value].ToDamageType(),
                    Id = a.Id.Value,
                    Name = a.Name,
                    Attribute1 = a.Attribute1,
                    Attribute2 = a.Attribute2,
                    AccuracyMod = a.AttackMod,
                    DamageMod = a.DamageMod.Value,
                    IsRanged = a.IsRanged.Value,
                    AttackSkills = specialAttacks.Where(s => s.BasicAttackId == a.Id.Value).Select(s => skillMap[s.SkillId]).ToArray()
                });
        }

        private IEnumerable<BeastAttack> GetBeastAttacks()
        {
            using (var connection = _configuration.GetConnection())
            {
                connection.Open();
                var beastAttacks = connection.Query(@"
                    SELECT BeastTemplateId, BasicAttackId FROM BeastAttack
                ");
                return beastAttacks
                    .Select(r => 
                        new BeastAttack 
                        { 
                            BeastTemplateId = Guid.Parse(r.BeastTemplateId), 
                            BasicAttackId = Guid.Parse(r.BasicAttackId) 
                        })
                    .ToList();
            }
        }

        public IEnumerable<Resistance> GetResistances()
        {
            using (var connection = _configuration.GetConnection())
            {
                connection.Open();
                var resistances = connection.Query(@"
                    SELECT BeastTemplateId, DamageTypeId, AffinityId FROM BeastResistance
                ");
                return resistances.Select(r => new Resistance(Guid.Parse(r.DamageTypeId), Guid.Parse(r.AffinityId), Guid.Parse(r.BeastTemplateId))).ToList();
            }
        }

        public virtual ICollection<Dice> GetDice()
        {
            using (var connection = _configuration.GetConnection())
            {
                connection.Open();
                var dice = connection.Query<Dice>(@"
                    SELECT Sides, Name FROM Dice
                ");
                return dice.ToList();
            }
        }

        public ICollection<DamageTypeEntry> GetDamageTypes()
        {
            using (var connection = _configuration.GetConnection())
            {
                connection.Open();
                var damageTypes = connection.Query(@"
                    SELECT Id, Name FROM DamageTypes
                ");
                return damageTypes.Select(d => new DamageTypeEntry { Id = Guid.Parse(d.Id), Name = d.Name }).ToList();
            }
        }

        public ICollection<Species> GetSpecies()
        {
            using (var connection = _configuration.GetConnection())
            {
                connection.Open();
                var species = connection.Query(@"
                    SELECT Id, Name FROM Species
                ");
                return species.Select(s => new Species { Id = Guid.Parse(s.Id), Name = s.Name }).ToList();
            }
        }

        public IEnumerable<Affinity> GetAffinities()
        {
            using (var connection = _configuration.GetConnection())
            {
                connection.Open();
                var species = connection.Query(@"
                    SELECT Id, Name, ShortName FROM Affinity
                ");
                return species.Select(s => new Affinity { Id = Guid.Parse(s.Id), Name = s.Name, ShortName = s.ShortName }).ToList();
            }
        }

        public IEnumerable<BasicAttackEntry> GetBasicAttacks()
        {
            using (var connection = _configuration.GetConnection())
            {
                connection.Open();
                var attacks = connection.Query(@"
                    SELECT Id, Name, Attribute1, Attribute2, IsRanged, DamageType, DamageMod, AttackMod FROM BasicAttack
                ");
                return attacks.Select(a => new BasicAttackEntry 
                    { 
                        Id = Guid.Parse(a.Id), 
                        Name = a.Name, 
                        Attribute1 = a.Attribute1, 
                        Attribute2 = a.Attribute2,
                        IsRanged = a.IsRanged == 0 ? false : true,
                        DamageType = Guid.Parse(a.DamageType),
                        DamageMod = (int?) a.DamageMod,
                        AttackMod = (int) a.AttackMod,
                }).ToList();
            }
        }

        public void CreateBasicAttack(BasicAttackEntry currentAttack)
        {
            using (var connection = _configuration.GetConnection())
            {
                connection.Open();                

                connection.Execute(@"
                    INSERT INTO BasicAttack (Id, Name, Attribute1, Attribute2, IsRanged, DamageType, DamageMod, AttackMod)
                    VALUES (@Id, @Name, @Attribute1, @Attribute2, @IsRanged, @DamageType, @DamageMod, @AttackMod)
                ",
                 new { 
                         Id = currentAttack.Id, 
                         Name = currentAttack.Name, 
                         Attribute1 = currentAttack.Attribute1,
                         Attribute2 = currentAttack.Attribute2,
                         IsRanged =  currentAttack.IsRanged.Value ? 1 : 0,
                         DamageType = currentAttack.DamageType,
                         DamageMod = currentAttack.DamageMod,
                         AttackMod = currentAttack.AttackMod,
                    });
            }
        }

        public void CreateSpell(SpellEntry currentSpell)
        {
            using (var connection = _configuration.GetConnection())
            {
                connection.Open();

                connection.Execute(@"
                    INSERT INTO Spell (Id, Name, Duration, Target, MagicPointCost, Description, Attribute1 , Attribute2, IsOffensive)
                    VALUES (@Id, @Name, @Duration, @Target, @MagicPointCost, @Description, @Attribute1, @Attribute2, @IsOffensive)
                ",
                 new
                 {
                     Id = currentSpell.Id,
                     Name = currentSpell.Name,
                     Duration = currentSpell.Duration,
                     Target = currentSpell.Target,
                     MagicPointCost = currentSpell.MagicPointCost,
                     Description = currentSpell.Description,
                     Attribute1 = currentSpell.Attribute1,
                     Attribute2 = currentSpell.Attribute2,
                     IsOffensive = currentSpell.IsOffensive.Value ? 1 : 0,
                 });
            }
        }

        public IEnumerable<SpellEntry> GetSpells()
        {
            using (var connection = _configuration.GetConnection())
            {
                connection.Open();
                var attacks = connection.Query(@"
                    SELECT Id, Name, Duration, Target, MagicPointCost, Description, Attribute1, Attribute2, IsOffensive FROM Spell
                ");
                return attacks.Select(a => new SpellEntry
                {
                    Id = Guid.Parse(a.Id),
                    Name = a.Name,
                    Duration = a.Duration,
                    Target = a.Target,
                    MagicPointCost = (int) a.MagicPointCost,
                    Description = a.Description,
                    Attribute1 = a.Attribute1,
                    Attribute2 = a.Attribute2,
                    IsOffensive = a.IsOffensive == 0 ? false : true,
                }).ToList();
            }
        }

        public IEnumerable<EquipmentEntry> GetEquipment()
        {
            using (var connection = _configuration.GetConnection())
            {
                connection.Open();
                var equipment = connection.Query(@"
                    SELECT Id, Name, CategoryId, Attribute1, Attribute2, Cost, DamageType, DamageMod, AttackMod, NumHands, 
                        IsMartial, Quality, DefenseModification, DefenseOverride, MagicDefenceModification, InitiativeModification FROM Equipment
                ");
                return equipment.Select(a => new EquipmentEntry
                {
                    Id = Guid.Parse(a.Id),
                    Name = a.Name,
                    CategoryId = Guid.Parse(a.CategoryId),
                    Attribute1 = a.Attribute1,
                    Attribute2 = a.Attribute2,                    
                    IsMartial = a.IsMartial == 0 ? false : true,
                    Cost = (int) a.Cost,
                    DamageMod = (int?)a.DamageMod,
                    AttackMod = (int?)a.AttackMod,
                    DamageType = Guid.TryParse(a.DamageType, out Guid damageType) ? damageType : null,
                    NumHands = (int?)a.NumHands,
                    Quality = a.Quality,
                    DefenseModification = (int?)a.DefenseModification,
                    DefenseOverride = (int?)a.DefenseOverride,
                    MagicDefenceModification = (int?)a.MagicDefenceModification,
                    InitiativeModification = (int?)a.InitiativeModification,

                }).ToList();
            }
        }

        public void CreateEquipment(EquipmentEntry currentEquipment)
        {
            using (var connection = _configuration.GetConnection())
            {
                connection.Open();

                connection.Execute(@"
                    INSERT INTO Equipment (Id, Name, CategoryId, Attribute1 , Attribute2, Cost, DamageMod, AttackMod, DamageType, NumHands, IsMartial, Quality, 
                                DefenseModification, DefenseOverride, MagicDefenceModification, InitiativeModification)
                    VALUES (@Id, @Name, @CategoryId, @Attribute1, @Attribute2, @Cost, @DamageMod, @AttackMod, @DamageType, @NumHands, @IsMartial, @Quality,
                                @DefenseModification, @DefenseOverride, @MagicDefenceModification, @InitiativeModification)
                ",
                 new
                 {
                     Id = currentEquipment.Id,
                     Name = currentEquipment.Name,
                     CategoryId = currentEquipment.CategoryId,
                     Attribute1 = currentEquipment.Attribute1,
                     Attribute2 = currentEquipment.Attribute2,
                     Cost = currentEquipment.Cost,
                     DamageMod = currentEquipment.DamageMod,
                     AttackMod = currentEquipment.AttackMod,
                     DamageType = currentEquipment.DamageType,
                     NumHands = currentEquipment.NumHands,                     
                     IsMartial = ToNullableInt(currentEquipment.IsMartial),
                     Quality = currentEquipment.Quality,
                     DefenseModification = currentEquipment.DefenseModification,
                     DefenseOverride = currentEquipment.DefenseOverride,
                     MagicDefenceModification = currentEquipment.MagicDefenceModification,
                     InitiativeModification = currentEquipment.InitiativeModification,
                 });
            }
        }

        private static int? ToNullableInt(bool? value)
        {
            if (value == null) return null;
            return value.Value ? 1 : 0;
        }

        public ICollection<EquipmentCategory> GetEquipmentCategories()
        {
            using (var connection = _configuration.GetConnection())
            {
                connection.Open();
                var equipmentCategories
                    = connection.Query(@"
                    SELECT Id, Name, IsWeapon, IsArmor, IsRanged FROM EquipmentCategory
                ");
                return equipmentCategories.Select(e => 
                new EquipmentCategory 
                { 
                    Id = Guid.Parse(e.Id), 
                    Name = e.Name, 
                    IsWeapon = e.IsWeapon == 0 ? false : true,
                    IsArmor = e.IsArmor == 0 ? false : true,
                    IsRanged = e.IsRanged == 0 ? false : true,
                }).ToList();
            }
        }

        public virtual int GetNumSkills(SpeciesType species)
        {
            using var connection = _configuration.GetConnection();
            connection.Open();
            var selectSQL = "SELECT NumSkills FROM Species WHERE Id = @Id";
            var results = connection.Query(selectSQL, new { Id = species.Id.ToString() });
            return results.Select(r => (int)r.NumSkills).Single();
        }

        public virtual int GetSkillBonus(Guid damageTypeId)
        {
            using var connection = _configuration.GetConnection();
            connection.Open();
            var selectSQL = "SELECT SkillBonus FROM DamageTypes WHERE Id = @Id";
            var results = connection.Query(selectSQL, new { Id = damageTypeId.ToString() });
            return results.Select(r => (int)r.SkillBonus).Single();
        }

        public virtual IEnumerable<SkillTemplate> GetSkills(IDictionary<string, string>? otherAttributeFilter = null)
        {
            var builder = new SqlBuilder();
            
            foreach(var pair  in otherAttributeFilter ?? new Dictionary<string, string>())
            {
                builder = builder
                    .Where($"json_extract(OtherAttributes, '$.{pair.Key}') LIKE '{pair.Value}'");
            }

            var selectSql = builder.AddTemplate(@"SELECT 
                    Id,
                    Name,
                    TargetType,
                    [Text],
                    IsSpecialRule,
                    Keywords,
                    OtherAttributes
                FROM Skills
                /**where**/
                ");

            using (var connection = _configuration.GetConnection())
            {
                var result = connection.Query<SkillEntry>(selectSql.RawSql);
                return result.Select(e => e.ToSkillTemplate());
            }
        }

        public virtual int GetNumFreeResistances(SpeciesType species)
        {
            using var connection = _configuration.GetConnection();
            connection.Open();
            var selectSQL = "SELECT NumFreeResistances FROM Species WHERE Id = @Id";
            var results = connection.Query(selectSQL, new { Id = species.Id.ToString() });
            return results.Select(r => (int)r.NumFreeResistances).Single();
        }

        public virtual int GetNumFreeImmunities(SpeciesType species)
        {
            using var connection = _configuration.GetConnection();
            connection.Open();
            var selectSQL = "SELECT NumFreeImmunities FROM Species WHERE Id = @Id";
            var results = connection.Query(selectSQL, new { Id = species.Id.ToString() });
            return results.Select(r => (int)r.NumFreeImmunities).Single();
        }

        public virtual SpeciesBuiltInAffinities GetBuiltInVulnerbilityChoices(SpeciesType species)
        {   
            using var connection = _configuration.GetConnection();
            connection.Open();            

            var selectSQL = "SELECT NumFreeVulnerabilities, VulnerabilityChoices FROM Species WHERE Id = @Id";
            var results = connection.Query(selectSQL, new { Id = species.Id.ToString() });
            return results
                .Select(r =>
                {
                    var numChoices = (int)r.NumFreeVulnerabilities;
                    string choiceString = r.VulnerabilityChoices ?? "[]";
                    var choices = JsonConvert.DeserializeObject<Guid[]>(choiceString).Select(i => new Resistance { DamageTypeId = i }).ToArray();
                    return new SpeciesBuiltInAffinities
                    {
                        NumVulnerabilityChoices = numChoices,
                        VulnerabilityChoices = choices
                    };
                })
                .Single();
        }

        public void UpdateKnownSkills(IEnumerable<SkillTemplate> skills)
        {
            using var connection = _configuration.GetConnection();
            connection.Open();

            var deleteSql = @"DELETE FROM Skills WHERE json_extract(OtherAttributes, '$.IsKnownSkill') LIKE 'True'";
            var deletedRows = connection.Execute(deleteSql);

            InsertSkillsInternal(connection, skills);
        }

        public void AddSkills(IEnumerable<SkillTemplate> skills)
        {
            using var connection = _configuration.GetConnection();
            connection.Open();

            InsertSkillsInternal(connection, skills);
        }

        private void InsertSkillsInternal(SqliteConnection connection, IEnumerable<SkillTemplate> skills)
        {
            var insertSql = @"INSERT INTO Skills (Id, Name, TargetType, [Text], IsSpecialRule, Keywords, OtherAttributes)
                                    VALUES (@Id, @Name, @TargetType, @Text, @IsSpecialRule, @Keywords, @OtherAttributes)";

            var skillEntries = skills.Select(s => s.ToSkillEntry());

            var insertedRows = connection.Execute(insertSql, skillEntries);
        }

        public IBeastTemplate? GetBeast(Guid beastId)
        {
            return GetBeasts().SingleOrDefault(b => b.Id == beastId);
        }

        public void AssignSkills(Guid beastTemplateId, IEnumerable<BeastSkillEntry> beastSkillEntries)
        {
            using var connection = _configuration.GetConnection();
            connection.Open();

            var assignSkillSQL =
                @"
                    INSERT INTO BeastSkill (BeastTemplateId, SkillId, BasicAttackId) VALUES (@BeastTemplateId, @SkillId, @BasicAttackId)
                ";

            var insertedRows = connection.Execute(
                                assignSkillSQL, 
                                beastSkillEntries
                                    .Select(e => new 
                                    { 
                                        BeastTemplateId = beastTemplateId.ToString(), 
                                        SkillId = e.SkillId.ToString(), 
                                        BasicAttackId = e.BasicAttackId.ToString() 
                                    }));
        }

        public ICollection<ActionEntry> GetActions()
        {
            using (var connection = _configuration.GetConnection())
            {
                connection.Open();
                var attacks = connection.Query(@"
                    SELECT Id, Name, Effect FROM Action
                ");
                return attacks.Select(a => new ActionEntry
                {
                    Id = Guid.Parse(a.Id),
                    Name = a.Name,                  
                    Effect = a.Effect,
                }).ToList();
            }
        }

        public void CreateAction(ActionEntry currentAction)
        {
            using (var connection = _configuration.GetConnection())
            {
                connection.Open();

                connection.Execute(@"
                    INSERT INTO [Action] (Id, Name, Effect)
                    VALUES (@Id, @Name, @Effect)
                ",
                 new
                 {
                     Id = currentAction.Id,
                     Name = currentAction.Name,
                     Effect = currentAction.Effect,
                 });
            }
        }

        public void UpdateBeast(IBeastTemplate template)
        {
            var beastExisted = GetBeast(template.Id) != null;
            RemoveBeast(template.Id);           

            using (var connection = _configuration.GetConnection())
            {
                connection.Open();
                var beastId = template.Id.ToString();
              
                var beast = template.Model;
                connection.Execute(@"
                    INSERT INTO BeastTemplate (Id, Name, Description, Level, Traits, Species, Dexterity, Insight, Might, Willpower, ImageFile)
                    VALUES (@Id, @Name, @Description, @Level, @Traits, @Species, @Dexterity, @Insight, @Might, @Willpower, @ImageFile)
                ", new
                {
                    Id = beastId,
                    Name = beast.Name,
                    Description = beast.Description,
                    Level = beast.Level,
                    Traits = beast.Traits,
                    Dexterity = beast.Dexterity.Sides,
                    Insight = beast.Insight.Sides,
                    Might = beast.Might.Sides,
                    Willpower = beast.WillPower.Sides,
                    ImageFile = beast.ImageFile,
                    Species = beast.Species.Id.ToString().ToUpperInvariant(),
                });

                foreach(var attack in beast.BasicAttacks)
                {
                    var attackId = attack.Id.ToString().ToUpperInvariant();

                    if(!beastExisted)
                    {
                        //note: updating basic attacks is handled directly
                        connection.Execute(@"
                        INSERT INTO BasicAttack (Id, Name, Attribute1, Attribute2, IsRanged, DamageType, DamageMod, AttackMod)
                        Values (@Id, @Name, @Attribute1, @Attribute2, @IsRanged, @DamageType, 5, 0)
                            ",
                           new
                           {
                               Id = attackId,
                               Name = attack.Name,
                               Attribute1 = attack.Attribute1,
                               Attribute2 = attack.Attribute2,
                               IsRanged = attack.IsRanged ? 1 : 0,
                               DamageType = attack.DamageType.Id,
                           });
                    }
                   

                    connection.Execute(@"
                        INSERT INTO BeastAttack (BeastTemplateId, BasicAttackId)
                        Values (@BeastTemplateId, @BasicAttackId)
                    ",
                   new
                   {
                       BasicAttackId = attackId,
                       BeastTemplateId = beastId,
                   });

                }
                //todo: delete orphaned basic attacks

                foreach (var action in beast.Actions)
                {
                    var actionId = action.Id.ToString();
                    if (!beastExisted)
                    {
                        //note: updating other actions is handled directly
                        connection.Execute(@"
                        INSERT INTO Action (Id, Name, Effect)
                        Values (@Id, @Name, @Effect)
                        ",
                        new
                        {
                            Id = actionId.ToUpperInvariant(),
                            Name = action.Name,
                            Effect = action.Effect,
                        });
                    }

                    connection.Execute(@"
                        INSERT INTO BeastAction (BeastTemplateId, ActionId)
                        Values (@BeastTemplateId, @ActionId)
                    ",
                   new
                   {
                       ActionId = actionId,
                       BeastTemplateId = beastId,
                   });

                }
                //todo: delete orphaned actions

                foreach(var equipment in beast.Equipment)
                {
                    var equipmentId = equipment.Id.ToString().ToUpperInvariant();                   

                    connection.Execute(@"
                        INSERT INTO BeastEquipment (BeastTemplateId, EquipmentId)
                        Values (@BeastTemplateId, @EquipmentId)
                    ",
                   new
                   {
                       EquipmentId = equipmentId,
                       BeastTemplateId = beastId,
                   });
                }

                connection.Execute(@"
                    INSERT INTO BeastResistance (BeastTemplateId, DamageTypeId, AffinityId)
                    VALUES (@BeastTemplateId, @DamageTypeId, @AffinityId)
                ",
                    template.Resistances.Values.Select(r => new { BeastTemplateId = beastId, DamageTypeId = r.DamageTypeId, AffinityId = r.AffinityId })
                );

                connection.Execute(@"
                    INSERT INTO BeastSpell (BeastTemplateId, SpellId)
                    VALUES (@BeastTemplateId, @SpellId)
                ",
               beast.Spells.Select(a => new { BeastTemplateId = beastId, SpellId = a.Id }));
            }
            
            var specialAttackMap = new Dictionary<Guid, ICollection<Guid>>();
            foreach (var attack in template.AllAttacks)
            {
                foreach(var attackSkill in attack.AttackSkills)
                {
                    if (!specialAttackMap.ContainsKey(attackSkill.Id))
                    {
                        specialAttackMap[attackSkill.Id] = new HashSet<Guid>();
                    }
                    specialAttackMap[attackSkill.Id].Add(attack.Id);
                }
            }

            var skillEntries = template.Skills.Where(s => s.OtherAttributes?.IsSpecialAttack != true)
                                .Select(s => new BeastSkillEntry { SkillId = s.Id, BeastTemplateId = template.Id })
                                .Concat(specialAttackMap.SelectMany(p => p.Value.Select(a => new BeastSkillEntry { BasicAttackId = a, SkillId = p.Key, BeastTemplateId = template.Id })));

            AssignSkills(template.Id, skillEntries);
        }

        public void RemoveBeast(Guid id)
        {
            using (var connection = _configuration.GetConnection())
            {
                connection.Open();
                
                connection.Execute(@"
                    DELETE FROM [BeastTemplate]
                    WHERE Id = @Id;
                    
                    DELETE FROM [BeastResistance]
                    WHERE BeastTemplateId = @Id;

                    DELETE FROM [BeastAttack]
                    WHERE BeastTemplateId = @Id;

                    DELETE FROM [BeastAction]
                    WHERE BeastTemplateId = @Id;

                    DELETE FROM [BeastSpell]
                    WHERE BeastTemplateId = @Id;

                    DELETE FROM [BeastEquipment]
                    WHERE BeastTemplateId = @Id;

                    DELETE FROM [BeastSkill]
                    WHERE BeastTemplateId = @Id;
                ",
                new
                {
                    Id = id.ToString(),                 
                });                
            }
        }

        public IEnumerable<EquipmentTemplate> GetEquipmentTemplates()
        {
            var equipmentCategories = GetEquipmentCategories().ToDictionary(ec => ec.Id, ec => ec);
            var damageTypes = GetDamageTypes().ToDictionary(d => d.Id, d => d);
            foreach (var equipment in GetEquipment())
            {
                yield return new EquipmentTemplate
                {
                    Id = equipment.Id,
                    Name = equipment.Name,
                    Category = equipmentCategories[equipment.CategoryId.Value],
                    IsMartial = equipment.IsMartial.Value,
                    Quality = equipment.Quality,
                    NumHands = equipment.NumHands,
                    Cost = equipment.Cost,
                    BasicAttack = equipmentCategories[equipment.CategoryId.Value].IsWeapon ? new BasicAttackTemplate()
                    {
                        Id = equipment.Id.Value,
                        Name = equipment.Name,
                        AccuracyMod = equipment.AttackMod.Value,
                        Attribute1 = equipment.Attribute1,
                        Attribute2 = equipment.Attribute2,
                        DamageMod = equipment.DamageMod.Value,
                        DamageType = damageTypes[equipment.DamageType.Value].ToDamageType(),
                        IsRanged = equipmentCategories[equipment.CategoryId.Value].IsRanged,
                        //AttackSkills = specialAttacks.Where(s => s.BasicAttackId == equipment.Id.Value).Select(s => skillMap[s.SkillId]).ToArray() // no attack skills
                        IsEquipmentAttack = true,
                    } : null,
                    StatsModifier = equipmentCategories[equipment.CategoryId.Value].IsArmor ?
                 new StatsModifications
                 {
                     InitiativeModifier = -equipment.InitiativeModification.Value,
                     MagicDefenseModifier = equipment.MagicDefenceModification.Value,
                     DefenseModifier = equipment.DefenseOverride ?? equipment.DefenseModification.Value,
                     DefenseOverrides = equipment.DefenseOverride.HasValue

                 } : null,
                };
            }            
        }

        public IEnumerable<SpellTemplate> GetSpellTemplates()
        {
            return GetSpells().Select(s => new SpellTemplate
            {
                Id = s.Id.Value,
                Name = s.Name,
                Attribute1 = s.Attribute1,
                Attribute2 = s.Attribute2,
                Description = s.Description,
                Duration = s.Duration,
                IsOffensive = s.IsOffensive ?? false,
                MagicPointCost = s.MagicPointCost,
                Target = s.Target,
            });
        }

        public void UpdateSkill(SkillTemplate skill)
        {
            DeleteSkill(skill);
            AddSkills(new[] { skill });
        }

        /// <summary>
        /// doesn't yet delete associated links
        /// </summary>
        /// <param name="skill"></param>
        private void DeleteSkill(SkillTemplate skill)
        {
            using (var connection = _configuration.GetConnection())
            {
                connection.Open();

                connection.Execute(@"
                    DELETE FROM [Skills]
                    WHERE Id = @Id;
                ",
                new
                {
                    Id = skill.Id.ToString(),
                });
            }
        }

        public void UpdateSpell(SpellTemplate spell)
        {
            DeleteSpell(spell);
            CreateSpell(spell.ToSpellEntry());
        }

        /// <summary>
        /// doesn't yet delete associated links
        /// </summary>
        /// <param name="skill"></param>
        private void DeleteSpell(SpellTemplate spell)
        {
            using (var connection = _configuration.GetConnection())
            {
                connection.Open();

                connection.Execute(@"
                    DELETE FROM [Spell]
                    WHERE Id = @Id;
                ",
                new
                {
                    Id = spell.Id.ToString().ToUpperInvariant(),
                });
            }
        }
    }
}