using FabulaUltimaSkillLibrary;
using FirstProject;
using Godot;

namespace FabulaUltimaGMTool
{
    public partial class SkillResolver : Node
    {
        public Resolver Instance
        {
            get
            {
                var db = GetNode<DbAccess>("/root/DbAccess").Repository.Database;
                var specAtkIndex = new SpecialAttackIndex(db);
                return new Resolver(db, specAtkIndex);
            }
        }
    }
}
