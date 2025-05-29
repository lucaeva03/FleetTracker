using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FleetTracker.Models.Events
{
	[Table("consumi_giornalieri")]
	public class ConsumoGiornaliero
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int cg_id { get; set; }
		public int v_id { get; set; }
		public int dipendente_id { get; set; }
		public DateTime data_log { get; set; }
		public decimal litri_carburante { get; set; }
		public int km_percorsi { get; set; }

		[ForeignKey(nameof(v_id))]
		public Veicolo? Veicolo { get; set; }
		[ForeignKey(nameof(dipendente_id))]
		public Utenti? Dipendente { get; set; }
	}
}