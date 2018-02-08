using System.Data.Entity.ModelConfiguration;

namespace Dominio
{
    internal class UsersMap : EntityTypeConfiguration<User>
    {
        public UsersMap()
        {
            HasRequired(u => u.FavoriteTeam).WithMany(f => f.Fans).HasForeignKey(h => h.FavoriteTeamId);
        }
    }
}