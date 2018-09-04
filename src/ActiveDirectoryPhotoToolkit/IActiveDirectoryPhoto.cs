﻿using System;
using System.Diagnostics.Contracts;

namespace ActiveDirectoryPhotoToolkit
{
    [ContractClass(typeof(ActiveDirectoryPhotoContracts))]
    public interface IActiveDirectoryPhoto
    {
        byte[] GetThumbnailPhoto(string userName, ActiveDirectoryPhoto.Format format);
        void SetThumbnailPhoto(string userName, string thumbNailLocation);
        void SaveThumbnailToDisk(byte[] thumbNail, string location);
    }
}
