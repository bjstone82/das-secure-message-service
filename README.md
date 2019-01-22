[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status/das-secure-message-service?branchName=master)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=1342?branchName=master)

# das-secure-message-service

Inspired by snappass, the secure messaging service is a safe way to send one time contextless messages.

## Deploy
```PowerShell
$ResourceGroupName = ""
$DeploymentParameters = @{
    AppServiceName = ""
    AppServicePlanName = ""
    RedisCacheName = ""
    RedisConnectionString = ""
    CdnUrl = ""
}

New-AzureRmResourceGroupDeployment -Name Deploy01 -ResourceGroupName $ResourceGroupName -TemplateFile .\azure\template.json @DeploymentParameters
```
