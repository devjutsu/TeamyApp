using AutoMapper;
using Teamy.Server.Data;
using Teamy.Server.Services;

namespace Teamy.Server.Logic
{
    public interface IManageEvents 
    {
        Task Test();
    }
    public class EventLogic : IManageEvents
    {
        ILogger<EventLogic> _logger { get; set; }
        TeamyDbContext _db { get; set; }
        private readonly IMapper _mapper;
        IVoteHub _hub;
        public EventLogic(ILogger<EventLogic> logger,
                                    TeamyDbContext db,
                                    IMapper mapper,
                                    IVoteHub hub)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
            _hub = hub;
        }

        public async Task Test()
        {
            
        }
    }

}