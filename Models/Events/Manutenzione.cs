using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FleetTracker.Models.Events
{
	[Table("manutenzioni")]
	public class Manutenzione
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int m_id { get; set; }
		public int v_id { get; set; }
		public int mt_id { get; set; }
		public DateTime data_intervento { get; set; }
		public decimal costo { get; set; }
		
		[Required]
		public bool is_completed { get; set; }


		[ForeignKey(nameof(v_id))]
		public Veicolo? Veicolo { get; set; }
		
		[ForeignKey(nameof(mt_id))]
		public TipoManutenzione? TipoManutenzione { get; set; }

	}
}