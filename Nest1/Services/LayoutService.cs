using Nest1.DAL;

namespace Nest1.Services
{
    public class LayoutService
    {
        AppDBContext _context;
        public LayoutService(AppDBContext dBContext)
        {
            _context = dBContext;
        }
        public async Task<Dictionary<string,string>> GetSetting()
        {
            var setting = _context.Settings.ToDictionary(x=>x.Key,x=>x.Value);
            return setting;
        }
    }
}
