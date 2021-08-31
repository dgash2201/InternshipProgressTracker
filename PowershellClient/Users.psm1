Import-Module ".\SharedData.psm1"
$UsersUrl = "$($Script:Context.BaseUrl)/Users"

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
        $Uri = "$UsersUrl/login"
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
            ContentType = $Script:JsonContentType
        }

        try {
            $response = Invoke-RestMethod @parameters
            $Script:Context.Token = $response.Model.Jwt
            $Script:Context.RefreshToken = $response.Model.RefreshToken

            $response | ConvertTo-Json
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

        [Parameter(Mandatory=$false)]
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
        $Uri = "$UsersUrl/register"
    }
    process {
        if ($AvatarPath) {
            $avatar = Get-Item -Path $AvatarPath
        }

        $form = @{
            Email = $Email
            FirstName = $FirstName
            LastName = $LastName
            Avatar = $avatar
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
        $uri = "$UsersUrl/login"
    }
    process {
        $body = ConvertTo-Json @{
            UserId = $UserId
            RefreshToken = $Script:Context.RefreshToken
        }

        $parameters = @{
            Uri = $uri
            Method = "POST"
            Body = $body
            ContentType = $Script:JsonContentType
        }

        try {
            $response = Invoke-RestMethod @parameters
            $Script:Context.Token = $response.Model.Jwt
            $Script:Context.RefreshToken = $response.Model.RefreshToken
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
            Authorization = "Bearer $($Script:Context.Token)"
        }
    }
    process {
        $parameters = @{
            Uri = "$UsersUrl/$UserId"
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
            Authorization = "Bearer $($Script:Context.Token)"
        }
    }
    process {
        $parameters = @{
            Uri = "$UsersUrl/$UserId"
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
            Authorization = "Bearer $($Script:Context.Token)"
        }
    }
    process {
        $parameters = @{
            Uri = "$UsersUrl/$UserId"
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