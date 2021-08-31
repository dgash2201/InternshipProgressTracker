Import-Module ".\SharedData.psm1"
$Script:StudyPlansUrl = "$($Script:Context.BaseUrl)/StudyPlans"

function Get-StudyPlans {
    [CmdletBinding()]
    param (
    )
    
    begin {
        $uri = $Script:StudyPlansUrl
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
            Invoke-RestMethod @parameters
        }
        catch {
            Write-Host $Error[0] | ConvertTo-Json
        }
    }
}

function Get-StudyPlan {
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
            Uri = "$Script:StudyPlansUrl/$Id"
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

function New-StudyPlan {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)]
        [String]
        $Title,

        [Parameter(Mandatory = $false)]
        [String]
        $Description,

        [Parameter(Mandatory)]
        [Int]
        $InternshipStreamId
    )
    begin {
        $uri = "$Script:StudyPlansUrl"
    }
    process {
        $headers = @{
            Authorization = "Bearer $($Script:Context.Token)"
        }

        $body = ConvertTo-Json @{
            Title = $Title
            Description = $Description
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
            Invoke-RestMethod @parameters
        }
        catch {
            Write-Host $Error[0] | ConvertTo-Json
        }
    }
}

function Set-StudyPlan {
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
        [Int]
        $InternshipStreamId
    )
    begin {
        $headers = @{
            Authorization = "Bearer $($Script:Context.Token)"
        }
    }
    process {
        $uri = "$Script:StudyPlansUrl/$Id" 

        $body = ConvertTo-Json @{
            Title = $Title
            Description = $Description
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
            Invoke-RestMethod @parameters
        }
        catch {
            Write-Host $Error[0] | ConvertTo-Json
        }
    }
}

function Remove-StudyPlan {
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
            Uri = "$Script:StudyPlansUrl/$Id"
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

function Remove-StudyPlanHard {
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
            Uri = "$Script:StudyPlansUrl/$Id"
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