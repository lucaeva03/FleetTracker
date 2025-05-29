using FleetTracker.Models.Events;
using Microsoft.EntityFrameworkCore;

namespace FleetTracker.Data
{
	public class FleetTrackerDbContext : DbContext
	{
		public FleetTrackerDbContext(DbContextOptions<FleetTrackerDbContext> options) : base(options) { }

		public DbSet<Utenti> utenti { get; set; }

		public DbSet<Veicolo> Veicoli { get; set; }
		public DbSet<TipoManutenzione> TipiManutenzione { get; set; }
		public DbSet<Manutenzione> Manutenzioni { get; set; }
		
		public DbSet<AssegnazioneVeicolo> AssegnazioniVeicolo { get; set; }
		public DbSet<ConsumoGiornaliero> ConsumiGiornalieri { get; set; }
		public DbSet<Consegna> Consegne { get; set; }
		

	}
}
