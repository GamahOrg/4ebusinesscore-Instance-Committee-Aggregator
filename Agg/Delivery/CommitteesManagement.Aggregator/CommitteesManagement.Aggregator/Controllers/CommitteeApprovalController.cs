namespace CommitteesManagement.Aggregator.Controllers
{
	public class CommitteeApprovalController:BaseController
	{
		private readonly ICommitteeService _committeeService;

		public CommitteeApprovalController(ICommitteeService committeeService)
		{
			_committeeService = committeeService;
		}

		[HttpGet("GetAllCommitteeApprovals")]
		public async Task<ResponseDTO> GetAllCommitteeApprovals(int pageSize,int pageNumber,string searchTerm = "")
		{
			return await _committeeService.GetAllCommitteeApprovals(pageSize,pageNumber,searchTerm);
		}

		[HttpGet("GetCommitteeApprovalById/{commiteeId}")]
		public async Task<ResponseDTO> GetCommitteeApprovalById(Guid commiteeId)
		{
			return await _committeeService.GetApprovalsCommitteeById(commiteeId);
		}
	}
}
