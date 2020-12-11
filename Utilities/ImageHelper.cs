#nullable enable
using System;
using System.IO;
using BragiBlogPoster.Models;
using Microsoft.AspNetCore.Http;

namespace BragiBlogPoster.Utilities
{
    public class ImageHelper
    {
        // This method helps get the image
        public static string GetImage( Post post )
        {
            string  binary       = Convert.ToBase64String( post.Image );
            string? ext          = Path.GetExtension( post.FileName );
            string  imageDataUrl = $"data:image/{ext};base64,{binary}";

            return imageDataUrl;
        }

        // This method helps encode the image
        public static byte[] EncodeImage( IFormFile image )
        {
            MemoryStream ms = new MemoryStream( );
            image.CopyTo( ms );
            byte[] output = ms.ToArray( );

            ms.Close( );
            ms.Dispose( );

            return output;
        }
    }
}
