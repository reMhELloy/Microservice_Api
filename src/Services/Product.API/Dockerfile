# Stage 1: Sử dụng ASP.NET Core runtime làm base image
# mcr.microsoft.com/dotnet/aspnet:6.0 là image chính thức từ Microsoft chứa runtime để chạy ứng dụng
# ASP.NET Core, nhẹ hơn SDK vì chỉ chứa những thành phần cần thiết để chạy ứng dụng
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
# Thiết lập thư mục làm việc trong container
WORKDIR /app
# Khai báo port 80 sẽ được sử dụng bởi ứng dụng
EXPOSE 80

# Stage 2: Build stage - Sử dụng .NET SDK để build ứng dụng
# sdk:6.0 chứa đầy đủ công cụ để build và phát triển ứng dụng .NET
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
# Thiết lập thư mục làm việc cho quá trình build
WORKDIR /src

# Copy các file project (.csproj) 
# Copy riêng các file project để tận dụng Docker cache layer
# Nếu các dependencies không thay đổi, Docker sẽ sử dụng cached layer
COPY ["Services/Product.API/Product.API.csproj", "Services/Product.API/"]
COPY ["BuildingBlocks/Common.Logging/Common.Logging.csproj", "BuildingBlocks/Common.Logging/"]
COPY ["BuildingBlocks/Contracts/Contracts.csproj", "BuildingBlocks/Contracts/"]
COPY ["BuildingBlocks/Infrastructure/Infrastructure.csproj", "BuildingBlocks/Infrastructure/"]
COPY ["BuildingBlocks/Shared/Shared.csproj", "BuildingBlocks/Shared/"]

# Khôi phục các NuGet packages cho project
# dotnet restore tải về các packages được định nghĩa trong các file .csproj
RUN dotnet restore "Services/Product.API/Product.API.csproj"

# Copy toàn bộ source code từ host vào container
# Thực hiện sau restore để tận dụng cache
COPY . .

# Di chuyển vào thư mục chứa project chính
WORKDIR "/src/Services/Product.API"

# Build project ở chế độ Release
# -c Release: build ở chế độ Release (đã được tối ưu hóa)
# -o /app/build: output đến thư mục /app/build
RUN dotnet build "Product.API.csproj" -c Release -o /app/build

# Stage 3: Publishing stage - Tạo bản publish của ứng dụng
FROM build AS publish
# Publish ứng dụng
# Tạo ra phiên bản đã được tối ưu và sẵn sàng để deploy
RUN dotnet publish "Product.API.csproj" -c Release -o /app/publish

# Stage 4: Final stage - Tạo image cuối cùng
FROM base as final
# Thiết lập thư mục làm việc
WORKDIR /app
# Copy các file đã được publish từ stage publish
# --from=publish chỉ định copy từ stage publish
COPY --from=publish /app/publish .
# Thiết lập command để chạy ứng dụng
# Khi container khởi động sẽ chạy lệnh: dotnet Product.API.dll
ENTRYPOINT ["dotnet", "Product.API.dll"]