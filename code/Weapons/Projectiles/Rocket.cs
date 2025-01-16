using Sandbox;

public sealed class Rocket : Projectile, Component.ICollisionListener
{

	[Property] public float Speed { get; set; }
	[Sync, Property] public Rigidbody Rigidbody { get; set; }
	

	protected override void OnAwake()
	{
		if ( IsProxy )
			return;
		Rigidbody.ApplyForce(WorldTransform.Forward * Speed);
	}
	protected override void OnFixedUpdate()
	{
		if ( IsProxy )
			return;
		Rigidbody.Velocity = WorldTransform.Forward * Speed;
	}
	public void OnCollisionStart( Collision collision )
	{
		if ( IsProxy )
		{
			return;
		}
		Log.Info( collision.Other.GameObject.ToString() );
		
		if ( collision.Other.GameObject != null && collision.Other.GameObject == Owner.Collider )
		{
			return;
		}
		//Log.Info( $"{collider.GameObject}" );
		Speed = 0;
		var explo = ExplosionPrefab.Clone(WorldPosition);
		var exploComponent = explo.GetComponent<Explosion>();
		exploComponent.Owner = Owner.GameObject;
		explo.NetworkSpawn();
		GameObject.Destroy();
	} 
}
