using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FleetTracker.Models.Events
{
	[Table("assegnazioni_veicolo")]
	public class AssegnazioneVeicolo
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int av_id { get; set; }

		public int v_id { get; set; }
		public int dipendente_id { get; set; }
		public DateTime data_inizio { get; set; }
		public DateTime? data_fine { get; set; }

		[ForeignKey(nameof(v_id))]
		public Veicolo? Veicolo { get; set; }
		[ForeignKey(nameof(dipendente_id))]
		public Utenti? Dipendente { get; set; }
	}
}
