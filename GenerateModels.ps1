dotnet tool restore
$OUTPUT_PATH = Join-Path $PSScriptRoot ".\Models\ContentTypes"
$appSettings = Get-Content '.\appsettings.json' | Out-String | ConvertFrom-Json
dotnet tool run KontentModelGenerator -p $appSettings."DeliveryOptions"."ProjectId" -o $OUTPUT_PATH -n "StatiqTutorial"