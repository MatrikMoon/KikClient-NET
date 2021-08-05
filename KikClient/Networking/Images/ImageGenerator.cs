using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

/**
 * Created by Moon on 2/13/2021
 * Generates text images based on input
 */

namespace KikClient.Networking.Images
{
    static class ImageGenerator
    {
        /// <summary>
        /// Generates a png byte array for the given input text and text options
        /// </summary>
        /// <param name="text">The text to generate</param>
        /// <param name="font">The font to write in</param>
        /// <param name="textColor">The desired text color</param>
        /// <param name="backColor">The desired background color</param>
        /// <returns>A png in the form of an array of bytes</returns>
        internal static byte[] DrawText(string text, Font font, Color textColor, Color backColor)
        {
            //Measure the string to see how big the image needs to be
            using (var sizeDummyBitmap = new Bitmap(1, 1))
            using (var sizeDummyGraphcs = Graphics.FromImage(sizeDummyBitmap))
            {
                var textSize = sizeDummyGraphcs.MeasureString(text, font);

                //Create actual image
                using (var bitmap = new Bitmap((int)textSize.Width, (int)textSize.Height))
                using (var graphics = Graphics.FromImage(bitmap))
                using (var textBrush = new SolidBrush(textColor))
                using (var output = new MemoryStream())
                {
                    graphics.Clear(backColor);
                    graphics.DrawString(text, font, textBrush, 0, 0);
                    graphics.Save();

                    bitmap.Save(output, ImageFormat.Png);

                    return output.ToArray();
                }
            }
        }

        public static void TestDrawText()
        {
            File.WriteAllBytes("testoutput.png", DrawText("aaaaaaaa aa adfias sdf ahs", new Font(FontFamily.GenericSansSerif, 12), Color.Blue, Color.Black));
        }
    }
}
