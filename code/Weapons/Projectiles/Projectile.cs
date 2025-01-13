public class Projectile : Component
{
	[Property] public GameObject ExplosionPrefab { get; set; }
	[Sync] public GameObject Owner { get; set; }
}
