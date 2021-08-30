$Script:BaseUrl = "https://localhost:44312/Students"

function Set-Notes {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)]
        [Int]
        $StudyPlanEntryId,

        [Parameter(Mandatory)]
        [String]
        $Notes
    )
    
    begin {
        $uri = "$Script:BaseUrl/add-notes"
        $headers = @{
            Authorization = "Bearer $($Global:Context.Token)"
        }
    }
    
    process {
        $body = ConvertTo-Json @{
            StudyPlanEntryId = $StudyPlanEntryId
            Notes = $Notes
        }

        $parameters = @{
            Uri = $uri
            Method = "PUT"
            Headers = $headers
            Body = $body
        }

        try {
            Invoke-RestMethod @parameters
        }
        catch {
            Write-Host $Error[0] | ConvertTo-Json
        }
    }
}

function Set-Notes {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)]
        [Int]
        $StudyPlanEntryId
    )
    
    begin {
        $uri = "$Script:BaseUrl/start-study-plan-entry"
        $headers = @{
            Authorization = "Bearer $($Global:Context.Token)"
        }
    }
    
    process {
        $body = ConvertTo-Json @{
            StudyPlanEntryId = $StudyPlanEntryId
            Notes = $Notes
        }

        $parameters = @{
            Uri = $uri
            Method = "PUT"
            Headers = $headers
            Body = $body
        }

        try {
            Invoke-RestMethod @parameters
        }
        catch {
            Write-Host $Error[0] | ConvertTo-Json
        }
    }
}

function Set-Notes {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)]
        [Int]
        $StudyPlanEntryId
    )
    
    begin {
        $uri = "$Script:BaseUrl/finish-study-plan-entry"
        $headers = @{
            Authorization = "Bearer $($Global:Context.Token)"
        }
    }
    
    process {
        $body = ConvertTo-Json @{
            StudyPlanEntryId = $StudyPlanEntryId
            Notes = $Notes
        }

        $parameters = @{
            Uri = $uri
            Method = "PUT"
            Headers = $headers
            Body = $body
        }

        try {
            Invoke-RestMethod @parameters
        }
        catch {
            Write-Host $Error[0] | ConvertTo-Json
        }
    }
}