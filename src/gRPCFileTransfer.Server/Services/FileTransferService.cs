using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using gRPCFileTransferServer;
using FileInfo = gRPCFileTransferServer.FileInfo;

namespace gRPCFileTransfer.Server.Services
{
    public class FileTransferService(IWebHostEnvironment webHostEnvironment) : FileTransferDefinition.FileTransferDefinitionBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        public override async Task<Empty> FileUpload(IAsyncStreamReader<BytesContent> requestStream, ServerCallContext context)
        {
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "Files");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            FileStream fileStream = null!;

            try
            {
                int count = 0;
                decimal chunkSize = 0;

                while (await requestStream.MoveNext())
                {
                    if (count++ is 0)
                    {
                        fileStream = new FileStream($"{path}/{requestStream.Current.Info.FileName}{requestStream.Current.Info.FileExtension}", FileMode.Create);
                        fileStream.SetLength(requestStream.Current.FileSize);
                    }

                    var buffer = requestStream.Current.Buffer.ToByteArray();

                    await fileStream.WriteAsync(buffer, 0, buffer.Length);

                    var percentage = ((chunkSize += requestStream.Current.ReadedByte) * 100) / requestStream.Current.FileSize;

                    Console.WriteLine($"{Math.Round(percentage)}%");
                }
            }
            catch
            {
                Console.WriteLine("An error occurred while uploading the file.");
            }

            await fileStream.DisposeAsync();
            fileStream.Close();

            Console.WriteLine("File uploaded successfully.");

            return new Empty();
        }

        public override async Task FileDownload(gRPCFileTransferServer.FileInfo request, IServerStreamWriter<BytesContent> responseStream, ServerCallContext context)
        {
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "Files");

            FileStream fileStream = new($"{path}/{request.FileName}{request.FileExtension}", FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[2048];

            BytesContent content = new()
            {
                FileSize = fileStream.Length,
                Info = new FileInfo
                {
                    FileName = Path.GetFileNameWithoutExtension(fileStream.Name),
                    FileExtension = Path.GetExtension(fileStream.Name)
                },
                ReadedByte = 0
            };

            while ((content.ReadedByte = await fileStream.ReadAsync(buffer)) > 0)
            {
                content.Buffer = ByteString.CopyFrom(buffer);
                await responseStream.WriteAsync(content);
            }

            fileStream.Close();
        }
    }
}
