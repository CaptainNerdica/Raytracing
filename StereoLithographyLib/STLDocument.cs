using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace StereoLithographyLib
{
	public sealed class STLDocument
	{
		public string Header { get; set; }
		public ICollection<Facet> Facets { get; }

		public STLDocument()
		{
			Header = string.Empty;
			Facets = new HashSet<Facet>();
		}
		public STLDocument(string header, ICollection<Facet> facets)
		{
			Header = header;
			Facets = facets;
		}

		public static STLDocument OpenRead(string path)
		{
			using Stream s = File.OpenRead(path);
			return Read(s);
		}
		public static STLDocument Read(Stream stream)
		{
			stream.Seek(0, SeekOrigin.Begin);
			byte[] bytes = new byte[80];
			stream.Read(bytes, 0, 80);
			string header = Encoding.ASCII.GetString(bytes);
			stream.Read(bytes, 0, 4);
			uint facetCount = BitConverter.ToUInt32(bytes, 0);
			ICollection<Facet> facets = new HashSet<Facet>();
			for (int i = 0; i < facetCount; ++i)
			{
				stream.Read(bytes, 0, 50);
				Vector3 normal = VectorFromBytes(bytes, 0);
				Vector3 v1 = VectorFromBytes(bytes, 12);
				Vector3 v2 = VectorFromBytes(bytes, 24);
				Vector3 v3 = VectorFromBytes(bytes, 36);
				ushort attribute = BitConverter.ToUInt16(bytes, 48);
				facets.Add(new Facet(normal, v1, v2, v3, attribute));
			}
			return new STLDocument(header, facets);
		}

		private static Vector3 VectorFromBytes(byte[] bytes, int offset)
		{
			float x, y, z;
			x = BitConverter.ToSingle(bytes, offset + 0 * sizeof(float));
			y = BitConverter.ToSingle(bytes, offset + 1 * sizeof(float));
			z = BitConverter.ToSingle(bytes, offset + 2 * sizeof(float));
			return new Vector3(x, y, z);
		}
	}
}
