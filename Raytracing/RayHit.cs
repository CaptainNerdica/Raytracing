namespace Raytracing
{
	public readonly struct RayHit
	{
		public Point Point { get; }
		public Vector3 Normal { get; }
		public float Length { get; }
		public Material Material { get; }

		public RayHit(Point point, Vector3 normal, float length, Material material)
		{
			Point = point;
			Normal = normal;
			Length = length;
			Material = material;
		}
	}
}