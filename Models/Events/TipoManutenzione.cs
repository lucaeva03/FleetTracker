using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FleetTracker.Models.Events
{
	[Table("tipo_manutenzione")]
	public class TipoManutenzione
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int mt_id { get; set; }
		public string nome { get; set; }
		public string? descrizione { get; set; }

		public ICollection<Manutenzione>? Manutenzioni { get; set; }
	}
}