Import-Module ".\SharedData.psm1"
$Script:StudyPlanEntriesUrl = "$($Script:Context.BaseUrl)/StudyPlanEntries"

function Get-StudyPlanEntries {
    [CmdletBinding()]
    param (
    )
    
    begin {
        $uri = $Script:StudyPlanEntriesUrl
        $headers = @{
            Authorization = "Bearer $($Script:Context.Token)"
        }
    }
    
    process {
        $parameters = @{
            Uri = $uri
            Method = "GET"
            Header = $headers
        }

        try {
            Invoke-RestMethod @parameters | ConvertTo-Json
        }
        catch {
            $_.Exception
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
            Authorization = "Bearer $($Script:Context.Token)"
        }
    }
    process {
        $parameters = @{
            Uri = "$Script:StudyPlanEntriesUrl/$Id"
            Method = "GET"
            Headers = $headers
        }

        try {
            Invoke-RestMethod @parameters | ConvertTo-Json
        }
        catch {
            $_.Exception
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
        $uri = $Script:StudyPlanEntriesUrl
    }
    process {
        $headers = @{
            Authorization = "Bearer $($Script:Context.Token)"
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
            ContentType = $Script:JsonContentType
        }

        try {
            Invoke-RestMethod @parameters | ConvertTo-Json
        }
        catch {
            $_.Exception
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
            Authorization = "Bearer $($Script:Context.Token)"
        }
    }
    process {
        $uri = "$Script:StudyPlanEntriesUrl/$Id"

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
            ContentType = $Script:JsonContentType
        }

        try {
            Invoke-RestMethod @parameters | ConvertTo-Json
        }
        catch {
            $_.Exception
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
            Authorization = "Bearer $($Script:Context.Token)"
        }
    }
    process {
        $parameters = @{
            Uri = "$Script:StudyPlanEntriesUrl/$Id"
            Method = "DELETE"
            Headers = $headers
        }

        try {
            Invoke-RestMethod @parameters
        }
        catch {
            $_.Exception
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
            Authorization = "Bearer $($Script:Context.Token)"
        }
    }
    process {
        $parameters = @{
            Uri = "$Script:StudyPlanEntriesUrl/$Id"
            Method = "DELETE"
            Headers = $headers
        }

        try {
            Invoke-RestMethod @parameters | ConvertTo-Json
        }
        catch {
            $_.Exception
        }
    }
}