# gRPC File Transfer System

A file transfer system utilizing **gRPC** with both server and client implementations, built on **.NET 8**.

---

## 📌 Architectural Overview

This project demonstrates a file transfer system using gRPC, focusing on efficient and reliable communication between clients and the server. Key features include:

- **gRPC Communication**: Utilizes gRPC for high-performance, cross-platform communication.
- **Asynchronous File Transfer**: Supports asynchronous streaming for file uploads and downloads.
- **Scalability**: Designed to handle multiple clients concurrently.

---

## 🏗️ Project Components

### 📁 gRPCFileTransfer.sln

The main solution file that includes the following projects:

- **Server**: Hosts the gRPC service and handles incoming file transfer requests.
- **Client**: Connects to the gRPC server to upload or download files.

---

## 🚀 Running the Project Locally

### 📌 Prerequisites

- **.NET 8.0 SDK** installed on your system.

### 🔧 Setup Instructions

1. **Clone the repository**:
   ```sh
   git clone https://github.com/enesmetek/grpc-filetransfer.git
   ```
2. **Navigate into the project directory**:
   ```sh
   cd grpc-filetransfer
   ```
3. **Restore dependencies**:
   ```sh
   dotnet restore
   ```
4. **Build the solution**:
   ```sh
   dotnet build
   ```

### 🖥️ Running the Server

1. **Navigate to the server project directory**:
   ```sh
   cd src/Server
   ```
2. **Run the server**:
   ```sh
   dotnet run
   ```

   The server will start and listen for incoming gRPC connections.

### 🖥️ Running the Client

1. **Open a new terminal window**.
2. **Navigate to the client project directory**:
   ```sh
   cd src/Client
   ```
3. **Run the client with the desired operation**:
   ```sh
   dotnet run -- [upload/download] [file_path]
   ```

   Replace `[upload/download]` with the desired operation and `[file_path]` with the path to the file you wish to upload or download.

---

## 📜 License

This project is licensed under the **MIT License**.

---

## 🤝 Contributing

Contributions are welcome! Feel free to submit a pull request or open an issue.

---

## 📧 Contact

For any questions or issues, please reach out via GitHub Issues or email me at **[emkafali@gmail.com]**.

---

### 📢 Star the Repository ⭐

If you found this project useful, consider giving it a star on GitHub! 😊
