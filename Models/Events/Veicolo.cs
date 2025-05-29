using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FleetTracker.Models.Events
{
	[Table("veicoli")]
	public class Veicolo
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int v_id { get; set; }
		public string targa { get; set; }
		public string modello { get; set; }
		public short anno_immatricolazione { get; set; }
		public int km_totali { get; set; }
		public string stato_corrente { get; set; }
		public DateTime data_acquisto { get; set; }

		public ICollection<Manutenzione>? Manutenzioni { get; set; }
		public ICollection<AssegnazioneVeicolo>? Assegnazioni { get; set; }
		public ICollection<ConsumoGiornaliero>? Consumi { get; set; }
	}
}
