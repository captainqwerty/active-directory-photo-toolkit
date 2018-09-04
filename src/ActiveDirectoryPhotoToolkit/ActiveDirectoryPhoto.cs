﻿using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using System.IO;



namespace ActiveDirectoryPhotoToolkit
{
    public class ActiveDirectoryPhoto : IActiveDirectoryPhoto
    {
        public enum Format
        {
            BMP, JPG, PNG
        }

        public byte[] GetThumbnailPhoto(string userName, Format format)
        {
            const int imageQuality = 95;
            var imageSize = new Size(96, 96);

            var principalContext = new PrincipalContext(ContextType.Domain);

            var userPrincipal = new UserPrincipal(principalContext)
            {
                SamAccountName = userName
            };

            var principalSearcher = new PrincipalSearcher
            {
                QueryFilter = userPrincipal
            };

            var result = principalSearcher.FindOne();

            if (result != null)
            {
                using (var entry = result.GetUnderlyingObject() as DirectoryEntry)
                {
                    if (entry.Properties["thumbnailPhoto"] != null)
                    {
                        //if bitmap just return this...
                        var bytes = entry.Properties["thumbnailPhoto"][0] as byte[];

                        //if not bimap do above then do this...
                        using (var inStream = new MemoryStream(bytes))
                        {
                            using (var outStream = new MemoryStream())
                            {
                                using (var imageFactory = new ImageFactory())
                                {
                                    imageFactory.Load(inStream);

                                    switch (format)
                                    {
                                        case Format.JPG:
                                            imageFactory.Format(new JpegFormat());
                                            break;
                                        case Format.PNG:
                                            imageFactory.Format(new PngFormat());
                                            break;
                                        case Format.BMP:
                                        default:
                                            imageFactory.Format(new BitmapFormat());
                                            break;
                                    }

                                    imageFactory.Resize(imageSize);
                                    imageFactory.Quality(imageQuality);
                                    imageFactory.Save(outStream);
                                }

                                // rewind the memory stream so that it can be exported.
                                outStream.Position = 0;

                                return outStream.ToArray();
                            }
                        }
                    }
                }
            }

            return null;
        }

        public void SetThumbnailPhoto(string userName, string thumbNailLocation)
        {
           var principalContext = new PrincipalContext(ContextType.Domain);

            var userPrincipal = new UserPrincipal(principalContext)
            {
                SamAccountName = userName
            };

            var principalSearcher = new PrincipalSearcher
            {
                QueryFilter = userPrincipal
            };

            var result = principalSearcher.FindOne();

            if (result != null)
            {
                byte[] bytes = File.ReadAllBytes(thumbNailLocation);

                using (var entry = result.GetUnderlyingObject() as DirectoryEntry)
                {
                    if (entry.Properties["thumbnailPhoto"] != null)
                    {
                        //if bitmap just return this...
                        entry.Properties["thumbnailPhoto"][0] = bytes;
                        entry.CommitChanges();
                    }
                }
            }
        }

        public void SaveThumbnailToDisk(byte[] thumbNail, string location)
        {
            throw new System.NotImplementedException();
        }
    }
}