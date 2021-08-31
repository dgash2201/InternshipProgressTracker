$BaseUrl = "https://localhost:44312/Users"
$ContentType = "application/json"

function Login {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)]
        [string]
        $Email,

        [Parameter(Mandatory)]
        [string]
        $Password
    )
    begin {
        $Uri = "$BaseUrl/login"
    }
    process {
        $body = ConvertTo-Json @{
            email = $Email
            password = $Password
        }

        $parameters = @{
            Uri = $Uri
            Method = "POST"
            Body = $body
            ContentType = $ContentType
        }

        try {
            $response = Invoke-RestMethod @parameters

            $Global:Context = @{
                Token = $response.Model.Jwt
                RerfreshToken = $response.Model.RefreshToken
            }
        }
        catch {
            Write-Host $Error[0] | ConvertTo-Json
        }
    }
}

function Register {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)]
        [String]
        $Email,

        [Parameter(Mandatory)]
        [String]
        $FirstName,

        [Parameter(Mandatory)]
        [String]
        $AvatarPath,

        [Parameter(Mandatory)]
        [String]
        $LastName,

        [Parameter(Mandatory)]
        [String]
        $Password,

        [Parameter(Mandatory)]
        [String]
        $ConfirmPassword
    )
    begin {
        $Uri = "$BaseUrl/register"
    }
    process {
        $form = @{
            Email = $Email
            FirstName = $FirstName
            LastName = $LastName
            Avatar = Get-Item -Path $AvatarPath
            Password = $Password
            ConfirmPassword = $ConfirmPassword
        }

        $parameters = @{
            Uri = $Uri
            Method = "POST"
            Form = $form
        }

        try {
            Invoke-RestMethod @parameters
        }
        catch {
            Write-Host $Error[0] | ConvertTo-Json
        }
    }
}

function Update-Jwt {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)]
        [Int]
        $UserId
    )
    begin {
        $uri = "$BaseUrl/login"
    }
    process {
        $body = ConvertTo-Json @{
            UserId = $UserId
            RefreshToken = $Global:Context.RefreshToken
        }

        $parameters = @{
            Uri = $uri
            Method = "POST"
            Body = $body
            ContentType = $ContentType
        }

        try {
            $response = Invoke-RestMethod @parameters

            $Global:Context = @{
                Token = $response.Model.Jwt
                RerfreshToken = $response.Model.RefreshToken
            }
        }
        catch {
            Write-Host $Error[0] | ConvertTo-Json
        }
    }
} 

function Get-Profile {
    param (
        [Parameter(Mandatory)]
        [Int]
        $UserId
    )
    begin {
        $headers = @{
            Authorization = "Bearer $Global:Context.Token"
        }
    }
    process {
        $parameters = @{
            Uri = "$BaseUrl/$UserId"
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

function Remove-User {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)]
        [Int]
        $UserId
    )
    begin {
        $headers = @{
            Authorization = "Bearer $Global:Context.Token"
        }
    }
    process {
        $parameters = @{
            Uri = "$BaseUrl/$UserId"
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

function Remove-UserHard {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)]
        [Int]
        $UserId
    )
    begin {
        $headers = @{
            Authorization = "Bearer $Global:Context.Token"
        }
    }
    process {
        $parameters = @{
            Uri = "$BaseUrl/$UserId"
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