using System;
using System.Buffers;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Raytracing
{
	public static class Engine
	{
		private const int _parallelism = 4;

		internal class ProgressRef
		{
			public long Iteration;
			public int NextPercentage;
			public int Delta;
			public long Total;
		}
		public static Image Render(Scene scene, int blendAmount, int samples, int width, int height)
		{
			Color[] finalColors = new Color[width * height];
			long totalRays = 0;
			ProgressRef progress = new ProgressRef { Iteration = 0, NextPercentage = 0, Delta = 5, Total = (long)width * height * samples };
			Parallel.For(0, samples, new ParallelOptions { MaxDegreeOfParallelism = _parallelism }, s => TraceRays(finalColors, scene, width, height, blendAmount, samples, ref totalRays, progress));
			Console.WriteLine("100%");
			Console.WriteLine($"Traced {totalRays:N0} Rays");

			return CopyColorsToImage(finalColors, width, height);
		}
		private static unsafe Image CopyColorsToImage(Color[] colors, int width, int height)
		{
			Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppRgb);
			Rectangle rect = new Rectangle(0, 0, width, height);
			BitmapData data = bitmap.LockBits(rect, ImageLockMode.WriteOnly, bitmap.PixelFormat);
			IntPtr ptr = data.Scan0;
			Span<byte> bytes = new Span<byte>((void*)ptr, Math.Abs(data.Stride) * data.Height);
			for (int i = 0; i < width * height; i++)
				WriteColor32BppRgb(bytes, i, Color.Clamp(Color.Gamma(colors[i], 2.2f), Color.Black, Color.White));
			bitmap.UnlockBits(data);
			return bitmap;
		}
		private static void TraceRays(Color[] colors, Scene scene, int width, int height, int blendAmount, int samples, ref long totalRays, ProgressRef progress)
		{
			long rays = 0;
			Parallel.For(0, height, new ParallelOptions { MaxDegreeOfParallelism = _parallelism }, j =>
			{
				if (100L * progress.Iteration / progress.Total >= progress.NextPercentage)
				{
					progress.NextPercentage += progress.Delta;
					Console.WriteLine($"{100L * progress.Iteration / progress.Total}%");
				}
				Parallel.For(0, width, new ParallelOptions { MaxDegreeOfParallelism = _parallelism }, i =>
				{
					Span<Color> blendBuffer = stackalloc Color[blendAmount * blendAmount];
					int w = 2 * blendAmount + 1;
					for (int k = 0; k < blendBuffer.Length; k++)
					{
						float offsetX = (float)ThreadSafeRandom.NextDouble() - 0.5f;
						float offsetY = (float)ThreadSafeRandom.NextDouble() - 0.5f;
						float u = i + (float)(1 + 2 * (k % blendAmount) + offsetX) / w;
						float v = j + (float)(1 + 2 * (k / blendAmount) + offsetY) / w;
						blendBuffer[k] = scene.TraceRay(scene.Camera.GetRay(u, v, width, height), scene.Camera.NearClipping, scene.Camera.MaxDistance, ref rays);
					}
					colors[i + j * width] += Color.Blend(blendBuffer) / samples;
					++progress.Iteration;
				});
			});
			totalRays += rays;
		}

		private static void WriteColor32BppRgb(byte[] bytes, int index, Color color) => WriteColor32BppRgb(bytes, index, (byte)(color.R * byte.MaxValue), (byte)(color.G * byte.MaxValue), (byte)(color.B * byte.MaxValue));
		private static void WriteColor32BppRgb(byte[] bytes, int index, byte r, byte g, byte b)
		{
			bytes[4 * index] = b;
			bytes[4 * index + 1] = g;
			bytes[4 * index + 2] = r;
		}
		private static void WriteColor32BppRgb(Span<byte> bytes, int index, Color color) => WriteColor32BppRgb(bytes, index, (byte)(color.R * byte.MaxValue), (byte)(color.G * byte.MaxValue), (byte)(color.B * byte.MaxValue));
		private static void WriteColor32BppRgb(Span<byte> bytes, int index, byte r, byte g, byte b)
		{
			bytes[4 * index] = b;
			bytes[4 * index + 1] = g;
			bytes[4 * index + 2] = r;
		}
	}
}
