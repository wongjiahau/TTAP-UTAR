using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace Time_Table_Arranging_Program.Class.Helper {
    public static class Helper {
        public static string RawStringOfTestFile(string fileName, string nameSpace = "NUnit.Tests2.TestFiles.") {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = nameSpace + fileName;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream)) {
                return reader.ReadToEnd();
            }
        }

        public static BitArray ToBitArray(this List<int> input) {
            int length = 32;
            var vector = new BitArray(length);
            for (int i = 0; i < input.Count; i++) {
                vector[input[i]] = true;
            }
            return vector;
        }

        public static BitArray ToBitArray(this int x) {
            return new BitArray(new int[] {x});

            string s = Convert.ToString(x, 2); //Convert to binary in a string

            int[] bits = s.PadLeft(8, '0') // Add 0's from left
                .Select(c => int.Parse(c.ToString())) // convert each char to int
                .ToArray(); // Convert IEnumerable from select to Array
            return new BitArray(bits);
        }

        public static int ToInt(this BitArray bitArray) {
            if (bitArray.Length > 32)
                throw new ArgumentException("Argument length shall be at most 32 bits.");

            int[] array = new int[1];
            bitArray.CopyTo(array, 0);
            return array[0];
        }

        public static RenderTargetBitmap GetImage(UserControl view) {
            int qualityFactor = 10;
            int dotsPerInch = 96;
            Size size = new Size((int) view.ActualWidth * qualityFactor, (int) view.ActualHeight * qualityFactor);
            if (size.IsEmpty)
                return null;

            RenderTargetBitmap result = new RenderTargetBitmap((int) size.Width, (int) size.Height, dotsPerInch,
                dotsPerInch, PixelFormats.Pbgra32);
            DrawingVisual drawingvisual = new DrawingVisual();
            using (DrawingContext context = drawingvisual.RenderOpen()) {
                context.DrawRectangle(new VisualBrush(view), null, new Rect(new Point(), size));
                context.Close();
            }

            result.Render(drawingvisual);
            return result;
        }

        public static string Reverse(this string s) {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }


        public static void SaveAsPng(RenderTargetBitmap src, string targetPath) {
            Stream outputStream = File.Create(targetPath);
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(src));
            encoder.Save(outputStream);
            outputStream.Close();
        }

        /// <summary>
        /// To check if an element if visible to the user within a certain container
        /// Refer StackOverflow : "In WPF, how can I determine whether a control is visible to the user?"
        /// </summary>
        /// <param name="element"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public static bool IsVisibleToUser(this FrameworkElement element, FrameworkElement container) {
            if (!element.IsVisible) return false;
            Rect bounds = element.TransformToAncestor(container)
                .TransformBounds(new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));
            Rect rect = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);
            return rect.Contains(bounds.TopLeft) || rect.Contains(bounds.BottomRight);
        }

        /// <summary>
        /// Return true if there are Internet connection, else false
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool CanConnectToWebsite(string url) {
            try {
                using (var client = new WebClient()) {
                    using (client.OpenRead(url)) {
                        return true;
                    }
                }
            }
            catch {
                return false;
            }
        }
    }
}