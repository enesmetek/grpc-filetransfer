using Grpc.Net.Client;
using gRPCFileTransferServer;
using FileInfo = gRPCFileTransferServer.FileInfo;

var channel = GrpcChannel.ForAddress("https://localhost:5001");
var client = new FileTransferDefinition.FileTransferDefinitionClient(channel);

// Define your download path
string downloadPath = @"~\src\gRPCFileTransfer.Download\Files\";

// Define your file info
FileInfo fileInfo = new()
{
    FileName = "OllamaSetup",
    FileExtension = ".exe"
};

FileStream fileStream = null!;

var request = client.FileDownload(fileInfo);

CancellationTokenSource cancellationTokenSource = new();

int count = 0;
decimal chunkSize = 0;

while (await request.ResponseStream.MoveNext(cancellationTokenSource.Token))
{
    if (count++ is 0)
    {
        fileStream = new FileStream(@$"{downloadPath}\{request.ResponseStream.Current.Info.FileName}{request.ResponseStream.Current.Info.FileExtension}", FileMode.CreateNew);
        fileStream.SetLength(request.ResponseStream.Current.FileSize);
    }

    var buffer = request.ResponseStream.Current.Buffer.ToByteArray();
    await fileStream.WriteAsync(buffer, 0, request.ResponseStream.Current.ReadedByte);

    var percentage = ((chunkSize += request.ResponseStream.Current.ReadedByte) * 100) / request.ResponseStream.Current.FileSize;

    Console.WriteLine($"{Math.Round(percentage)}%");
}

Console.WriteLine("File downloaded successfully.");

await fileStream.DisposeAsync();
fileStream.Close();

