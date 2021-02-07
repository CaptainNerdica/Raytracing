using System.Numerics;

namespace StereoLithographyLib
{
	public class Facet
	{
		public Vector3 Normal { get; set; }
		public Vector3 Vertex1 { get; set; }
		public Vector3 Vertex2 { get; set; }
		public Vector3 Vertex3 { get; set; }
		public ushort AttributeByteCount { get; }

		public Facet(Vector3 normal, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, ushort attributeByteCount)
		{
			Normal = normal;
			Vertex1 = vertex1;
			Vertex2 = vertex2;
			Vertex3 = vertex3;
			AttributeByteCount = attributeByteCount;
		}
	}
}