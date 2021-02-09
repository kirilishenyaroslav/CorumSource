
using Corum.DAL.Entity;


namespace Corum.DAL
{
    public class EFBaseCorumDataProvider
    {
        protected Entities db;
        
        public EFBaseCorumDataProvider(Entities dbContext)
        {
            this.db = dbContext;
        }
    }
}
