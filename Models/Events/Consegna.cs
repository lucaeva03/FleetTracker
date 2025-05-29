using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;   // non strettamente necessario, ma ok

namespace FleetTracker.Models.Events
{
	[Table("consegne")]
	public class Consegna
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int c_id { get; set; }

		public int cliente_id { get; set; }
		public int dipendente_id { get; set; }

		public DateTime data_prevista { get; set; }
		public TimeSpan? orario_previsto { get; set; }
		public string indirizzo_dest { get; set; } = null!;
		public string stato { get; set; } = null!;
		public DateTime data_creazione { get; set; }

		[ForeignKey(nameof(cliente_id))]
		[InverseProperty(nameof(Utenti.ConsegneCliente))]
		public Utenti? Cliente { get; set; }

		[ForeignKey(nameof(dipendente_id))]
		[InverseProperty(nameof(Utenti.ConsegneDipendente))]
		public Utenti? Dipendente { get; set; }

	}
}
