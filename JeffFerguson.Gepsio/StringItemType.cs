
namespace JeffFerguson.Gepsio
{
    public class StringItemType : ComplexType
    {
        internal StringItemType()
            : base("stringItemType", new String(), new NonNumericItemAttributes())
        {
        }

        internal override void ValidateFact(Fact FactToValidate)
        {
            base.ValidateFact(FactToValidate);
        }
    }
}
