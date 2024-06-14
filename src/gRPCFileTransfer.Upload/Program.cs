using Google.Protobuf;
using Grpc.Net.Client;
using gRPCFileTransferServer;
using FileInfo = gRPCFileTransferServer.FileInfo;

var channel = GrpcChannel.ForAddress("https://localhost:5001");
var client = new FileTransferDefinition.FileTransferDefinitionClient(channel);

// Define your file path
string filePath = @"~\src\gRPCFileTransfer.Upload\Files\OllamaSetup.exe";

using FileStream fileStream = new(filePath, FileMode.Open);
var content = new BytesContent()
{
    FileSize = fileStream.Length,
    ReadedByte = 0,
    Info = new FileInfo
    {
        FileName = Path.GetFileNameWithoutExtension(fileStream.Name),
        FileExtension = Path.GetExtension(fileStream.Name)
    }
};

var upload = client.FileUpload();
byte[] buffer = new byte[2048];

while ((content.ReadedByte = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
{
    content.Buffer = ByteString.CopyFrom(buffer);
    await upload.RequestStream.WriteAsync(content);
}

await upload.RequestStream.CompleteAsync();
fileStream.Close();