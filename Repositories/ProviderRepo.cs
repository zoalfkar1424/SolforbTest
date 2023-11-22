using Microsoft.EntityFrameworkCore;
using SolforbTest.DBContext;
using SolforbTest.Interfaces;
using SolforbTest.Models;

namespace SolforbTest.Repositories
{
    public class ProviderRepo : IProvider
    {
        private readonly ApplicationDBcontext _context;
        private string _errors = "";
        public ProviderRepo(ApplicationDBcontext context)
        {
            _context = context;
        }
        public List<Provider> GetAllProviders(string SearchText = "")
        {
            IQueryable<Provider> Providers = _context.Provider;

            if (SearchText != "" && SearchText != null)
            {
                Providers = Providers.Where(n => n.Name.Contains(SearchText))
                    ;
            }
            return Providers.ToList();
        }
        public Provider GetProvider(int id)
        {
            Provider Provider = _context.Provider.Where(u => u.Id == id).FirstOrDefault();
            return Provider;
        }
        public bool Create(Provider Provider)
        {
            try
            {
                _context.Provider.Add(Provider);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _errors = "Create Failed - Sql Exception Occured , Error Info : " + ex.Message;
                return false;
            }
        }

        public bool Edit(Provider Provider)
        {
            try
            {
                _context.Provider.Attach(Provider);
                _context.Entry(Provider).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _errors = "Update Failed - Sql Exception Occured , Error Info : " + ex.Message;
                return false;
            }
        }

        public bool Delete(Provider Provider)
        {
            try
            {
                _context.Provider.Attach(Provider);
                _context.Entry(Provider).State = EntityState.Deleted;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                if (ex.InnerException != null)
                    _errors = "Delete Failed - Sql Exception Occured , Error Info : " + ex.InnerException.Message;
                else
                    _errors = "Delete Failed - Sql Exception Occured , Error Info : " + ex.Message;
                return false;
            }
        }

        public string GetErrors()
        {
            return _errors;
        }
    }
}
