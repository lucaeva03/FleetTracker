using System;
using System.Linq;
using FleetTracker.Data;
using FleetTracker.Models.Events;
using Microsoft.EntityFrameworkCore;    

namespace FleetTracker.Services
{
	public static class VehicleStateUpdater
	{
		public static void UpdateAllStates(FleetTrackerDbContext db)
		{
			var today = DateTime.Today;

			var vehicles = db.Veicoli
				.Include(v => v.Manutenzioni)    
				.Include(v => v.Assegnazioni) 
				.ToList();

			foreach (var v in vehicles)
			{
				bool hasMaintToday = v.Manutenzioni.Any(m =>
					m.data_intervento.Date == today && !m.is_completed);

				bool hasAssignToday = v.Assegnazioni.Any(a =>
					a.data_inizio.Date <= today &&
					(a.data_fine == null || a.data_fine.Value.Date >= today));

				if (hasAssignToday)
					v.stato_corrente = "fuori deposito";
				else if (hasMaintToday)
					v.stato_corrente = "manutenzione";
				// altrimenti non mofico v.stato_corrente
			}

			db.SaveChanges();
		}
	}
}
