using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;

namespace DSS19
{
	/// <summary>
	/// SQLite with EF6, copied from the Codeproject article 
	/// <see href="https://www.codeproject.com/Articles/1158937/SQLite-with-Csharp-Net-and-Entity-Framework">
	/// SQLite with C#.Net and Entity Framework</see>.
	/// </summary>
	public class SQLiteDatabaseContext : DbContext
	{
		public SQLiteDatabaseContext(string databaseFile) :
			base(new SQLiteConnection
				{
					ConnectionString = new SQLiteConnectionStringBuilder
						{
							DataSource = databaseFile,
							ForeignKeys = true
						}
						.ConnectionString
				},
				true)
		{
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
			base.OnModelCreating(modelBuilder);
		}

      public DbSet<Ordine> Ordini { get; set; }
   }
}