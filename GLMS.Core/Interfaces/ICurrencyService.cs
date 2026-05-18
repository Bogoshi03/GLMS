namespace GLMS.Core.Interfaces
{
    public interface ICurrencyService
    {
        Task<decimal> ConvertUsdToZar(decimal usdAmount);
    }
}
