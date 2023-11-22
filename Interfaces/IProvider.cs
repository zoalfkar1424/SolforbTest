using SolforbTest.Models;

namespace SolforbTest.Interfaces
{
    public interface IProvider
    {
        public List<Provider> GetAllProviders(string SearchText = "");
        Provider GetProvider(int id);

        bool Create(Provider Provider);

        bool Edit(Provider Provider);

        bool Delete(Provider Provider);

        public string GetErrors();
    }
}
