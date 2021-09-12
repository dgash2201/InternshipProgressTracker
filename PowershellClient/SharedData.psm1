$Script:Context = @{
    BaseUrl = "https://localhost:44312"
    Token = ""
    RefreshToken = ""
    UserId = 0
}

$Script:JsonContentType = "application/json"

Export-ModuleMember -Variable Context
Export-ModuleMember -Variable JsonContentType