using System;
using System.IO;
using BlogPosts.Models;
using Microsoft.AspNetCore.Http;

namespace BlogPosts.Utilities
{
    public class ImageHelper
    {
        // This method helps me get the image
        public static string GetImage( Post post )
        {
            string  binary       = Convert.ToBase64String( post.Image );
            string? ext          = Path.GetExtension( post.FileName );
            string  imageDataURL = $"data:image/{ext};base64,{binary}";

            return imageDataURL;
        }

        // This method helps me encode the image
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
