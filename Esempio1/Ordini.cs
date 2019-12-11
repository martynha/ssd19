using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSS19
{
	[Table("ordini")]
	public class Ordine
	{
		//[Key, Column("PK_UID", TypeName = "INTEGER")]
		//public long pk_uid { get; set; }
      [Column("id", TypeName = "INTEGER")]
      public long id { get; set; }
      [Column("customer", TypeName = "VARCHAR")]
      public string customer { get; set; }
      [Column("time", TypeName = "INTEGER")]
      public int time { get; set; }
      [Column("quant", TypeName = "INTEGER")]
      public int quant { get; set; }
   }
}