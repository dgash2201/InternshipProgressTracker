Import-Module ".\SharedData.psm1"
$Script:InternshipStreamsUrl = "$($Script:Context.BaseUrl)/InternshipStreams"

function Get-InternshipStreams {
    [CmdletBinding()]
    param (
    )
    
    begin {
        $uri = $Script:InternshipStreamsUrl
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
            Write-Host $Error[0] | ConvertTo-Json
        }
    }
}

function Get-InternshipStream {
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
        $uri = "$Script:InternshipStreamsUrl/$Id"

        $parameters = @{
            Uri = $uri
            Method = "GET"
            Headers = $headers
        }

        try {
            Invoke-RestMethod @parameters | ConvertTo-Json
        }
        catch {
            Write-Host $Error[0] | ConvertTo-Json
        }
    }
}

enum InternshipStreamStatus
{
    NotStarted
    Active
    Completed
}

function New-InternshipStream {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)]
        [String]
        $Title,

        [Parameter(Mandatory)]
        [InternshipStreamStatus]
        $Status,

        [Parameter(Mandatory = $false)]
        [String]
        $Description,

        [Parameter(Mandatory = $false)]
        [DateTime]
        $PlanStartDate,

        [Parameter(Mandatory = $false)]
        [DateTime]
        $FactStartDate,

        [Parameter(Mandatory = $false)]
        [DateTime]
        $PlanEndDate,

        [Parameter(Mandatory = $false)]
        [DateTime]
        $FactEndDate
    )
    begin {
        $uri = $Script:InternshipStreamsUrl
    }
    process {
        $headers = @{
            Authorization = "Bearer $($Script:Context.Token)"
        }

        $body = ConvertTo-Json @{
            Title = $Title
            Status = $Status
            Description = $Description
            PlanStartDate = $PlanStartDate
            FactStartDate = $FactStartDate
            PlanEndDate = $PlanEndDate
            FactEndDate = $FactEndDate
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
            Write-Host $Error[0] | ConvertTo-Json
        }
    }
}

function Set-InternshipStream {
    [CmdletBinding()]
    param (
        [Parameter]
        [Int]
        $Id,

        [Parameter(Mandatory)]
        [String]
        $Title,

        [Parameter(Mandatory)]
        [Int]
        $Status,

        [Parameter(Mandatory = $false)]
        [String]
        $Description,

        [Parameter(Mandatory = $false)]
        [DateTime]
        $PlanStartDate,

        [Parameter(Mandatory = $false)]
        [DateTime]
        $FactStartDate,

        [Parameter(Mandatory = $false)]
        [DateTime]
        $PlanEndDate,

        [Parameter(Mandatory = $false)]
        [DateTime]
        $FactEndDate
    )
    begin {        
        $headers = @{
            Authorization = "Bearer $($Script:Context.Token)"
        }
    }
    process {
        $uri = "$Script:InternshipStreamsUrl/$Id"      

        $body = ConvertTo-Json @{
            Title = $Title
            Status = $Status
            Description = $Description
            PlanStartDate = $PlanStartDate
            FactStartDate = $FactStartDate
            PlanEndDate = $PlanEndDate
            FactEndDate = $FactEndDate
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
            Write-Host $Error[0] | ConvertTo-Json
        }
    }
}

function Remove-InternshipStream {
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
            Uri = "$Script:InternshipStreamsUrl/$Id"
            Method = "DELETE"
            Headers = $headers
        }

        try {
            Invoke-RestMethod @parameters | ConvertTo-Json
        }
        catch {
            Write-Host $Error[0] | ConvertTo-Json
        }
    }
    
}

function Remove-InternshipStreamHard {
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
            Uri = "$Script:InternshipStreamsUrl/$Id"
            Method = "DELETE"
            Headers = $headers
        }

        try {
            Invoke-RestMethod @parameters | ConvertTo-Json
        }
        catch {
            Write-Host $Error[0] | ConvertTo-Json
        }
    }
}

function Add-MentorToStream {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)]
        [Int]
        $StreamId,

        [Parameter(Mandatory)]
        [Int]
        $MentorId
    )
    
    begin {
        $uri = "$Script:InternshipStreamsUrl/add-mentor"
        $headers = @{
            Authorization = "Bearer $($Script:Context.Token)"
        }
    }
    
    process {
        $body = ConvertTo-Json @{
            InternshipStreamId = $StreamId
            MentorId = $MentorId
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
            Write-Host $Error[0] | ConvertTo-Json
        }
    }
}

function Add-StudentToStream {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)]
        [Int]
        $StreamId,

        [Parameter(Mandatory)]
        [Int]
        $StudentId
    )
    
    begin {
        $uri = "$Script:InternshipStreamsUrl/add-student"
        $headers = @{
            Authorization = "Bearer $($Script:Context.Token)"
        }
    }
    
    process {
        $body = ConvertTo-Json @{
            InternshipStreamId = $StreamId
            StudentId = $StudentId
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
            Write-Host $Error[0] | ConvertTo-Json
        }
    }
}

function Remove-MentorFromStream {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)]
        [Int]
        $StreamId,

        [Parameter(Mandatory)]
        [Int]
        $MentorId
    )
    
    begin {
        $uri = "$Script:InternshipStreamsUrl/remove-mentor"
        $headers = @{
            Authorization = "Bearer $($Script:Context.Token)"
        }
    }
    
    process {
        $body = ConvertTo-Json @{
            InternshipStreamId = $StreamId
            MentorId = $MentorId
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
            Write-Host $Error[0] | ConvertTo-Json
        }
    }
}

function Remove-StudentFromStream {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)]
        [Int]
        $StreamId,

        [Parameter(Mandatory)]
        [Int]
        $StudentId
    )
    
    begin {
        $uri = "$Script:InternshipStreamsUrl/remove-student"
        $headers = @{
            Authorization = "Bearer $($Script:Context.Token)"
        }
    }
    
    process {
        $body = ConvertTo-Json @{
            InternshipStreamId = $StreamId
            StudentId = $StudentId
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
            Write-Host $Error[0] | ConvertTo-Json
        }
    }
}