namespace Authentication_Service.Enums
{
    public enum AccountRole
    {
        Undefined = 0,
        User = 1,
        Admin = 2,
        SiteAdmin = 3
    }

    public enum Gender
    {
        Female,
        Male
    }

    public enum DisableReason
    {
        Undefined = 0,
        EmailVerificationRequired = 1,
        AccountViolation = 2,
        ToManyFailedLoginAttempts = 3
    }
}
