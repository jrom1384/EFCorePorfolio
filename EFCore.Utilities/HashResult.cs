namespace EFCore.Utilities
{
    public enum HashResult
    {
        None,
        Error_NullHash,
        Error_EmptyOrWhiteSpaceHash,
        Error_HashLengthNot64,
        Error_SaltLengthNot128,
        Failed,
        Success
    }
}
