Import-Module ".\SharedData.psm1"
$Script:StudentsUrl = "$($Script:Context.BaseUrl)/Students"

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
        $uri = "$Script:StudentsUrl/add-notes"
        $headers = @{
            Authorization = "Bearer $($Script:Context.Token)"
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

function Set-StudyPlanEntryAsStarted {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)]
        [Int]
        $StudyPlanEntryId
    )
    
    begin {
        $uri = "$Script:StudentsUrl/start-study-plan-entry"
        $headers = @{
            Authorization = "Bearer $($Script:Context.Token)"
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

function Set-StudyPlanEntryAsFinished {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)]
        [Int]
        $StudyPlanEntryId
    )
    
    begin {
        $uri = "$Script:StudentsUrl/finish-study-plan-entry"
        $headers = @{
            Authorization = "Bearer $($Script:Context.Token)"
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