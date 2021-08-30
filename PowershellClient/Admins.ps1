$Script:BaseUrl = "https://localhost:44312/Admins"

function Get-Users {
    [CmdletBinding()]
    param (
    )
    
    begin {
        $uri = "$Script:BaseUrl/get-users"
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

function New-Admin {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)]
        [Int]
        $UserId
    )
    begin {
        $uri = "$Script:BaseUrl/create-admin"
        $headers = @{
            Authorization = "Bearer $($Global:Context.Token)"
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
        $uri = "$Script:BaseUrl/create-mentor"
        $headers = @{
            Authorization = "Bearer $($Global:Context.Token)"
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
        }

        try {
            Invoke-RestMethod @parameters
        }
        catch {
            Write-Host $Error[0] | ConvertTo-Json
        }
    }
}