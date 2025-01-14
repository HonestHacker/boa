public class Projectile : Component
{
	[Property] public GameObject ExplosionPrefab { get; set; }
	[Sync] public FWPlayerController Owner { get; set; }
}
