

namespace CompanyTraining.Repositories
{
    public class ChoiceRepository : Repository<Choice>, IChoiceRepository
    {
        private readonly ApplicationDbContext context;

        public ChoiceRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

       
    }
}
