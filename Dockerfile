FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine

# Copy app
WORKDIR /app
COPY src/release ./

# Set entry point
ENTRYPOINT ["dotnet", "SFA.DAS.SecureMessageService.Web.dll"]
