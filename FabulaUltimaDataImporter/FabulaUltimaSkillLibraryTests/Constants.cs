using FabulaUltimaNpc;

namespace FabulaUltimaSkillLibraryTests
{
    internal static class Constants
    {

        public const string DEXTERITY = "DEXTERITY";
        public const string MIGHT = "MIGHT";
        public const string INSIGHT = "INSIGHT";
        public const string WILLPOWER = "WILLPOWER";

        public readonly static Die D6 = new Die(6);
        public readonly static Die D8 = new Die(8);
        public readonly static Die D10 = new Die(10);
        public readonly static Die D12 = new Die(12);
        public readonly static Die D20 = new Die(20);

        public readonly static DamageType POISON   = new DamageType { Name = "poison",   Id = Guid.Parse("f36c11bf-a896-4cc7-9460-37bf5100e14a") };
        public readonly static DamageType PHYSICAL = new DamageType { Name = "physical", Id = Guid.Parse("ffad483e-0ad5-4a43-b235-080ddfd67470") };
        public readonly static DamageType AIR      = new DamageType { Name = "air",      Id = Guid.Parse("7dfe93bd-67d5-468d-bb32-b4d8c1676305") };
        public readonly static DamageType BOLT     = new DamageType { Name = "bolt",     Id = Guid.Parse("39fb0c13-06df-47e0-ae4b-ccfb2012b03d") };
        public readonly static DamageType DARK     = new DamageType { Name = "dark",     Id = Guid.Parse("c635cee1-fcbc-44cd-98c9-fe55c7084806") };
        public readonly static DamageType EARTH    = new DamageType { Name = "earth",    Id = Guid.Parse("813f85c6-fa28-42f2-ad4b-b682a4814382") };
        public readonly static DamageType FIRE     = new DamageType { Name = "fire",     Id = Guid.Parse("dda401cf-437c-438e-9a1b-e3421f9c4902") };
        public readonly static DamageType ICE      = new DamageType { Name = "ice",      Id = Guid.Parse("01a0f627-748c-49eb-999f-03746b673be5") };
        public readonly static DamageType LIGHT    = new DamageType { Name = "light",    Id = Guid.Parse("9ef9cb1e-96da-4acc-ae0e-66e8e5236888") };
        public readonly static DamageType NO_DAMAGE = new DamageType { Name = "no damage", Id = Guid.Parse("3da973fa-c8fa-4eaa-9fdc-8cda9fc0c5af") };

        public readonly static SpeciesType BEAST = new SpeciesType(Guid.Parse("b0788720-8fa0-4968-ac61-5f3063d97c17"), "Beast");
        public readonly static SpeciesType CONSTRUCT = new SpeciesType(Guid.Parse("f50815fc-9d41-4eeb-9797-182544244f0a"), "Construct");
        public readonly static SpeciesType DEMON = new SpeciesType(Guid.Parse("37e76b06-fd97-4c73-8509-eb42e3610eef"), "Demon");
        public readonly static SpeciesType ELEMENTAL = new SpeciesType(Guid.Parse("19014999-30a7-4635-b1a1-505b10a5bc19"), "Elemental");
        public readonly static SpeciesType HUMANOID = new SpeciesType(Guid.Parse("69711547-14c6-4a01-af94-f5d5117a6bae"), "Humanoid");
        public readonly static SpeciesType MONSTER = new SpeciesType(Guid.Parse("23e74a9c-8413-497f-b098-f541b43884c0"), "Monster");
        public readonly static SpeciesType PLANT = new SpeciesType(Guid.Parse("d608585c-32ff-4d10-88b9-b4df66364195"), "Plant");
        public readonly static SpeciesType UNDEAD = new SpeciesType(Guid.Parse("3e35bbec-d713-4efc-af8a-3d5e01403885"), "Plant");
    }
}
