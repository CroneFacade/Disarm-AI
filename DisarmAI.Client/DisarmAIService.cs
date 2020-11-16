using System;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using JetBrains.Annotations;
using NFive.SDK.Client.Commands;
using NFive.SDK.Client.Communications;
using NFive.SDK.Client.Events;
using NFive.SDK.Client.Interface;
using NFive.SDK.Client.Services;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Core.Models.Player;

namespace CroneFacade.DisarmAI.Client
{
	[PublicAPI]
	public class DisarmAIService : Service
	{
		public DisarmAIService(ILogger logger, ITickManager ticks, ICommunicationManager comms, ICommandManager commands, IOverlayManager overlay, User user) : base(logger, ticks, comms, commands, overlay, user) { }

		public override async Task Started()
		{
			// Attach a tick handler
			this.Ticks.On(OnTick);
			API.DisablePlayerVehicleRewards(API.PlayerId());
		}

		private async Task OnTick()
		{
			var peds = World.GetAllPeds().Where(x=> x.IsHuman && !x.IsPlayer);

			foreach (var ped in peds)
			{
				API.RemoveAllPedWeapons(ped.Handle, true);
				API.SetPedDropsWeaponsWhenDead(ped.Handle, false);
			}

			await Delay(TimeSpan.FromSeconds(3));
		}
	}
}
