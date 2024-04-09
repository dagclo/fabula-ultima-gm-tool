using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary;
using FabulaUltimaSkillLibrary.Models;
using NSubstitute;

namespace FabulaUltimaSkillLibraryTests
{
    internal class SkilledBeastTemplateTests
    {
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void ImprovedHPApplied(int numTimesApplied)
        {
            // Arrange
            const int mockHP = 10;
            var templateMock = Substitute.For<IBeastTemplate>();
            templateMock.HealthPoints.Returns(mockHP);
            var skill = KnownSkills.ImprovedHitPoints;
            var hpBonus = int.Parse(skill.OtherAttributes[StatsConstants.HP_BOOST]) * numTimesApplied;
            templateMock.Skills.Returns(Enumerable.Range(0, numTimesApplied).Select(_ => skill).ToArray());

            var skilledBeast = new SkilledBeastTemplateWrapper(templateMock);

            // Act
            var hp = skilledBeast.HealthPoints;

            // Assert
            Assert.That(hp, Is.EqualTo(mockHP + hpBonus));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void SpellCasterMoreMPApplied(int numTimesApplied)
        {
            // Arrange
            const int mockMp = 10;
            var templateMock = Substitute.For<IBeastTemplate>();
            templateMock.MagicPoints.Returns(mockMp);
            var skill = KnownSkills.SpellCasterMoreMP;
            var mpBoost = int.Parse(skill.OtherAttributes[StatsConstants.MP_BOOST]) * numTimesApplied;
            templateMock.Skills.Returns(Enumerable.Range(0, numTimesApplied).Select(_ => skill).ToArray());

            var skilledBeast = new SkilledBeastTemplateWrapper(templateMock);

            // Act
            var mp = skilledBeast.MagicPoints;

            // Assert
            Assert.That(mp, Is.EqualTo(mockMp + mpBoost));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void SpellCasterMoreSpellsApplied(int numTimesApplied)
        {
            // Arrange
            const int mockMp = 10;
            var templateMock = Substitute.For<IBeastTemplate>();
            templateMock.MagicPoints.Returns(mockMp);
            var skill = KnownSkills.SpellCasterMoreSpells;
            var mpBoost = int.Parse(skill.OtherAttributes[StatsConstants.MP_BOOST]) * numTimesApplied;
            templateMock.Skills.Returns(Enumerable.Range(0, numTimesApplied).Select(_ => skill).ToArray());

            var skilledBeast = new SkilledBeastTemplateWrapper(templateMock);

            // Act
            var mp = skilledBeast.MagicPoints;

            // Assert
            Assert.That(mp, Is.EqualTo(mockMp + mpBoost));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void ImprovedInitiativeApplied(int numTimesApplied)
        {
            // Arrange
            const int mockInit = 10;
            var templateMock = Substitute.For<IBeastTemplate>();
            templateMock.Initiative.Returns(mockInit);
            var skill = KnownSkills.ImprovedInitiative;
            var initiativeBoost = int.Parse(skill.OtherAttributes[StatsConstants.INIT_BOOST]) * numTimesApplied;
            templateMock.Skills.Returns(Enumerable.Range(0, numTimesApplied).Select(_ => skill).ToArray());

            var skilledBeast = new SkilledBeastTemplateWrapper(templateMock);

            // Act
            var init = skilledBeast.Initiative;

            // Assert
            Assert.That(init, Is.EqualTo(mockInit + initiativeBoost));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void ImprovedPDefApplied(int numTimesApplied)
        {
            // Arrange
            const int mockDefense = 10;
            const int mockMDefense = 10;
            var templateMock = Substitute.For<IBeastTemplate>();
            templateMock.Defense.Returns(mockDefense);
            templateMock.MagicalDefense.Returns(mockMDefense);
            var skill = KnownSkills.ImprovedDefensesPhysical;
            var defBonus = int.Parse(skill.OtherAttributes[StatsConstants.DEF_BOOST]) * numTimesApplied;
            var mDefBonus = int.Parse(skill.OtherAttributes[StatsConstants.MDEF_BOOST]) * numTimesApplied;
            templateMock.Skills.Returns(Enumerable.Range(0, numTimesApplied).Select(_ => skill).ToArray());

            var skilledBeast = new SkilledBeastTemplateWrapper(templateMock);

            // Act
            var def = skilledBeast.Defense;
            var mDef = skilledBeast.MagicalDefense;

            // Assert
            Assert.That(def, Is.EqualTo(mockDefense + defBonus));
            Assert.That(mDef, Is.EqualTo(mockMDefense + mDefBonus));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void ImprovedMDefApplied(int numTimesApplied)
        {
            // Arrange
            const int mockDefense = 10;
            const int mockMDefense = 10;
            var templateMock = Substitute.For<IBeastTemplate>();
            templateMock.Defense.Returns(mockDefense);
            templateMock.MagicalDefense.Returns(mockMDefense);
            var skill = KnownSkills.ImprovedDefensesMagical;
            var defBonus = int.Parse(skill.OtherAttributes[StatsConstants.DEF_BOOST]) * numTimesApplied;
            var mDefBonus = int.Parse(skill.OtherAttributes[StatsConstants.MDEF_BOOST]) * numTimesApplied;
            templateMock.Skills.Returns(Enumerable.Range(0, numTimesApplied).Select(_ => skill).ToArray());

            var skilledBeast = new SkilledBeastTemplateWrapper(templateMock);

            // Act
            var def = skilledBeast.Defense;
            var mDef = skilledBeast.MagicalDefense;

            // Assert
            Assert.That(def, Is.EqualTo(mockDefense + defBonus));
            Assert.That(mDef, Is.EqualTo(mockMDefense + mDefBonus));
        }



        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)] // checking even though this shouldn't be applied this many time
        public void SpecializedAccuracyCheckApplied(int numTimesApplied)
        {
            // Arrange

            var templateMock = Substitute.For<IBeastTemplate>();
            var attack = new BasicAttackTemplate
            {
                AttackMod = 0
            };
            templateMock.AllAttacks.Returns(
                new[]
                {
                    attack
                });

            templateMock.BasicAttacks.Returns(
                new[]
                {
                    attack
                });

            var skill = KnownSkills.SpecializedAccuracyCheck;
            var accMod = int.Parse(skill.OtherAttributes[CheckConstants.ACC_CHECK]) * numTimesApplied;
            templateMock.Skills.Returns(Enumerable.Range(0, numTimesApplied).Select(_ => skill).ToArray());

            var skilledBeast = new SkilledBeastTemplateWrapper(templateMock);

            // Act
            var improvedAttackFromAll = skilledBeast.AllAttacks.Single();
            var improvedAttackFromBasic = skilledBeast.BasicAttacks.Single();

            // Assert
            Assert.That(improvedAttackFromAll.AttackMod, Is.EqualTo(attack.AttackMod + accMod));
            Assert.That(improvedAttackFromBasic.AttackMod, Is.EqualTo(attack.AttackMod + accMod));
        }


        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)] // checking even though this shouldn't be applied this many time
        public void ImprovedDamageAttackApplied(int numTimesApplied)
        {
            // Arrange

            var templateMock = Substitute.For<IBeastTemplate>();
            var skill = KnownSkills.ImprovedDamageAttack;
            var attack = new BasicAttackTemplate
            {
                DamageMod = 0,
                AttackSkills = Enumerable.Range(0, numTimesApplied).Select(_ => skill).ToArray(),
            };
            templateMock.AllAttacks.Returns(
                new[]
                {
                    attack
                });

            templateMock.BasicAttacks.Returns(
                new[]
                {
                    attack
                });


            var damageBoost = int.Parse(skill.OtherAttributes[DamageConstants.DAMAGE_BOOST]) * numTimesApplied;


            var skilledBeast = new SkilledBeastTemplateWrapper(templateMock);

            // Act
            var improvedAttackFromAll = skilledBeast.AllAttacks.Single();
            var improvedAttackFromBasic = skilledBeast.BasicAttacks.Single();

            // Assert
            Assert.That(improvedAttackFromAll.DamageMod, Is.EqualTo(attack.DamageMod + damageBoost));
            Assert.That(improvedAttackFromBasic.DamageMod, Is.EqualTo(attack.DamageMod + damageBoost));
        }

        [Test]
        public void OtherSpecialAttacksAppliedNoBonus()
        {
            // Arrange

            var templateMock = Substitute.For<IBeastTemplate>();
            var skill = KnownSkills.SpecialAttackAltDamageIce;
            var attack = new BasicAttackTemplate
            {
                DamageMod = 0,
                AttackSkills = new [] {skill},
            };
            templateMock.AllAttacks.Returns(
                new[]
                {
                    attack
                });

            templateMock.BasicAttacks.Returns(
                new[]
                {
                    attack
                });

            var skilledBeast = new SkilledBeastTemplateWrapper(templateMock);

            // Act
            var improvedAttackFromAll = skilledBeast.AllAttacks.Single();
            var improvedAttackFromBasic = skilledBeast.BasicAttacks.Single();

            // Assert
            Assert.That(improvedAttackFromAll.DamageMod, Is.EqualTo(attack.DamageMod));
            Assert.That(improvedAttackFromBasic.DamageMod, Is.EqualTo(attack.DamageMod));
        }

        [Test]
        public void LevelDamageModifierApplied()
        {
            // Arrange
            const int damageModifer = 392;
            var templateMock = Substitute.For<IBeastTemplate>();
            
            var attack = new BasicAttackTemplate
            {
                DamageMod = 0,                
            };
            templateMock.AllAttacks.Returns(
                new[]
                {
                    attack
                });

            templateMock.BasicAttacks.Returns(
                new[]
                {
                    attack
                });

            templateMock.LevelDamageModifier.Returns(damageModifer);

            var skilledBeast = new SkilledBeastTemplateWrapper(templateMock);

            // Act
            var improvedAttackFromAll = skilledBeast.AllAttacks.Single();
            var improvedAttackFromBasic = skilledBeast.BasicAttacks.Single();

            // Assert
            Assert.That(improvedAttackFromAll.DamageMod, Is.EqualTo(attack.DamageMod + damageModifer));
            Assert.That(improvedAttackFromBasic.DamageMod, Is.EqualTo(attack.DamageMod + damageModifer));
        }

        [Test]
        public void LevelAccuracyModifierApplied()
        {
            // Arrange
            const int accuracyModifer = 392;
            var templateMock = Substitute.For<IBeastTemplate>();
            
            var attack = new BasicAttackTemplate
            {
                DamageMod = 0,
            };
            templateMock.AllAttacks.Returns(
                new[]
                {
                    attack
                });

            templateMock.BasicAttacks.Returns(
                new[]
                {
                    attack
                });

            templateMock.LevelAccuracyModifier.Returns(accuracyModifer);

            var skilledBeast = new SkilledBeastTemplateWrapper(templateMock);

            // Act
            var improvedAttackFromAll = skilledBeast.AllAttacks.Single();
            var improvedAttackFromBasic = skilledBeast.BasicAttacks.Single();

            // Assert
            Assert.That(improvedAttackFromAll.AttackMod, Is.EqualTo(attack.AttackMod + accuracyModifer));
            Assert.That(improvedAttackFromBasic.AttackMod, Is.EqualTo(attack.AttackMod + accuracyModifer));
        }
    }
}
