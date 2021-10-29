namespace System.ComponentModel.DataAnnotations;
/// <summary>
/// Value of Guid is not null nor Guid.Empty.
/// On asp.net core, if there is a parameter of Guid type, and there is no value for it, the value is Guid.Empty, but [Required] doesn't take Guid.Empty as invalid,
/// so please add  RequiredGuidAttribute to a parameter, property or field.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class RequiredGuidAttribute : ValidationAttribute
{
    public const string DefaultErrorMessage = "The {0} field is requird and not Guid.Empty";
    public RequiredGuidAttribute() : base(DefaultErrorMessage) { }

    public override bool IsValid(object value)
    {
        if (value is null)
        {
            return false;
        }
        if (value is Guid)
        {
            Guid guid = (Guid)value;
            return guid != Guid.Empty;
        }
        else
        {
            return false;
        }
    }
}