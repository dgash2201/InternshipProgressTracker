Import-Module ".\SharedData.psm1"
$Script:MentorsUrl = "$($Script:Context.BaseUrl)/Mentors"

function Set-Notes {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)]
        [Int]
        $StudentId,

        [Parameter(Mandatory)]
        [Int]
        $StudyPlanEntryId,

        [Parameter(Mandatory)]
        [String]
        $Notes
    )
    
    begin {
        $uri = "$Script:MentorsUrl/add-notes"
        $headers = @{
            Authorization = "Bearer $($Script:Context.Token)"
        }
    }
    
    process {
        $body = ConvertTo-Json @{
            StudentId = $StudentId
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
            Invoke-RestMethod @parameters
        }
        catch {
            Write-Host $Error[0] | ConvertTo-Json
        }
    }
}

function Set-ProgressGrade {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)]
        [Int]
        $StudentId,

        [Parameter(Mandatory)]
        [Int]
        $StudyPlanEntryId,

        [Parameter(Mandatory)]
        [Int]
        $GradingMentorId,

        [Parameter(Mandatory)]
        [ValidateRange(1,10)]
        [Int]
        $Grade
    )
    
    begin {
        $uri = "$Script:MentorsUrl/grade-progress"
        $headers = @{
            Authorization = "Bearer $($Script:Context.Token)"
        }
    }
    
    process {
        $body = ConvertTo-Json @{
            StudentId = $StudentId
            StudyPlanEntryId = $StudyPlanEntryId
            GradingMentorId = $GradingMentorId
            Grade = $Grade
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

enum StudentGrade {
    Junior
    Middle
    Senior
}

function Set-StudentGrade {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)]
        [Int]
        $StudentId,

        [Parameter(Mandatory)]
        [ValidateRange(1,10)]
        [StudentGrade]
        $Grade
    )
    
    begin {
        $uri = "$Script:MentorsUrl/set-grade"
        $headers = @{
            Authorization = "Bearer $($Script:Context.Token)"
        }
    }
    
    process {
        $body = ConvertTo-Json @{
            StudentId = $StudentId
            Grade = $Grade
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