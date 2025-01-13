using System.Threading;

public sealed class Grenade : Item
{
	[Property] public GameObject DroppedGrenadePrefab { get; set; }
	[Property] public string PullAnimName { get; set; } = "Throwable_pull";
	[Property] public string ThrowAnimName { get; set; } = "Throwable_pull";

	protected override void OnDisabled()
	{
		base.OnDisabled();
	}

	protected override void OnStart()
	{
		base.OnStart();
	}

	protected override void OnUpdate()
	{
		if ( IsProxy )
			return;
		if ( Input.Pressed( "attack1" ) )
		{
			Log.Info("Grenade was pulled");
			PullGrenade();
		}
		if (Input.Down( "attack1" ) )
		{
			//DropGrenade();
		}
	}

	protected override void OnPreRender()
	{
		
	}
	public void PullGrenade()
	{
		if ( IsProxy )
			return;
		GameObject.Dispatch( new WeaponAnimEvent( ThrowAnimName, true ) );
	}

	public void DropGrenade()
	{
		if ( IsProxy )
			return;

		GameObject.Dispatch( new WeaponAnimEvent( ThrowAnimName, true ) );

		var local = FWPlayerController.Local;

		if ( !local.IsValid() || (!local?.Inventory.IsValid() ?? true) )
			return;

		if ( local.Inventory.CurrentItem == GameObject )
			Log.Info( "Flag disabled" );

		local.SetHasFlag( false );

		//We need this, but I don't think we should need it
		var clone = DroppedGrenadePrefab.Clone( local.WorldPosition );

		if ( clone.Components.TryGet<DroppedGrenade>( out var droppedGrenade ) )
		{
			//droppedGrenade.Owner = local;
		}
		clone.NetworkSpawn( null );

		local.Inventory.RemoveItem( GameObject, true );

		FWPlayerController.ClearHoldRenderer( local.HoldRenderer );
	}
}


public sealed class DroppedGrenade : Component
{
	[Property] public int Damage {  get; set; }
	[Property] public int DetonateTime { get; set; }
	[Property] public float PunchForce {  get; set; }
	[Property] public GameObject ExplosionEffect { get; set; }

	[Sync] private float timer { get; set; } = 0;

	protected override void OnFixedUpdate() {
		if ( IsProxy )
			return;
		if ( timer >= DetonateTime )
		{
			var clone = ExplosionEffect.Clone( WorldPosition );
			clone.NetworkSpawn( null );
		}
	}
}
