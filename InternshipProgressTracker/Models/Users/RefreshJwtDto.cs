/// <summary>
/// Contains data for refreshing the JWT 
/// </summary>
public class RefreshJwtDto
{
    public int UserId { get; set; }

    public string RefreshToken { get; set; }
}