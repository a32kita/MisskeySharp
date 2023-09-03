using MisskeySharp.Entities;
using MisskeySharp.InternalUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MisskeySharp.ClientEndpoints
{
    public class Drive_Files : EndpointBase
    {
        internal Drive_Files(MisskeyService parent)
            : base(parent) { }

        public async Task<FileCollection> Get(DriveFilesQuery query)
        {
            return await this.Parent.PostAsync<DriveFilesQuery, FileCollection>("drive/files", query);
        }

        public async Task<Entities.File> Create(FileUploadRequest uploadRequest)
        {
            var uploadContents = new List<MultipartUploadContent>();

            using (var br = new BinaryReader(uploadRequest.ContentStream))
            {
                var buffer = br.ReadBytes((int)br.BaseStream.Length);

                uploadContents.Add(new MultipartUploadContent()
                {
                    Name = "file",
                    FileName = "blob",
                    ContentType = uploadRequest.ContentType,
                    ByteContent = buffer,
                });

                uploadContents.Add(new MultipartUploadContent()
                {
                    Name = "name",
                    StringContent = uploadRequest.FileName,
                });
            }

            return await this.Parent.PostFormDataAsync<Entities.File>("drive/files/create", uploadContents);
        }
    }
}
