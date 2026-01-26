namespace CommitteesManagement.Aggregator.Controllers
{
    public class CommitteeController : BaseController
    {
        private readonly ICommitteeService _committeeService;

        public CommitteeController(ICommitteeService committeeService)
        {
            _committeeService = committeeService;
        }

        [HttpGet("GetAllCommittees")]
        public async Task<ResponseDTO> GetAllCommittees(RequestStatus FilterByStatues, int? PageSize, int? PageNumber, DelegationStatus delegationStatus)
        {
            PaginationModel paginationModel = new PaginationModel(PageNumber, PageSize);

            return await _committeeService.GetAllCommittees(paginationModel, FilterByStatues, delegationStatus);
        }

        [HttpGet("GetAllCommitteeMembers/{commiteeId}")]
        public async Task<ResponseDTO> GetAllCommitteeMembers(int PageSize, int PageNumber, Guid commiteeId)
        {
            return await _committeeService.GetAllCommitteeMembers(PageSize, PageNumber, commiteeId);
        }

        [HttpGet("GetCommitteeById/{commiteeId}")]
        public async Task<ResponseDTO> GetCommitteeById(Guid commiteeId)
        {
            return await _committeeService.GetCommitteeById(commiteeId);
        }

        [HttpGet("GetNotApprovedCommitteeById/{commiteeId}")]
        public async Task<ResponseDTO> GetNotApprovedCommitteeById(Guid commiteeId)
        {
            return await _committeeService.GetNotApprovedCommitteeById(commiteeId);
        }

        [HttpPut("AssignWorkflowToCommittee")]
        public async Task<ResponseDTO> AssignWorkflowToCommittee(AssignWorkflowToCommittee assignWorkflowToCommittee)
        {
            return await _committeeService.AssignWorkflowToCommittee(assignWorkflowToCommittee);
        }

        [HttpGet("GetRequestiInstanceIdsByCommitteeNumber/{commiteeNumber}")]
        public async Task<ResponseDTO> GetRequestInstanceIdsByNumber(long commiteeNumber)
        {
            return await _committeeService.GetRequestInstanceIdsByNumber(commiteeNumber);
        }
    }
}
