using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FleetTracker.Models.Events
{
	public class Utenti
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }
		public string username { get; set; }
		public string password { get; set; }
		public string ruolo { get; set; }
		public string? email { get; set; }

		public ICollection<AssegnazioneVeicolo>? Assegnazioni { get; set; }
		public ICollection<ConsumoGiornaliero>? Consumi { get; set; }
		
		[InverseProperty(nameof(Consegna.Cliente))]
		public ICollection<Consegna>? ConsegneCliente { get; set; }

		[InverseProperty(nameof(Consegna.Dipendente))]
		public ICollection<Consegna>? ConsegneDipendente { get; set; }
	}
}