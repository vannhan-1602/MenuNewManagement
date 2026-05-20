\# MenuNewsManagement



&#x20;Quản lý Menu và Tin tức (News) được xây dựng trên nền tảng \*\*ASP.NET Core 8 Web API\*\* theo kiến trúc \*\*Clean Architecture\*\*. Dự án được thiết kế như một ứng dụng enterprise thực tế, tích hợp đầy đủ các công nghệ hiện đại bao gồm CQRS với MediatR, EF Core DB First, MongoDB audit log, RabbitMQ message broker và gRPC.



\---



\## Mục lục



1\. \[Giới thiệu dự án](#1-giới-thiệu-dự-án)

2\. \[Kiến trúc Clean Architecture](#2-kiến-trúc-clean-architecture)

3\. \[Cấu trúc thư mục](#3-cấu-trúc-thư-mục)

4\. \[Công nghệ sử dụng](#4-công-nghệ-sử-dụng)

5\. \[Yêu cầu hệ thống](#5-yêu-cầu-hệ-thống)

6\. \[Hướng dẫn cài đặt môi trường](#6-hướng-dẫn-cài-đặt-môi-trường)

7\. \[Hướng dẫn clone source code](#7-hướng-dẫn-clone-source-code)

8\. \[Hướng dẫn database](#8-hướng-dẫn-database)

9\. \[Cấu hình appsettings.json](#9-cấu-hình-appsettingsjson)

10\. \[Hướng dẫn chạy project](#10-hướng-dẫn-chạy-project)

11\. \[Hướng dẫn test API](#11-hướng-dẫn-test-api)

12\. \[Hướng dẫn RabbitMQ](#12-hướng-dẫn-rabbitmq)

13\. \[Hướng dẫn MongoDB](#13-hướng-dẫn-mongodb)

14\. \[Hướng dẫn gRPC](#14-hướng-dẫn-grpc)

15\. \[Flow hệ thống](#15-flow-hệ-thống)

16\. \[Roadmap / Future Improvements](#16-roadmap--future-improvements)



\---



\## 1. Giới thiệu dự án



\### Mục tiêu project



MenuNewsManagement được xây dựng nhằm mục đích quản lý danh mục Menu và các bài viết Tin tức trong một hệ thống nội dung số. Một Menu có thể chứa nhiều News, và một News có thể thuộc nhiều Menu (quan hệ nhiều-nhiều). Dự án đồng thời đóng vai trò là một \*\*reference project\*\* giúp người học hiểu và thực hành Clean Architecture, CQRS, message broker, audit log và gRPC trong một ngữ cảnh thực tế.



\### Chức năng chính



\- Quản lý Menu: tạo mới, cập nhật, xóa mềm (soft delete), lấy danh sách có phân trang, lấy theo ID.

\- Quản lý News: tạo mới, cập nhật, xóa mềm, lấy danh sách có phân trang, lấy theo ID.

\- Gán News vào Menu (quan hệ nhiều-nhiều qua bảng trung gian MenuNews).

\- Truy vấn ngược: lấy danh sách Menu theo News và danh sách News theo Menu.

\- Tự động publish event lên RabbitMQ mỗi khi một News được tạo mới.

\- Ghi audit log vào MongoDB cho tất cả các thao tác tạo dữ liệu.

\- Cung cấp gRPC endpoint để truy vấn News với hiệu năng cao.

\- Trả về response chuẩn hóa `ApiResponse<T>` cho tất cả các endpoint REST.

\- Validate dữ liệu đầu vào tự động qua FluentValidation pipeline.

\- Xử lý lỗi tập trung qua `ExceptionHandlingMiddleware`.



\### Kiến trúc sử dụng



Dự án tuân theo \*\*Clean Architecture\*\* (còn gọi là Onion Architecture hoặc Ports \& Adapters), với nguyên tắc cốt lõi là các layer bên trong không phụ thuộc vào các layer bên ngoài. Business logic hoàn toàn tách biệt khỏi database, framework và các dịch vụ bên ngoài.



Bên cạnh đó, dự án áp dụng pattern \*\*CQRS (Command Query Responsibility Segregation)\*\*: tách riêng các thao tác ghi dữ liệu (Command) và đọc dữ liệu (Query) để dễ bảo trì, mở rộng và kiểm thử độc lập.



\### Công nghệ sử dụng



\- \*\*.NET 8\*\* — nền tảng runtime và framework chính

\- \*\*Clean Architecture\*\* — tổ chức codebase theo layer, tách biệt business logic

\- \*\*CQRS\*\* — tách Command và Query thông qua MediatR

\- \*\*MediatR\*\* — thư viện Mediator Pattern, dispatcher cho Command/Query

\- \*\*EF Core DB First\*\* — scaffold entity từ database SQL Server có sẵn

\- \*\*SQL Server 2022\*\* — database chính lưu trữ dữ liệu nghiệp vụ

\- \*\*MongoDB\*\* — lưu audit log phi cấu trúc

\- \*\*RabbitMQ\*\* — message broker, publish/consume event bất đồng bộ

\- \*\*gRPC\*\* — giao thức RPC hiệu năng cao cho truy vấn News

\- \*\*FluentValidation\*\* — validate dữ liệu đầu vào theo pipeline

\- \*\*AutoMapper\*\* — mapping giữa Entity và DTO

\- \*\*Swagger / Scalar\*\* — giao diện tương tác test API

\- \*\*Docker Compose\*\* — khởi động toàn bộ infrastructure chỉ với một lệnh



\---



\## 2. Kiến trúc Clean Architecture



Clean Architecture chia ứng dụng thành nhiều vòng tròn đồng tâm (layer). \*\*Quy tắc quan trọng nhất\*\*: dependency chỉ được phép trỏ từ ngoài vào trong — layer ngoài phụ thuộc vào layer trong, tuyệt đối không được phép ngược lại.



```

+--------------------------------------------------+

|                      API                         |  <-- Ngoài cùng: HTTP, gRPC, Middleware

+--------------------------------------------------+

|               Infrastructure                     |  <-- MongoDB, RabbitMQ, CurrentUser

+--------------------------------------------------+

|               Persistence                        |  <-- EF Core, SQL Server, Repositories

+--------------------------------------------------+

|               Application                        |  <-- CQRS, MediatR, Validators, DTOs

+--------------------------------------------------+

|                  Domain                          |  <-- Entities, Interfaces (không phụ thuộc gì)

+--------------------------------------------------+

```



\### Domain (lõi trong cùng)



Domain là trung tâm của toàn bộ hệ thống. Layer này chứa:



\- \*\*Entities\*\*: `Menu`, `NewsItem`, `MenuNews` — đây là các đối tượng nghiệp vụ thuần túy, không phụ thuộc vào bất kỳ framework nào.

\- \*\*Interfaces\*\*: `IMenuRepository`, `INewsRepository`, `IMenuNewsRepository`, `INewsEventPublisher`, `IAuditLogService`, `IAuditLogQueryService` — định nghĩa contract mà các layer ngoài phải implement.

\- \*\*Common\*\*: `BaseEntity`, `ApiResponse<T>`, `PaginatedList<T>` — các class dùng chung.



Domain không import bất kỳ NuGet package nào ngoài thư viện chuẩn .NET.



\### Application



Application là nơi chứa toàn bộ \*\*business logic\*\* của ứng dụng. Layer này chứa:



\- \*\*Features\*\*: tổ chức theo tính năng, mỗi tính năng có thư mục Commands và Queries riêng.

\- \*\*Commands\*\*: `CreateNewsCommand`, `UpdateNewsCommand`, `DeleteNewsCommand`, `CreateMenuCommand`, `UpdateMenuCommand`, `DeleteMenuCommand`, `AssignNewsToMenuCommand` — mỗi Command có Handler và Validator tương ứng.

\- \*\*Queries\*\*: `GetAllNewsQuery`, `GetNewsByIdQuery`, `GetAllMenusQuery`, `GetMenuByIdQuery`, `GetNewsByMenuQuery`, `GetMenusByNewsQuery` — mỗi Query có Handler và Validator tương ứng.

\- \*\*Behaviors\*\*: `ValidationBehavior<TRequest, TResponse>` — tự động validate mọi Command/Query trước khi Handler được gọi, nhờ MediatR Pipeline.

\- \*\*DTOs\*\*: `NewsDto`, `MenuDto` — đối tượng truyền dữ liệu ra bên ngoài, tách biệt với Entity.

\- \*\*Mappings\*\*: `MappingProfile` — cấu hình AutoMapper giữa Entity và DTO.



Application chỉ phụ thuộc vào Domain. Nó không biết SQL Server, MongoDB hay RabbitMQ là gì.



\### Persistence



Persistence là nơi implement các Repository Interface mà Domain đã định nghĩa. Layer này chứa:



\- \*\*MenuNewsDbContext\*\*: DbContext của EF Core, được cấu hình theo kiểu DB First (schema khớp với bảng SQL Server đã tạo sẵn).

\- \*\*Repositories\*\*: `MenuRepository`, `NewsRepository`, `MenuNewsRepository` — implement `IMenuRepository`, `INewsRepository`, `IMenuNewsRepository` từ Domain.

\- Cấu hình soft delete thông qua `HasQueryFilter` — mọi query tự động lọc `IsDeleted = false`.



Persistence chỉ phụ thuộc vào Domain và EF Core.



\### Infrastructure



Infrastructure implement các dịch vụ kỹ thuật không liên quan đến database SQL:



\- \*\*MongoDb\*\*: `MongoAuditLogService` (implement `IAuditLogService`), `MongoAuditLogQueryService` (implement `IAuditLogQueryService`), `AuditLogDocument` — document model trong MongoDB.

\- \*\*RabbitMq\*\*: `RabbitMqNewsEventPublisher` (implement `INewsEventPublisher`), `NewsCreatedConsumerHostedService` — background service lắng nghe message từ queue.

\- \*\*Services\*\*: `CurrentUserService` (implement `ICurrentUserService`) — lấy thông tin user hiện tại từ HttpContext.

\- \*\*Options\*\*: `MongoDbSettings`, `RabbitMqSettings` — strongly-typed configuration.

\- Hỗ trợ \*\*No-Op\*\* implementations (`NoOpAuditLogService`, `NoOpNewsEventPublisher`) khi MongoDB hoặc RabbitMQ bị tắt qua cấu hình `Enabled: false`.



\### API



API là layer ngoài cùng, tiếp nhận request từ client:



\- \*\*Controllers\*\*: `NewsController`, `MenusController`, `MenuNewsController`, `SystemController` — nhận HTTP request và gọi MediatR.

\- \*\*GrpcServices\*\*: `NewsGrpcService` — expose gRPC endpoint, bên trong cũng gọi MediatR Query handlers.

\- \*\*Middleware\*\*: `ExceptionHandlingMiddleware` — bắt toàn bộ exception chưa được xử lý, trả về response lỗi chuẩn hóa.

\- \*\*Program.cs\*\*: đăng ký DI, cấu hình middleware pipeline, mapping route.



\### Luồng hoạt động giữa các layer



Khi một HTTP request đến endpoint `POST /api/news`:



1\. `NewsController` nhận request, tạo `CreateNewsCommand` và gọi `\_mediator.Send(command)`.

2\. MediatR pipeline gọi `ValidationBehavior` trước, FluentValidation kiểm tra dữ liệu. Nếu lỗi, ném `ValidationException`.

3\. Nếu hợp lệ, MediatR gọi `CreateNewsCommandHandler.Handle()`.

4\. Handler gọi `INewsRepository.AddAsync()` — Persistence thực thi INSERT vào SQL Server.

5\. Handler gọi `INewsEventPublisher.PublishNewsCreatedAsync()` — Infrastructure gửi message lên RabbitMQ.

6\. Handler gọi `IAuditLogService.LogAsync()` — Infrastructure ghi document vào MongoDB.

7\. Handler trả về `NewsDto`. Controller đóng gói vào `ApiResponse<T>` và trả về HTTP 200.



\---



\## 3. Cấu trúc thư mục



```

MENUNEWSMANAGEMENT/

│

├── database/

│   └── 01\_CreateDatabase.sql

│

├── scripts/

│   └── start-infrastructure.ps1

│

├── src/

│   ├── Api/

│   │   ├── Controllers/

│   │   │   ├── MenuNewsController.cs

│   │   │   ├── MenusController.cs

│   │   │   ├── NewsController.cs

│   │   │   └── SystemController.cs

│   │   ├── GrpcServices/

│   │   │   └── NewsGrpcService.cs

│   │   ├── Middleware/

│   │   │   └── ExceptionHandlingMiddleware.cs

│   │   ├── Protos/

│   │   │   └── news.proto

│   │   ├── Properties/

│   │   │   └── launchSettings.json

│   │   ├── appsettings.json

│   │   ├── appsettings.Development.json

│   │   ├── appsettings.Docker.json

│   │   ├── Program.cs

│   │   └── MenuNews.Api.csproj

│   │

│   ├── Application/

│   │   ├── Common/

│   │   │   ├── Behaviors/

│   │   │   │   └── ValidationBehavior.cs

│   │   │   ├── Interfaces/

│   │   │   │   └── ICurrentUserService.cs

│   │   │   └── Mappings/

│   │   │       └── MappingProfile.cs

│   │   ├── Features/

│   │   │   ├── MenuNews/

│   │   │   │   ├── Commands/

│   │   │   │   │   └── AssignNewsToMenu/

│   │   │   │   │       ├── AssignNewsToMenuCommand.cs

│   │   │   │   │       ├── AssignNewsToMenuCommandHandler.cs

│   │   │   │   │       └── AssignNewsToMenuCommandValidator.cs

│   │   │   │   └── Queries/

│   │   │   │       ├── GetMenusByNews/

│   │   │   │       └── GetNewsByMenu/

│   │   │   ├── Menus/

│   │   │   │   ├── Commands/

│   │   │   │   │   ├── CreateMenu/

│   │   │   │   │   ├── DeleteMenu/

│   │   │   │   │   └── UpdateMenu/

│   │   │   │   ├── Dtos/

│   │   │   │   │   └── MenuDto.cs

│   │   │   │   └── Queries/

│   │   │   │       ├── GetAllMenus/

│   │   │   │       └── GetMenuById/

│   │   │   └── News/

│   │   │       ├── Commands/

│   │   │       │   ├── CreateNews/

│   │   │       │   │   ├── CreateNewsCommand.cs

│   │   │       │   │   ├── CreateNewsCommandHandler.cs

│   │   │       │   │   └── CreateNewsCommandValidator.cs

│   │   │       │   ├── DeleteNews/

│   │   │       │   └── UpdateNews/

│   │   │       ├── Dtos/

│   │   │       │   └── NewsDto.cs

│   │   │       └── Queries/

│   │   │           ├── GetAllNews/

│   │   │           └── GetNewsById/

│   │   ├── DependencyInjection.cs

│   │   └── MenuNews.Application.csproj

│   │

│   ├── Domain/

│   │   ├── Common/

│   │   │   ├── ApiResponse.cs

│   │   │   ├── BaseEntity.cs

│   │   │   └── PaginatedList.cs

│   │   ├── Entities/

│   │   │   ├── Menu.cs

│   │   │   ├── MenuNews.cs

│   │   │   └── NewsItem.cs

│   │   ├── Interfaces/

│   │   │   ├── IAuditLogQueryService.cs

│   │   │   ├── IAuditLogService.cs

│   │   │   ├── IMenuNewsRepository.cs

│   │   │   ├── IMenuRepository.cs

│   │   │   ├── INewsEventPublisher.cs

│   │   │   └── INewsRepository.cs

│   │   └── MenuNews.Domain.csproj

│   │

│   ├── Infrastructure/

│   │   ├── MongoDb/

│   │   │   ├── AuditLogDocument.cs

│   │   │   ├── MongoAuditLogQueryService.cs

│   │   │   ├── MongoAuditLogService.cs

│   │   │   ├── NoOpAuditLogQueryService.cs

│   │   │   └── NoOpAuditLogService.cs

│   │   ├── Options/

│   │   │   ├── MongoDbSettings.cs

│   │   │   └── RabbitMqSettings.cs

│   │   ├── RabbitMq/

│   │   │   ├── NewsCreatedConsumerHostedService.cs

│   │   │   ├── NewsCreatedMessage.cs

│   │   │   ├── NoOpNewsEventPublisher.cs

│   │   │   └── RabbitMqNewsEventPublisher.cs

│   │   ├── Services/

│   │   │   └── CurrentUserService.cs

│   │   ├── DependencyInjection.cs

│   │   └── MenuNews.Infrastructure.csproj

│   │

│   └── Persistence/

│       ├── Context/

│       │   └── MenuNewsDbContext.cs

│       ├── Repositories/

│       │   ├── MenuNewsRepository.cs

│       │   ├── MenuRepository.cs

│       │   └── NewsRepository.cs

│       ├── DependencyInjection.cs

│       └── MenuNews.Persistence.csproj

│

├── tests/

│   └── MenuNews.UnitTests/

│       └── Validators/

│           ├── CreateMenuCommandValidatorTests.cs

│           └── CreateNewsCommandValidatorTests.cs

│

├── docker-compose.yml

└── MenuNewsManagement.slnx

```



\### Giải thích từng thư mục



\*\*database/\*\*

Chứa script SQL khởi tạo database. File `01\_CreateDatabase.sql` tạo database `MenuNewsDb`, tạo ba bảng `Menus`, `News`, `MenuNews`, định nghĩa khóa chính, khóa ngoại, index và chèn dữ liệu mẫu. Script này phải được chạy trước khi scaffold EF Core.



\*\*scripts/\*\*

Chứa PowerShell script `start-infrastructure.ps1` dùng để khởi động toàn bộ container Docker (SQL Server, MongoDB, RabbitMQ) chỉ bằng một lệnh. Script kiểm tra Docker Desktop đang chạy, thực thi `docker compose up -d` và in ra thông tin kết nối.



\*\*src/Api/\*\*

Entry point của ứng dụng. Đăng ký các service, cấu hình middleware pipeline, khai báo gRPC service và expose Swagger UI. Không chứa business logic.



\*\*src/Application/\*\*

Business use case layer. Mỗi tính năng được tổ chức theo thư mục riêng, với từng Command/Query có file Command (request object), Handler (xử lý logic) và Validator (kiểm tra đầu vào) tách biệt.



\*\*src/Domain/\*\*

Trái tim của hệ thống. Chỉ chứa entity thuần C# và interface. Không reference bất kỳ NuGet package infrastructure nào.



\*\*src/Infrastructure/\*\*

Implement các dịch vụ kỹ thuật: ghi MongoDB, publish/consume RabbitMQ. Hỗ trợ cơ chế No-Op để tắt từng dịch vụ qua cấu hình mà không cần sửa code.



\*\*src/Persistence/\*\*

Tầng truy cập database SQL Server thông qua EF Core. `MenuNewsDbContext` được cấu hình thủ công (DB First approach) khớp với schema đã tạo bằng SQL script.



\*\*tests/\*\*

Chứa unit test cho các Validator. Sử dụng xUnit và kiểm tra các rule của FluentValidation.



\---



\## 4. Công nghệ sử dụng



| Công nghệ | Vai trò |

|-----------|---------|

| ASP.NET Core 8 | Framework Web API chính, xử lý HTTP request/response |

| Clean Architecture | Tổ chức codebase theo layer, đảm bảo separation of concerns |

| CQRS | Tách biệt Command (ghi) và Query (đọc) để dễ mở rộng và test |

| MediatR 12 | Dispatcher cho Command/Query, kết nối Controller với Handler qua Pipeline |

| Entity Framework Core 8 | ORM truy cập SQL Server theo cách DB First (scaffold từ database) |

| SQL Server 2022 | Database quan hệ chính lưu Menu, News, quan hệ nhiều-nhiều |

| MongoDB 7 | Lưu audit log dạng document NoSQL, không ảnh hưởng luồng chính |

| RabbitMQ 3 | Message broker bất đồng bộ, publish event khi News được tạo |

| gRPC (Grpc.AspNetCore) | Giao thức RPC hiệu năng cao, expose endpoint truy vấn News |

| Protocol Buffers | Định nghĩa contract gRPC qua file `.proto` |

| FluentValidation | Validate dữ liệu đầu vào theo rule, tích hợp vào MediatR Pipeline |

| AutoMapper | Mapping tự động giữa Entity và DTO |

| Swagger (Swashbuckle) | Giao diện web test REST API, tự động generate từ Controller |

| Docker Compose | Khởi động SQL Server + MongoDB + RabbitMQ bằng một lệnh duy nhất |



\---



\## 5. Yêu cầu hệ thống



Trước khi bắt đầu, hãy đảm bảo máy tính đã cài đặt các công cụ sau:



| Công cụ | Phiên bản tối thiểu | Mục đích |

|---------|---------------------|---------|

| .NET SDK | 8.0 | Build và chạy project |

| Visual Studio 2022 | 17.8 trở lên | IDE phát triển (hoặc VS Code + C# Extension) |

| Docker Desktop | 4.x | Chạy SQL Server, MongoDB, RabbitMQ qua container |

| Git | 2.x | Clone source code |

| Postman | Bất kỳ | Test REST API và gRPC |



> Lưu ý: Docker Desktop là cách nhanh nhất để có đủ SQL Server + MongoDB + RabbitMQ mà không cần cài thủ công từng dịch vụ. Nếu đã có SQL Server cài trực tiếp trên máy, vẫn cần Docker cho MongoDB và RabbitMQ, hoặc cài riêng theo hướng dẫn ở mục 6.



\---



\## 6. Hướng dẫn cài đặt môi trường



\### Bước 1: Cài .NET SDK 8



Truy cập trang chính thức và tải .NET 8 SDK về máy:



```

https://dotnet.microsoft.com/download/dotnet/8.0

```



Sau khi cài xong, kiểm tra phiên bản:



```bash

dotnet --version

```



Kết quả phải là `8.0.x`.



\### Bước 2: Cài Docker Desktop



Truy cập trang chính thức của Docker:



```

https://www.docker.com/products/docker-desktop

```



Tải và cài Docker Desktop cho Windows hoặc macOS. Sau khi cài xong, mở Docker Desktop và chờ cho đến khi trạng thái chuyển sang "Running".



Kiểm tra Docker hoạt động:



```bash

docker version

docker compose version

```



\### Bước 3: Khởi động SQL Server, MongoDB, RabbitMQ bằng Docker Compose



Đây là cách nhanh nhất. Toàn bộ infrastructure sẽ được khởi động chỉ với một lệnh.



Từ thư mục gốc của dự án:



```bash

docker compose up -d

```



Hoặc chạy script PowerShell đã chuẩn bị sẵn:



```powershell

.\\scripts\\start-infrastructure.ps1

```



Sau khi chạy, các dịch vụ sẽ hoạt động tại:



\- SQL Server: `localhost,1433` — tài khoản `sa` / mật khẩu `YourStrong@Passw0rd`

\- MongoDB: `localhost:27017` — không cần xác thực (mặc định)

\- RabbitMQ AMQP: `localhost:5672` — tài khoản `guest` / mật khẩu `guest`

\- RabbitMQ Management UI: `http://localhost:15672` — đăng nhập bằng `guest` / `guest`



Kiểm tra các container đang chạy:



```bash

docker ps

```



Ba container `menunews-sqlserver`, `menunews-mongodb`, `menunews-rabbitmq` phải ở trạng thái `Up`.



\### Bước 4 (Tùy chọn): Cài SQL Server thủ công



Nếu không dùng Docker, có thể tải SQL Server 2022 Developer Edition miễn phí tại:



```

https://www.microsoft.com/en-us/sql-server/sql-server-downloads

```



Cài SQL Server Management Studio (SSMS) để kết nối và chạy script:



```

https://aka.ms/ssmsfullsetup

```



\### Bước 5 (Tùy chọn): Cài MongoDB thủ công



Tải MongoDB Community Server tại:



```

https://www.mongodb.com/try/download/community

```



Sau khi cài, MongoDB mặc định chạy tại `localhost:27017`.



\### Bước 6 (Tùy chọn): Cài RabbitMQ thủ công



RabbitMQ yêu cầu Erlang/OTP được cài trước. Tải tại:



```

https://www.rabbitmq.com/download.html

```



Bật Management Plugin để có giao diện web quản lý:



```bash

rabbitmq-plugins enable rabbitmq\_management

```



\---



\## 7. Hướng dẫn clone source code



\### Bước 1: Clone repository



```bash

git clone https://github.com/your-org/MenuNewsManagement.git

```



Thay `your-org` bằng tên tổ chức hoặc tài khoản GitHub thực tế của bạn.



\### Bước 2: Di chuyển vào thư mục dự án



```bash

cd MenuNewsManagement

```



\### Bước 3: Khôi phục NuGet packages



```bash

dotnet restore

```



Lệnh này tải về toàn bộ các NuGet package cần thiết được khai báo trong các file `.csproj`.



\### Bước 4: Kiểm tra cấu trúc project



```bash

dotnet sln list

```



Kết quả liệt kê năm project: `MenuNews.Api`, `MenuNews.Application`, `MenuNews.Domain`, `MenuNews.Infrastructure`, `MenuNews.Persistence`.



\---



\## 8. Hướng dẫn database



\### Bước 1: Đảm bảo SQL Server đang chạy



Nếu dùng Docker Compose (khuyến nghị):



```bash

docker compose up -d sqlserver

```



Chờ khoảng 30 giây để SQL Server khởi động hoàn toàn. Có thể theo dõi log:



```bash

docker logs menunews-sqlserver

```



Khi thấy dòng `SQL Server is now ready for client connections`, SQL Server đã sẵn sàng.



\### Bước 2: Chạy script khởi tạo database



Mở SSMS hoặc dùng sqlcmd để kết nối vào `localhost,1433` với tài khoản `sa` và mật khẩu `YourStrong@Passw0rd`. Mở file `database/01\_CreateDatabase.sql` và thực thi toàn bộ script.



Nếu dùng sqlcmd từ terminal:



```bash

sqlcmd -S localhost,1433 -U sa -P "YourStrong@Passw0rd" -C -i database/01\_CreateDatabase.sql

```



Script này sẽ thực hiện tuần tự:



1\. Tạo database `MenuNewsDb` nếu chưa tồn tại.

2\. Tạo bảng `Menus` với các cột `Id`, `Name`, `Description`, `CreatedDate`, `IsDeleted`, `DeletedAt`.

3\. Tạo bảng `News` với các cột `Id`, `Title`, `Content`, `Author`, `CreatedDate`, `IsDeleted`, `DeletedAt`.

4\. Tạo bảng trung gian `MenuNews` với khóa chính composite `(MenuId, NewsId)`, khóa ngoại CASCADE DELETE, và index trên `NewsId`.

5\. Chèn 2 bản ghi mẫu vào `Menus`, 2 bản ghi vào `News` và 3 bản ghi vào `MenuNews` để demo.



Sau khi chạy xong, bạn sẽ thấy thông báo:



```

Database MenuNewsDb created successfully.

```



\### Bước 3: EF Core DB First — Scaffold DbContext



Dự án sử dụng \*\*DB First approach\*\*: entity và DbContext được tạo dựa trên database đã có sẵn. Trong dự án này, `MenuNewsDbContext` và các entity đã được scaffold và tinh chỉnh tay. Nếu bạn muốn scaffold lại từ đầu (ví dụ khi schema thay đổi), sử dụng lệnh sau.



Từ thư mục gốc của solution, mở Package Manager Console trong Visual Studio hoặc dùng dotnet CLI:



```bash

dotnet ef dbcontext scaffold \\

&#x20; "Server=localhost,1433;Database=MenuNewsDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;" \\

&#x20; Microsoft.EntityFrameworkCore.SqlServer \\

&#x20; --output-dir Context \\

&#x20; --context MenuNewsDbContext \\

&#x20; --context-dir Context \\

&#x20; --project src/Persistence \\

&#x20; --startup-project src/Api \\

&#x20; --no-onconfiguring \\

&#x20; --force

```



Giải thích từng tham số:



\- Connection string sau lệnh `scaffold` — chuỗi kết nối đến SQL Server đang chạy.

\- `Microsoft.EntityFrameworkCore.SqlServer` — provider cho SQL Server.

\- `--output-dir Context` — thư mục đầu ra chứa entity được scaffold (tính theo project Persistence).

\- `--context MenuNewsDbContext` — tên class DbContext sẽ được tạo ra.

\- `--context-dir Context` — thư mục chứa DbContext class.

\- `--project src/Persistence` — project đích để scaffold vào.

\- `--startup-project src/Api` — project startup để đọc cấu hình connection string.

\- `--no-onconfiguring` — bỏ qua việc tạo `OnConfiguring()` trong DbContext (vì connection string đã được inject qua DI).

\- `--force` — ghi đè lên file đã có nếu chạy lại scaffold.



> Sau khi scaffold, cần kiểm tra lại và điều chỉnh thủ công các `HasQueryFilter` cho soft delete, vì tool scaffold không tự thêm filter này.



\---



\## 9. Cấu hình appsettings.json



File `src/Api/appsettings.json` là cấu hình mặc định khi chạy ở môi trường `Production`. File `appsettings.Development.json` ghi đè khi chạy ở môi trường `Development`.



Nội dung `appsettings.json` hoàn chỉnh:



```json

{

&#x20; "Logging": {

&#x20;   "LogLevel": {

&#x20;     "Default": "Information",

&#x20;     "Microsoft.AspNetCore": "Warning"

&#x20;   }

&#x20; },

&#x20; "AllowedHosts": "\*",

&#x20; "ConnectionStrings": {

&#x20;   "DefaultConnection": "Server=localhost,1433;Database=MenuNewsDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;"

&#x20; },

&#x20; "MongoDb": {

&#x20;   "Enabled": true,

&#x20;   "ConnectionString": "mongodb://localhost:27017",

&#x20;   "DatabaseName": "MenuNewsAudit",

&#x20;   "AuditCollectionName": "audit\_logs"

&#x20; },

&#x20; "RabbitMq": {

&#x20;   "Enabled": true,

&#x20;   "HostName": "localhost",

&#x20;   "Port": 5672,

&#x20;   "UserName": "guest",

&#x20;   "Password": "guest",

&#x20;   "QueueName": "news\_created\_queue"

&#x20; }

}

```



Khi chạy với Docker Compose (profile Docker), dùng file `appsettings.Docker.json` — các hostname được thay bằng tên service Docker: `sqlserver` thay cho `localhost`, `mongodb` thay cho `localhost`, `rabbitmq` thay cho `localhost`.



\### Giải thích từng section



\*\*ConnectionStrings.DefaultConnection\*\*

Chuỗi kết nối đến SQL Server. Thay `YourStrong@Passw0rd` bằng mật khẩu thực tế nếu không dùng Docker Compose mặc định. `TrustServerCertificate=True` bắt buộc khi kết nối đến SQL Server trong container Docker vì certificate tự ký.



\*\*MongoDb.Enabled\*\*

Đặt thành `false` để tắt hoàn toàn MongoDB. Hệ thống sẽ tự động dùng `NoOpAuditLogService` — audit log sẽ không được ghi nhưng ứng dụng vẫn chạy bình thường. Hữu ích khi chạy trên máy không có MongoDB.



\*\*MongoDb.DatabaseName\*\*

Tên database trong MongoDB. Database này sẽ được tự động tạo khi có document đầu tiên được insert — không cần tạo thủ công.



\*\*MongoDb.AuditCollectionName\*\*

Tên collection lưu audit log. Mặc định là `audit\_logs`.



\*\*RabbitMq.Enabled\*\*

Đặt thành `false` để tắt RabbitMQ. Hệ thống dùng `NoOpNewsEventPublisher` — event không được publish nhưng ứng dụng vẫn chạy bình thường.



\*\*RabbitMq.QueueName\*\*

Tên queue dùng để truyền message. Producer và Consumer đều phải dùng cùng tên queue này.



\---



\## 10. Hướng dẫn chạy project



\### Bước 1: Đảm bảo infrastructure đang chạy



```bash

docker compose up -d

```



Kiểm tra trạng thái:



```bash

docker ps

```



Ba container phải ở trạng thái `Up (healthy)` hoặc `Up`.



\### Bước 2: Di chuyển vào thư mục Api



```bash

cd src/Api

```



\### Bước 3: Khôi phục dependencies



```bash

dotnet restore

```



Lệnh này đọc file `MenuNews.Api.csproj` và tải về tất cả NuGet package còn thiếu từ nuget.org.



\### Bước 4: Build project



```bash

dotnet build

```



Lệnh này compile toàn bộ source code. Nếu có lỗi compile, thông báo lỗi sẽ hiển thị tại đây. Lỗi thường gặp bao gồm thiếu package hoặc sai namespace.



\### Bước 5: Chạy project



```bash

dotnet run

```



Hoặc để chỉ định launch profile cụ thể (khuyến nghị):



```bash

dotnet run --launch-profile http

```



Sau khi chạy thành công, terminal sẽ hiển thị:



```

info: Microsoft.Hosting.Lifetime\[14]

&#x20;     Now listening on: http://localhost:5000

info: Microsoft.Hosting.Lifetime\[0]

&#x20;     Application started. Press Ctrl+C to shut down.

info: MenuNews.Infrastructure.RabbitMq.NewsCreatedConsumerHostedService\[0]

&#x20;     RabbitMQ Consumer đang lắng nghe queue: news\_created\_queue

```



Truy cập Swagger UI tại: `http://localhost:5000/swagger`



\### Chạy từ Visual Studio 2022



Mở file `MenuNewsManagement.slnx`, chọn project `MenuNews.Api` làm Startup Project, nhấn `F5` để chạy ở chế độ Debug hoặc `Ctrl+F5` không có debugger.



\---



\## 11. Hướng dẫn test API



\### Swagger UI



Sau khi chạy project ở môi trường Development, truy cập:



```

http://localhost:5000/swagger

```



Swagger UI liệt kê toàn bộ endpoint, cho phép thực thi trực tiếp từ trình duyệt.



\### Các nhóm API chính



\#### News API



| Method | Endpoint | Mô tả |

|--------|----------|-------|

| POST | /api/news | Tạo mới News |

| GET | /api/news | Lấy danh sách News (có phân trang) |

| GET | /api/news/{id} | Lấy News theo ID |

| PUT | /api/news/{id} | Cập nhật News |

| DELETE | /api/news/{id} | Xóa News (soft delete) |



\#### Menus API



| Method | Endpoint | Mô tả |

|--------|----------|-------|

| POST | /api/menus | Tạo mới Menu |

| GET | /api/menus | Lấy danh sách Menu (có phân trang) |

| GET | /api/menus/{id} | Lấy Menu theo ID |

| PUT | /api/menus/{id} | Cập nhật Menu |

| DELETE | /api/menus/{id} | Xóa Menu (soft delete) |



\#### MenuNews API



| Method | Endpoint | Mô tả |

|--------|----------|-------|

| POST | /api/menunews/assign | Gán News vào Menu |

| GET | /api/menunews/by-menu/{menuId} | Lấy danh sách News theo MenuId |

| GET | /api/menunews/by-news/{newsId} | Lấy danh sách Menu theo NewsId |



\### Ví dụ request — Tạo News mới



```bash

curl -X POST http://localhost:5000/api/news \\

&#x20; -H "Content-Type: application/json" \\

&#x20; -d '{

&#x20;   "title": "ASP.NET Core 8 và Clean Architecture",

&#x20;   "content": "Bài viết này giải thích cách áp dụng Clean Architecture trong dự án ASP.NET Core 8 thực tế, bao gồm CQRS, MediatR và các pattern liên quan.",

&#x20;   "author": "admin"

&#x20; }'

```



Kết quả trả về:



```json

{

&#x20; "success": true,

&#x20; "message": "Tạo News thành công - message đã gửi RabbitMQ.",

&#x20; "data": {

&#x20;   "id": 3,

&#x20;   "title": "ASP.NET Core 8 và Clean Architecture",

&#x20;   "content": "Bài viết này giải thích cách áp dụng Clean Architecture...",

&#x20;   "author": "admin",

&#x20;   "createdDate": "2025-01-15T08:30:00Z"

&#x20; }

}

```



\### Ví dụ request — Lấy danh sách News có phân trang



```bash

curl "http://localhost:5000/api/news?pageNumber=1\&pageSize=10"

```



\### Ví dụ request — Gán News vào Menu



```bash

curl -X POST http://localhost:5000/api/menunews/assign \\

&#x20; -H "Content-Type: application/json" \\

&#x20; -d '{

&#x20;   "menuId": 1,

&#x20;   "newsId": 3

&#x20; }'

```



\### Kiểm tra validation — Request thiếu dữ liệu



```bash

curl -X POST http://localhost:5000/api/news \\

&#x20; -H "Content-Type: application/json" \\

&#x20; -d '{ "title": "Hi", "content": "Short", "author": "" }'

```



Kết quả trả về HTTP 400:



```json

{

&#x20; "success": false,

&#x20; "message": "Validation failed.",

&#x20; "errors": \[

&#x20;   "Title phải có tối thiểu 5 ký tự.",

&#x20;   "Content phải có tối thiểu 20 ký tự.",

&#x20;   "Author không được để trống."

&#x20; ]

}

```



\---



\## 12. Hướng dẫn RabbitMQ



\### Khái niệm cơ bản



\*\*Producer (nhà xuất bản)\*\*: là thành phần gửi message vào queue. Trong dự án này, `RabbitMqNewsEventPublisher` là producer — nó được gọi từ `CreateNewsCommandHandler` ngay sau khi News được lưu vào SQL Server.



\*\*Queue (hàng đợi)\*\*: là nơi message được lưu tạm thời cho đến khi Consumer xử lý. Queue trong dự án có tên `news\_created\_queue`, được cấu hình là `durable: true` — nghĩa là message không bị mất khi RabbitMQ restart.



\*\*Consumer (người tiêu thụ)\*\*: là thành phần nhận và xử lý message từ queue. Trong dự án, `NewsCreatedConsumerHostedService` là Consumer — nó chạy ngầm như một `BackgroundService`, liên tục lắng nghe queue. Khi nhận được message, nó in ra console và ghi log.



\*\*Message\*\*: là dữ liệu được publish lên queue. Trong dự án, message có dạng JSON chứa `NewsId` và `Title` của News vừa được tạo.



\### Luồng RabbitMQ khi tạo News



```

CreateNewsCommandHandler

&#x20;   |

&#x20;   |-- \[1] Lưu News vào SQL Server

&#x20;   |

&#x20;   |-- \[2] Gọi INewsEventPublisher.PublishNewsCreatedAsync(newsId, title)

&#x20;               |

&#x20;               v

&#x20;       RabbitMqNewsEventPublisher

&#x20;               |

&#x20;               |-- Kết nối đến RabbitMQ (localhost:5672)

&#x20;               |-- Declare queue "news\_created\_queue" nếu chưa có

&#x20;               |-- Publish JSON message: { "NewsId": 3, "Title": "..." }

&#x20;               |

&#x20;               v

&#x20;       RabbitMQ Broker (Queue: news\_created\_queue)

&#x20;               |

&#x20;               v

&#x20;       NewsCreatedConsumerHostedService (BackgroundService)

&#x20;               |

&#x20;               |-- Deserialize JSON message

&#x20;               |-- Log: "News Created Successfully - Id: 3, Title: ..."

&#x20;               |-- BasicAck: xác nhận đã xử lý xong, message bị xóa khỏi queue

```



\### Kiểm tra RabbitMQ Management UI



Sau khi tạo một News mới qua API, mở trình duyệt và truy cập:



```

http://localhost:15672

```



Đăng nhập bằng `guest` / `guest`. Vào tab \*\*Queues\*\* và tìm queue `news\_created\_queue`. Tại đây bạn có thể thấy số lượng message đã được publish, message đang chờ xử lý và thống kê throughput.



\### Tắt RabbitMQ khi chạy không có Docker



Nếu không muốn dùng RabbitMQ, đặt trong `appsettings.json`:



```json

"RabbitMq": {

&#x20; "Enabled": false

}

```



Hệ thống sẽ tự động dùng `NoOpNewsEventPublisher` — việc tạo News vẫn hoạt động bình thường, chỉ không publish event.



\---



\## 13. Hướng dẫn MongoDB



\### Vai trò của MongoDB trong dự án



MongoDB được dùng để lưu \*\*audit log\*\* — ghi lại lịch sử các thao tác quan trọng trong hệ thống. Audit log có đặc điểm là dữ liệu metadata đi kèm mỗi action có thể khác nhau (tạo News có `Id` và `Title`, gán Menu có `MenuId` và `NewsId`), nên MongoDB với kiểu document linh hoạt rất phù hợp.



Audit log được ghi độc lập với luồng chính — nếu MongoDB gặp lỗi, ứng dụng vẫn tiếp tục hoạt động (lỗi được log nhưng không ném exception lên user).



\### Database và Collection



\- \*\*Database\*\*: `MenuNewsAudit`

\- \*\*Collection\*\*: `audit\_logs`

\- \*\*Document model\*\*: `AuditLogDocument`



```json

{

&#x20; "\_id": "ObjectId(...)",

&#x20; "action": "CREATE\_NEWS",

&#x20; "user": "system",

&#x20; "metadata": {

&#x20;   "Id": 3,

&#x20;   "Title": "ASP.NET Core 8 và Clean Architecture"

&#x20; },

&#x20; "createdAt": "2025-01-15T08:30:00.000Z"

}

```



\### Khi nào dữ liệu được ghi



Hiện tại, audit log được ghi trong `CreateNewsCommandHandler` sau khi News được lưu thành công vào SQL Server. Dữ liệu ghi bao gồm action (`CREATE\_NEWS`), user thực hiện và metadata gồm `Id` và `Title` của News vừa tạo.



\### Xem dữ liệu trong MongoDB



Dùng MongoDB Compass (công cụ GUI miễn phí của MongoDB) để kết nối:



```

mongodb://localhost:27017

```



Hoặc dùng mongo shell:



```bash

docker exec -it menunews-mongodb mongosh

use MenuNewsAudit

db.audit\_logs.find().pretty()

```



Kết quả sẽ liệt kê tất cả document audit log đã được ghi.



\### Tắt MongoDB khi không cần



Đặt trong `appsettings.json`:



```json

"MongoDb": {

&#x20; "Enabled": false

}

```



Hệ thống dùng `NoOpAuditLogService` — log không được ghi nhưng ứng dụng vẫn chạy đầy đủ.



\---



\## 14. Hướng dẫn gRPC



\### Tổng quan



Dự án expose một gRPC service `NewsGrpc` cho phép truy vấn News với hiệu năng cao hơn REST. gRPC sử dụng Protocol Buffers (protobuf) để serialize dữ liệu — nhỏ gọn và nhanh hơn JSON đáng kể.



\### Proto file



File `src/Api/Protos/news.proto` định nghĩa contract của gRPC service:



```protobuf

syntax = "proto3";



option csharp\_namespace = "MenuNews.Api.Grpc";



package news;



service NewsGrpc {

&#x20; rpc GetNewsById (GetNewsByIdRequest) returns (NewsGrpcResponse);

&#x20; rpc GetAllNews (GetAllNewsRequest) returns (GetAllNewsResponse);

}



message GetNewsByIdRequest {

&#x20; int32 id = 1;

}



message GetAllNewsRequest {

&#x20; int32 page\_number = 1;

&#x20; int32 page\_size = 2;

}



message NewsGrpcResponse {

&#x20; int32 id = 1;

&#x20; string title = 2;

&#x20; string content = 3;

&#x20; string author = 4;

&#x20; string created\_date = 5;

}



message GetAllNewsResponse {

&#x20; repeated NewsGrpcResponse items = 1;

&#x20; int32 total\_count = 2;

}

```



`repeated` trong protobuf tương đương với `List<T>` trong C#. File `.proto` này được compile tự động bởi MSBuild thành C# class khi build project.



\### gRPC Service implementation



`NewsGrpcService` kế thừa từ `NewsGrpc.NewsGrpcBase` (được tạo tự động từ proto) và override hai method `GetNewsById` và `GetAllNews`. Bên trong mỗi method, nó gọi `IMediator.Send()` giống hệt REST Controller — tái sử dụng hoàn toàn Application layer.



\### Port và protocol



gRPC chạy trên cùng port với HTTP API nhưng dùng HTTP/2. Trong môi trường Development, port thường là `5000` (HTTP) hoặc `5001` (HTTPS). Kiểm tra `launchSettings.json` để biết port chính xác.



\### Test gRPC bằng Postman



Postman hỗ trợ gRPC từ phiên bản 9.7 trở lên.



Bước 1: Tạo request mới, chọn loại \*\*gRPC\*\*.



Bước 2: Nhập URL:



```

http://localhost:5000

```



Bước 3: Postman tự động discover service thông qua gRPC Reflection (đã được bật trong Development mode). Chọn service `news.NewsGrpc` và method `GetNewsById`.



Bước 4: Nhập message body:



```json

{

&#x20; "id": 1

}

```



Bước 5: Nhấn \*\*Invoke\*\*. Kết quả trả về là protobuf được deserialize sang JSON.



\### Test gRPC bằng grpcurl



`grpcurl` là công cụ command-line để gọi gRPC, tương tự curl cho REST.



Cài đặt:



```bash

\# macOS

brew install grpcurl



\# Windows (dùng Scoop)

scoop install grpcurl

```



Liệt kê các service có sẵn (nhờ gRPC Reflection):



```bash

grpcurl -plaintext localhost:5000 list

```



Liệt kê các method của service NewsGrpc:



```bash

grpcurl -plaintext localhost:5000 list news.NewsGrpc

```



Gọi GetNewsById:



```bash

grpcurl -plaintext -d '{"id": 1}' localhost:5000 news.NewsGrpc/GetNewsById

```



Gọi GetAllNews với phân trang:



```bash

grpcurl -plaintext -d '{"page\_number": 1, "page\_size": 5}' localhost:5000 news.NewsGrpc/GetAllNews

```



\---



\## 15. Flow hệ thống



\### Flow REST API — Tạo News



```

Client (Postman / Browser)

&#x20;   |

&#x20;   | HTTP POST /api/news

&#x20;   v

ExceptionHandlingMiddleware

&#x20;   | (bắt exception nếu có, trả về response lỗi chuẩn)

&#x20;   v

NewsController.Create()

&#x20;   | tạo CreateNewsCommand và gọi \_mediator.Send()

&#x20;   v

MediatR Pipeline

&#x20;   | \[1] ValidationBehavior

&#x20;   |      |-- gọi CreateNewsCommandValidator

&#x20;   |      |-- nếu lỗi: ném ValidationException -> Middleware bắt -> HTTP 400

&#x20;   |      |-- nếu hợp lệ: tiếp tục

&#x20;   v

CreateNewsCommandHandler.Handle()

&#x20;   | \[2] INewsRepository.AddAsync(newsItem)

&#x20;   |      |-- Persistence: EF Core INSERT INTO News

&#x20;   |      |-- SQL Server lưu bản ghi

&#x20;   |

&#x20;   | \[3] INewsEventPublisher.PublishNewsCreatedAsync(newsId, title)

&#x20;   |      |-- Infrastructure: RabbitMqNewsEventPublisher

&#x20;   |      |-- publish JSON message lên queue "news\_created\_queue"

&#x20;   |

&#x20;   | \[4] IAuditLogService.LogAsync("CREATE\_NEWS", user, metadata)

&#x20;   |      |-- Infrastructure: MongoAuditLogService

&#x20;   |      |-- INSERT document vào MongoDB collection "audit\_logs"

&#x20;   |

&#x20;   | \[5] trả về NewsDto

&#x20;   v

NewsController

&#x20;   | đóng gói: ApiResponse<NewsDto>.Ok(result, message)

&#x20;   v

Client nhận HTTP 200 + JSON response

```



\### Flow gRPC — Truy vấn News



```

gRPC Client (Postman / grpcurl)

&#x20;   |

&#x20;   | gRPC Call: NewsGrpc/GetNewsById { id: 1 }

&#x20;   v

NewsGrpcService.GetNewsById()

&#x20;   | gọi \_mediator.Send(new GetNewsByIdQuery(1))

&#x20;   v

GetNewsByIdQueryHandler.Handle()

&#x20;   | gọi INewsRepository.GetByIdAsync(1)

&#x20;   v

NewsRepository

&#x20;   | EF Core: SELECT \* FROM News WHERE Id = 1 AND IsDeleted = 0

&#x20;   v

SQL Server trả về bản ghi

&#x20;   v

Handler trả về NewsDto

&#x20;   v

NewsGrpcService map sang NewsGrpcResponse (protobuf)

&#x20;   v

gRPC Client nhận response

```



\### Flow RabbitMQ — Publish và Consume



```

\[Producer]

CreateNewsCommandHandler

&#x20;   |

&#x20;   | PublishNewsCreatedAsync(newsId, title)

&#x20;   v

RabbitMqNewsEventPublisher

&#x20;   |

&#x20;   | BasicPublish -> exchange: "" -> routingKey: "news\_created\_queue"

&#x20;   v

RabbitMQ Broker

&#x20;   |

&#x20;   | message lưu trong queue "news\_created\_queue"

&#x20;   v

\[Consumer - Background Service]

NewsCreatedConsumerHostedService

&#x20;   |

&#x20;   | Nhận message qua AsyncEventingBasicConsumer

&#x20;   | Deserialize JSON -> NewsCreatedMessage { NewsId, Title }

&#x20;   | Console.WriteLine + ILogger.LogInformation

&#x20;   | BasicAck: xác nhận xử lý thành công

&#x20;   v

Message bị xóa khỏi queue

```



\### Flow Audit Log — MongoDB



```

CreateNewsCommandHandler (hoặc bất kỳ Handler nào cần log)

&#x20;   |

&#x20;   | IAuditLogService.LogAsync("CREATE\_NEWS", "system", { Id, Title })

&#x20;   v

MongoAuditLogService.LogAsync()

&#x20;   |

&#x20;   | Tạo AuditLogDocument { Action, User, Metadata, CreatedAt }

&#x20;   | IMongoCollection.InsertOneAsync(document)

&#x20;   v

MongoDB: database "MenuNewsAudit", collection "audit\_logs"

&#x20;   |

&#x20;   | Document được lưu với \_id dạng ObjectId

&#x20;   v

Log hoàn tất (bất đồng bộ, không block request chính)

```



\---



\## 16. Roadmap / Future Improvements



Dưới đây là danh sách các tính năng và cải tiến kỹ thuật có thể thêm vào dự án trong tương lai, theo thứ tự ưu tiên từ cao đến thấp.



\### JWT Authentication \& Authorization



Hiện tại API chưa có xác thực. Bước tiếp theo là tích hợp JWT Bearer Token: thêm endpoint `POST /api/auth/login`, generate JWT, bảo vệ các endpoint với `\[Authorize]`. `CurrentUserService` đã được chuẩn bị sẵn để đọc user từ `HttpContext.User.Identity.Name`.



\### Redis Distributed Cache



Thêm Redis để cache kết quả của các Query thường xuyên được truy vấn như `GetAllMenus` và `GetAllNews`. Implement `ICacheService` tại Infrastructure layer, inject vào Query Handler và đặt TTL phù hợp. Khi dữ liệu thay đổi (Command thành công), invalidate cache liên quan.



\### Docker hóa toàn bộ ứng dụng



Thêm `Dockerfile` cho project `MenuNews.Api` và bổ sung service `api` vào `docker-compose.yml`. Khi đó toàn bộ hệ thống gồm API + SQL Server + MongoDB + RabbitMQ có thể chạy hoàn toàn qua Docker Compose, không cần cài .NET SDK trên máy host.



\### Unit Test và Integration Test đầy đủ



Bổ sung test cho tất cả Handler (dùng `Moq` để mock repository và service), test cho Controller (dùng `WebApplicationFactory`), test tích hợp cho toàn bộ luồng từ HTTP request đến database. Mục tiêu đạt coverage trên 80%.



\### CI/CD Pipeline



Cấu hình GitHub Actions để tự động build, chạy test và deploy khi có code mới được push lên branch `main`. Pipeline bao gồm: restore, build, test, publish, và deploy lên môi trường staging.



\### Serilog và Structured Logging



Thay thế logging mặc định của .NET bằng Serilog với sink ghi ra file (rolling) và console. Cấu hình structured logging để dễ query log sau này. Tích hợp Serilog Enrichers để tự động thêm `CorrelationId`, `MachineName`, `Environment` vào mỗi log entry.



\### Health Checks



Thêm endpoint `GET /health` và `GET /health/ready` sử dụng `Microsoft.Extensions.Diagnostics.HealthChecks`. Check trạng thái kết nối SQL Server, MongoDB và RabbitMQ. Tích hợp với Docker Compose `healthcheck` để tự động restart khi service không healthy.



\### Pagination chuẩn hóa và Filtering



Mở rộng `PaginatedList<T>` với metadata đầy đủ hơn (totalPages, hasPreviousPage, hasNextPage). Thêm tính năng lọc và sắp xếp vào các Query: lọc News theo Author, theo khoảng thời gian; lọc Menu theo tên.



\### Outbox Pattern cho RabbitMQ



Hiện tại message RabbitMQ được publish trực tiếp trong Handler — nếu RabbitMQ tạm thời down, message sẽ bị mất. Implement Transactional Outbox Pattern: lưu message vào bảng `OutboxMessages` trong SQL Server cùng transaction với dữ liệu, sau đó một background service đọc Outbox và publish lên RabbitMQ. Đảm bảo at-least-once delivery.



