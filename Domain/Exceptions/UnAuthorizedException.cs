namespace Domain.Exceptions;

public class UnAuthorizedException (string msg = "Invalid email or password"): Exception(msg)
{
    
}