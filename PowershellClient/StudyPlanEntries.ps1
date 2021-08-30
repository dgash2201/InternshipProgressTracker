$Script:BaseUrl = "https://localhost:44312/StudyPlanEntries"
$ContentType = "application/json"

function Get-StudyPlanEntries {
    [CmdletBinding()]
    param (
    )
    
    begin {
        $uri = $Script:BaseUrl
        $headers = @{
            Authorization = "Bearer $($Global:Context.Token)"
        }
    }
    
    process {
        $parameters = @{
            Uri = $uri
            Method = "GET"
            Header = $headers
        }

        try {
            Invoke-RestMethod @parameters
        }
        catch {
            Write-Host $Error[0] | ConvertTo-Json
        }
    }
}

function Get-StudyPlanEntry {
    param (
        [Parameter(Mandatory)]
        [Int]
        $Id
    )
    begin {
        $headers = @{
            Authorization = "Bearer $($Global:Context.Token)"
        }
    }
    process {
        $parameters = @{
            Uri = "$Script:BaseUrl/$Id"
            Method = "GET"
            Headers = $headers
        }

        try {
            Invoke-RestMethod @parameters
        }
        catch {
            Write-Host $Error[0] | ConvertTo-Json
        }
    }
}

function New-StudyPlanEntry {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)]
        [String]
        $Title,

        [Parameter(Mandatory = $false)]
        [String]
        $Description,

        [Parameter(Mandatory)]
        [String]
        $Duration,

        [Parameter(Mandatory)]
        [Int]
        $InternshipStreamId
    )
    begin {
        $uri = "$Script:BaseUrl"
    }
    process {
        $headers = @{
            Authorization = "Bearer $($Global:Context.Token)"
        }

        $body = ConvertTo-Json @{
            Title = $Title
            Description = $Description
            Duration = $Duration
            InternshipStreamId = $InternshipStreamId
        }

        $parameters = @{
            Uri = $uri
            Method = "POST"
            Headers = $headers
            Body = $body
            ContentType = $ContentType
        }

        try {
            Invoke-RestMethod @parameters
        }
        catch {
            Write-Host $Error[0] | ConvertTo-Json
        }
    }
}

function Set-StudyPlanEntry {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)]
        [Int]
        $Id,

        [Parameter(Mandatory)]
        [String]
        $Title,

        [Parameter(Mandatory = $false)]
        [String]
        $Description,

        [Parameter(Mandatory)]
        [String]
        $Duration,

        [Parameter(Mandatory)]
        [Int]
        $InternshipStreamId
    )
    begin {
        $headers = @{
            Authorization = "Bearer $($Global:Context.Token)"
        }
    }
    process {
        $uri = "$Script:BaseUrl/$Id"

        $body = ConvertTo-Json @{
            Title = $Title
            Description = $Description
            Duration = $Duration
            InternshipStreamId = $InternshipStreamId
        }

        $parameters = @{
            Uri = $uri
            Method = "PUT"
            Headers = $headers
            Body = $body
            ContentType = $ContentType
        }

        try {
            Invoke-RestMethod @parameters
        }
        catch {
            Write-Host $Error[0] | ConvertTo-Json
        }
    }
}

function Remove-StudyPlanEntry {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)]
        [Int]
        $Id
    )
    begin {
        $headers = @{
            Authorization = "Bearer $($Global:Context.Token)"
        }
    }
    process {
        $parameters = @{
            Uri = "$BaseUrl/$Id"
            Method = "DELETE"
            Headers = $headers
        }

        try {
            Invoke-RestMethod @parameters
        }
        catch {
            Write-Host $Error[0] | ConvertTo-Json
        }
    }
}

function Remove-StudyPlanEntryHard {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)]
        [Int]
        $Id
    )
    begin {
        $headers = @{
            Authorization = "Bearer $($Global:Context.Token)"
        }
    }
    process {
        $parameters = @{
            Uri = "$BaseUrl/$Id"
            Method = "DELETE"
            Headers = $headers
        }

        try {
            Invoke-RestMethod @parameters
        }
        catch {
            Write-Host $Error[0] | ConvertTo-Json
        }
    }
}