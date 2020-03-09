FROM mcr.microsoft.com/dotnet/core/sdk:2.1 as builder
ADD NugetDeployValidator /app
WORKDIR /app
RUN dotnet publish -c Release -o build

FROM mcr.microsoft.com/dotnet/core/runtime:2.1
COPY --from=builder /app/build /app
ENTRYPOINT [ "dotnet", "/app/NugetDeployValidator.dll" ]