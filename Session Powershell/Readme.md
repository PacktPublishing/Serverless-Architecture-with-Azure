```
$Region = "eastus2"

# Create a new resource groups
New-AzResourceGroup -Name gridResourceGroup -Location $Region

# Register Event Grid Provider
Register-AzResourceProvider -ProviderNamespace Microsoft.EventGrid

# Wait for RegistrationStatus = Registered
$EGStatue = Get-AzResourceProvider -ProviderNamespace Microsoft.EventGrid

# Create a custom topic
$topicname="<your-topic-name>"
New-AzEventGridTopic -ResourceGroupName gridResourceGroup -Location $Region -Name $topicname

#Create a unique Endpoint, https://<your-site-name>.azurewebsites.net
$sitename="<your-site-name>"

New-AzResourceGroupDeployment `
  -ResourceGroupName gridResourceGroup `
  -TemplateUri "https://raw.githubusercontent.com/Azure-Samples/azure-event-grid-viewer/master/azuredeploy.json" `
  -siteName $sitename `
  -hostingPlanName viewerhost

# Subscribe to an Event - add /api/updates
$endpoint = "https://" + $sitename + ".azurewebsites.net/api/updates"

New-AzEventGridSubscription `
  -EventSubscriptionName demoViewerSub `
  -Endpoint $endpoint `
  -ResourceGroupName gridResourceGroup `
  -TopicName $topicname

# Cleanup script
Remove-AzResourceGroup -Name gridResourceGroup
```
