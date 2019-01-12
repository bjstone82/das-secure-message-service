FROM microsoft/aspnetcore:2.2

# Copy app
WORKDIR /app
COPY src/release .

# Set entry point
ENTRYPOINT ["dotnet", "SFA.DAS.SecureMessageService.Web.dll"]