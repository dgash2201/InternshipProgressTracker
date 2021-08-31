Import-Module ".\SharedData.psm1"
$Script:AdminsUrl = "$($Script:Context.BaseUrl)/Admins"

function Get-Users {
    [CmdletBinding()]
    param (
    )
    
    begin {
        $uri = "$Script:AdminsUrl/get-users"
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

function New-Admin {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)]
        [Int]
        $UserId
    )
    begin {
        $uri = "$Script:AdminsUrl/create-admin"
        $headers = @{
            Authorization = "Bearer $($Script:Context.Token)"
        }
    }
    process {
        $body = ConvertTo-Json @{
            UserId = $UserId
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

enum MentorRole {
    Mentor
    Lead
}

function New-Mentor {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)]
        [Int]
        $UserId,

        [Parameter(Mandatory)]
        [MentorRole]
        $Role
    )
    begin {
        $uri = "$Script:AdminsUrl/create-mentor"
        $headers = @{
            Authorization = "Bearer $($Script:Context.Token)"
        }
    }
    process {
        $body = @{
            UserId = $UserId
            Role = $Role
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