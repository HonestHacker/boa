using Sandbox;

public sealed class Explosion : Component
{
	[Property] public int MaxDamage { get; set; }
	[Property] public int MinDamage { get; set; }
	[Property] public float Radius { get; set; }
	[Property] public float PunchForce { get; set; }
	[Sync] public GameObject Owner { get; set; }

	protected override void OnAwake()
	{
		if ( IsProxy )
			return;
		var tr = Scene.Trace
			.Sphere( Radius, WorldPosition, WorldPosition )
			.WithTag( "player" )
			.RunAll();
		foreach ( var t in tr)
		{
			var go = t.GameObject;
			if ( go.Components.TryGet<FWPlayerController>( out var playerController, FindMode.EverythingInSelfAndParent ) )
			{
				var damage = (int)Math.Round( ((MinDamage - MaxDamage) / Radius * WorldPosition.Distance(go.WorldPosition) + MaxDamage) * (Owner == go ? 0.2f : 1) );
				playerController.HealthComponent.TakeDamage(Owner, damage);
				var punch = (go.WorldPosition - WorldPosition).Normal * PunchForce;
				if ( playerController.IsCrouching )
					punch *= 1.5f;
				Log.Info( $"Rocket damage: {damage}; distance: {WorldPosition.Distance( go.WorldPosition )}; Punch {punch}" );
				playerController.shrimpleCharacterController.Punch(punch);
			}
		}
	}
}
